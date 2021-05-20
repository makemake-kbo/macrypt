using macrypt.data;
using System.Collections.Generic;

namespace macrypt.Miner
{
    public interface IBlockMiner
    {
        List<block> blockchain { get; }

        void Start();

        void Stop();
    }
}
