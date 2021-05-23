using System;
using macrypt.data;
using macrypt.mempool;
using macrypt.Miner;

namespace macrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("macrypt core V0.1");
            Mempool mempool = new Mempool();
            mempool.addTx("bob", "alice", 10000, 0);

            blockMiner miner = new blockMiner();
            miner.Miner(mempool);

            Console.WriteLine(mempool.returnMempool());

            miner.Start();

        }
    }
}
