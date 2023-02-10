using System;

using macrypt.data;
using macrypt.mempool;
using macrypt.Miner;
using macrypt.Server;
using macrypt.PeerStore;

namespace macrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("macrypt core V0.2.1");
            Mempool mempool = new Mempool();
            blockMiner miner = new blockMiner();
            RPCServer server = new RPCServer();
            Peers peers = new Peers();

            server.EmbedServer(mempool, miner, peers);

            server.Start();
            miner.Miner(mempool);
            miner.Start();
        }
    }
}
