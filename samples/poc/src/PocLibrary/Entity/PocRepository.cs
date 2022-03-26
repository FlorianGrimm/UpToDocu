using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Entity {
    public class PocRepository {
        public readonly PocContext Context;

        public PocRepository(PocContext pocContext) {
            this.Context = pocContext;
        }


    }
}
