using macrypt.data;
using System.Collections.Generic;

namespace macrypt.Miner
{
    public interface IBlockMiner
    {
        List<block> blockchain { get; }

        block recievedBlock { get; set; }

        string difficulty { get; }

        void Start();

        void Stop();

        string getFinishedBlockHash(block blockToHash);

    }
}
