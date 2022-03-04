﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using dpp.cot;
using NetCoreServer;
using Serilog;

namespace dpp.opentakrouter
{
    public class TakTlsSession : SslSession
    {
        private readonly Router _router;
        public TakTlsSession(TakTlsServer server) : base(server)
        {
            _router = server.Router;
        }
        protected override void OnConnected()
        {
            Log.Information($"id={Id} endpoint={Socket.RemoteEndPoint} state=connected");
        }

        protected override void OnDisconnected()
        {
            Log.Information($"id={Id} state=disconnected");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            try
            {
                var msg = Message.Parse(buffer, (int)offset, (int)size);
                if (msg.Event.IsA(CotPredicates.t_ping))
                {
                    SendAsync(Event.Pong().ToXmlString());
                }

                _router.Send(msg.Event);
            }
            catch (Exception e)
            {
                Log.Error(e, $"id={Id} endpoint={Socket.RemoteEndPoint} type=unknown error=true forwarded=false");
            }
        }

        protected override void OnError(SocketError error)
        {
            Log.Error($"id={Id} error={error}");
        }
    }
}
