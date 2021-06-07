using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace macrypt.Server
{
    public interface IRPCServer
    {
        void Stop();

        void Start();
    }
}
