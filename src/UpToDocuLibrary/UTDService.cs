using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

namespace UpToDocu {
    public class UtdService {
        private readonly UTDServiceOptions _Options;

        public UtdService(
            IOptions<UTDServiceOptions> options
            ) {
            this._Options = options.Value;
            var optionsValue = options.Value;
        }

        public UtdBound  Bind(UtdObject utd) {
            return new UtdBound (this, utd);
        }

        public static UtdBound  operator +(UtdService utdService, UtdObject utd) {
            return new UtdBound (utdService, utd);
        }
    }

    public class UTDServiceOptions {
        private readonly List<UtdSpecification> _Specifications;

        public UTDServiceOptions() {
            this._Specifications=new List<UtdSpecification>();
        }

        public UTDServiceOptions AddSpecification(UtdSpecification specification) {
            this._Specifications.Add(specification);
            return this;
        }
    }
}
