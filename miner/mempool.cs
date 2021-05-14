using System.Collections.Generic;
using System.Linq;
using macrypt.data;

namespace Blockchain.miner {
    public class Mempool {
        private List<transaction> mempool;

        private object lockObj;

        public Mempool() {
            lockObj = new object();
            mempool = new List<transaction>();
        }

        public void addRaw(transaction transaction) {
            lock (lockObj) {
                mempool.Add(transaction);
            }
        }
        public void addTx(string from, string to, uint amount) {
            var transaction = new transaction(from, to, amount);
            lock (lockObj)
            {
                mempool.Add(transaction);
            }
        }

        public List<transaction> returnMempool() {
            lock (lockObj) {
                var all = mempool.ToList();
                mempool.Clear();
                return all;
            }
        }
    }
}