using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpToDocu {
    public class UTDService {
        public UTDService() { }
        public UTDBoundObject Bind(UTDObject utd) {
            return new UTDBoundObject(this, utd);
        }

        public static UTDBoundObject operator +(UTDService utdService, UTDObject utd) {
            return new UTDBoundObject(utdService, utd);
        }
    }
}
