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
        private static uint blockReward = 6500000;
        private Mempool mempool;
        public List<block> blockchain { get; private set; }

        public string nodeName = "Melchior";
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
            var txList = mempool.returnMempool();
            txList.Add(new transaction()
            {
                Amount = blockReward,
                From = "coinbase",
                To = nodeName
            });

            var block = new block()
            {
                nonce = 0,
                hash = string.Empty,
                previousHash = blockchain.LastOrDefault()?.hash ?? string.Empty,
                reward = blockReward,
                timestamp = DateTime.Now,
                extdata = "macrypt core",
                txList = txList

            };
            mineBlock(block);
            Console.ReadKey();
            blockchain.Add(block);
            mempool.clearMempool();
        }


        public void mineBlock(block blockToMine)
        {
            var merkleRootHash = FindMerkleRootHash(blockToMine.txList);
            ulong currentNonce = 0;
            var hash = string.Empty;
            Console.WriteLine("Started mining on block");
            Console.WriteLine(blockToMine.txList);
            do
            {
                Console.WriteLine("currentNonce == {0}", currentNonce);
                var rowData = blockToMine.previousHash + currentNonce + merkleRootHash;
                hash = calculateHash(calculateHash(rowData));
                Console.WriteLine("hash == {0}", hash);
                currentNonce++;
            }
            while (!hash.StartsWith("00000"));

            Console.WriteLine("Block finished mining with hash {0} and nonce {1}", hash, currentNonce);
            blockToMine.hash = hash;
            blockToMine.nonce = currentNonce;
        }

        public string createRootHash(IList<string> merkelLeaves)
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

        private string FindMerkleRootHash(IList<transaction> txList)
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
    }

}
