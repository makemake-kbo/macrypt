using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;

using macrypt.Miner;
using macrypt.data;
using macrypt.mempool;

namespace macrypt.Server
{
    public class RPCServer : IRPCServer
    {
        private IBlockMiner miner;
        private Mempool mempool;

        private WebServer server;
        private string url;

        public void EmbedServer(Mempool mempool, blockMiner miner)
        {
            url = "http://localhost:6475/";

            server = CreateWebServer(url);
            this.mempool = mempool;
            this.miner = miner;
        }

        public void Stop()
        {
            Console.WriteLine("http server stopped");
        }
        public void Start()
        {
            server.RunAsync();
            Console.WriteLine($"http server available at {url}api");
        }

        private WebServer CreateWebServer(string url)
        {
            var server = new WebServer(o => o
                .WithUrlPrefix(url)
                .WithMode(HttpListenerMode.EmbedIO))
                .WithLocalSessionManager()
                .WithWebApi("/api", m => m.WithController(() => new Controller(miner, mempool)))
                .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" })));

            return server;
        }

        public sealed class Controller : WebApiController
        {
            private readonly IBlockMiner blockMiner;
            private readonly Mempool mempool;

            public Controller(IBlockMiner blockMiner, Mempool mempool)
            {
                this.blockMiner = blockMiner;
                this.mempool = mempool;
            }

            [Route(HttpVerbs.Get, "/blocks")]
            public string GetAllBlocks() => Newtonsoft.Json.JsonConvert.SerializeObject(blockMiner.blockchain);

            [Route(HttpVerbs.Get, "/blocks/index/{index?}")]
            public string GetAllBlocks(int index)
            {
                data.block block = null;
                if (index < blockMiner.blockchain.Count)
                    block = blockMiner.blockchain[index];
                return Newtonsoft.Json.JsonConvert.SerializeObject(block);
            }

            [Route(HttpVerbs.Get, "/blocks/latest")]
            public string GetLatestBlocks()
            {
                var block = blockMiner.blockchain.LastOrDefault();
                return Newtonsoft.Json.JsonConvert.SerializeObject(block);
            }

            //{"From":"alice","To":"bob","Amount":1000000000000}
            [Route(HttpVerbs.Post, "/add")]
            public void AddTransaction()
            {
                var data = HttpContext.GetRequestDataAsync<data.transaction>();
                if (data != null && data.Result != null)
                    mempool.addRawTx(data.Result);
            }
        }
    }
}
