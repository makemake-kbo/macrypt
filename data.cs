using System;
using System.Collections.Generic;

namespace macrypt.data {

    public class transaction
    {

        public transaction() { }
        public transaction(string from, string to, uint amount)
        {
            this.From = from;
            this.To = to;
            this.Amount = amount;
        }

        public string From { get; set; }
        public string To { get; set; }
        public uint Amount { get; set; }
    }

    public class block {
        public ulong nonce { get; set; }
        public string hash { get; set; }
        public string previousHash { get; set; }
        public uint reward { get; set; }
        public DateTime timestamp { get; set; }
        public string extdata { get; set; }
        public List<transaction> txList { get; set; }
    }

}