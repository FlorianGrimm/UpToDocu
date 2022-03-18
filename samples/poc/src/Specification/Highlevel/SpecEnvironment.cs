using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpToDocu;

namespace Poc.Highlevel {
    public partial class Spec {
        public UTDEnv SqlServer { get; } = new UTDEnv();
        public UTDEnv WebServer { get; } = new UTDEnv();
        //public UTDEnv User { get; } = new UTDActor();
    }
}
