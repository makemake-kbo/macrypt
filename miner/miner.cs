using macrypt.Miner;
using macrypt.data;
using macrypt.mempool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace macrypt.Miner {
    
    public class miner : IBlockMiner {
        private static uint blockReward = 6500000;
        private Mempool mempool;
        public List<block> blockchain { get; private set; }

        public string nodeName = "Melachoir";


}
