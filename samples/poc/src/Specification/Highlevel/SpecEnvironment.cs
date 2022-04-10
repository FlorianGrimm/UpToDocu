using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpToDocu;

namespace Poc.Highlevel {
    public partial class Spec {
        public UtdEnv SqlServer { get; } = new UtdEnv();
        public UtdEnv WebServer { get; } = new UtdEnv();
        //public UTDEnv User { get; } = new UTDActor();
    }
}
