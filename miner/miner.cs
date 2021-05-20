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
        private CancellationTokenSource cancellationToken;

        public void BlockMiner(Mempool mempool)
        {
            blockchain = new List<block>();
            this.mempool = mempool;
        }

        public void createBlock() {
            var previousBlock = blockchain.LastOrDefault();
            var txList = mempool.returnMempool();

            var newBlock = new block() {
                nonce = 0,
                hash = "",
                previousHash = previousBlock?.hash ?? string.Empty,
                reward = blockReward,
                timestamp = DateTime.Now,
                extdata = "macrypt core",
                txList = txList
            };
        }

        public void mineBlock(block blockToMine) {

        }

        public string createRootHash(IList<string> merkelLeaves) {
            if (merkelLeaves == null || !merkelLeaves.Any()) {
                return string.Empty;
            }
            if (merkelLeaves.Count() == 1) {
                return merkelLeaves.First();
            }

            if (merkelLeaves.Count() % 2 > 0) {
                merkelLeaves.Add(merkelLeaves.Last());
            }

            var merkleBranches = new List<string>();

            for (int i = 0; i < merkelLeaves.Count(); i += 2)
            {
                var leafPair = string.Concat(merkelLeaves[i], merkelLeaves[i + 1]);
                merkleBranches.Add(calculateHash(calculateHash(leafPair)));
            }

            return createRootHash(merkleBranches);
        }

        public static string calculateHash(string rawData)
        {
            // SHA512 for asiic resistance hehe
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] bytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

    }
