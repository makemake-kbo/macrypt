using System.Collections.Generic;
using System.Linq;
using macrypt.data;

namespace macrypt.mempool {
    public class Mempool {
        private List<transaction> mempool;

        private object lockObj;

        public Mempool() {
            lockObj = new object();
            mempool = new List<transaction>();
        }

        public void addRawTx(transaction transaction) {
            lock (lockObj) {
                mempool.Add(transaction);
            }
        }
        public void addTx(string from, string to, uint amount, uint fee) {
            var transaction = new transaction(from, to, amount, fee);
            lock (lockObj)
            {
                mempool.Add(transaction);
            }
        }


            public List<transaction> returnMempool() {
            lock (lockObj) {
                var all = mempool.ToList();
                return all;
            }
        }

        public void clearMempool() {
            mempool.Clear();
        }

    }
}