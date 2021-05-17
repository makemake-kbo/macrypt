using macrypt.data;
using System.Collections.Generic;

namespace macrypt.Miner
{
    public interface IBlockMiner
    {
        List<block> Blockchain { get; }

        void Start();

        void Stop();
    }
}