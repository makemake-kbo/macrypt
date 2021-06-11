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

namespace macrypt.Miner
{

    public class blockMiner : IBlockMiner
    {
        private static uint blockReward = 6500000; // nagrada za blok
        private Mempool mempool;
        public List<block> blockchain { get; private set; }

        public string nodeName = "Melchior"; // za umrezavanje kako se indentifikuje node
        private CancellationTokenSource cancellationToken;

        public void Miner(Mempool mempool)
        {
            blockchain = new List<block>();
            this.mempool = mempool;
        }

        public void Start()
        {
            cancellationToken = new CancellationTokenSource();
            Console.WriteLine("Mining has started");
            doGenerateBlock();
        }
        public void Stop()
        {
            cancellationToken.Cancel();
            Console.WriteLine("Mining stopped");
        }

        private void doGenerateBlock() {
            while (true) 
            {
                generateBlock();                
            }
        }

        private void generateBlock()
        {
            var lastBlock = blockchain.LastOrDefault();
            List<transaction> emptyTxList = mempool.returnMempool();

            var block = new block()
            {
                nonce = 0,
                hash = string.Empty,
                previousHash = blockchain.LastOrDefault()?.hash ?? string.Empty,
                reward = blockReward,
                timestamp = DateTime.Now,
                extdata = "macrypt core",
                txList = emptyTxList

            };
            mineBlock(block);
            blockchain.Add(block);
            mempool.clearMempool();
        }


        public void mineBlock(block blockToMine)
        {
            var merkleRootHash = FindMerkleRootHash(blockToMine.txList);
            ulong currentNonce = 0;
            var hash = string.Empty;
            Console.WriteLine("Started mining on block");
            do
            {
                var txList = mempool.returnMempool();
                txList.Add(new transaction()
                {
                    Amount = blockReward,
                    From = "coinbase",
                    To = nodeName
                });
                blockToMine.txList = txList;
                hash = getBlockHash(blockToMine, blockToMine.txList, currentNonce);
                currentNonce++;
            }
            while (!hash.StartsWith("00000"));

            Console.WriteLine("Block finished mining with hash {0} and nonce {1}", hash, currentNonce);
            blockToMine.hash = hash;
            blockToMine.nonce = currentNonce;
        }

        public static string createRootHash(IList<string> merkelLeaves)
        {
            if (merkelLeaves == null || !merkelLeaves.Any())
            {
                return string.Empty;
            }
            if (merkelLeaves.Count() == 1)
            {
                return merkelLeaves.First();
            }

            if (merkelLeaves.Count() % 2 > 0)
            {
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

        private static string FindMerkleRootHash(IList<transaction> txList)
        {
            var transactionStrList = txList.Select(tran => calculateHash(calculateHash(tran.From + tran.To + tran.Amount))).ToList();
            return createRootHash(transactionStrList);
        }


        public static string calculateHash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }

        public static string getBlockHash(block blockToHash,  IList<transaction> txList, ulong currentNonce) {
            var rawData = blockToHash.previousHash + currentNonce + FindMerkleRootHash(txList)+ blockToHash.timestamp;
            return calculateHash(calculateHash(rawData));
        }
    }

}
