﻿namespace dpp.opentakrouter
{
    public class TakServerConfig
    {
        public bool Enabled { get; set; } = false;
        public int Port { get; set; }
        public string Cert { get; set; } = "";
        public string Passphrase { get; set; } = "";
    }
}
