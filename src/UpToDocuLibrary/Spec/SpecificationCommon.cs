using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpToDocu.Spec {
    public class SpecificationCommon : UpToDocu.UtdSpecification {
        public static SpecificationCommon Instance => UpToDocu.UtdSpecification.GetInstance(() => new SpecificationCommon());

        public SpecificationCommon() :base(){
            this.Kind = this.Specification;
        }

        public UtdObject Specification => this.Define(() => UTD.UTDObject());
        public UtdObject Class => this.Define(()=> UTD.UTDObject());
        public UtdObject Method => this.Define(()=> UTD.UTDObject());
        public UtdObject Condition => this.Define(() => UTD.UTDObject());
    }
}
