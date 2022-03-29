using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpToDocu.Spec {
    public class SpecificationCommon : UpToDocu.UTDSpecification {
        public static SpecificationCommon Instance => UpToDocu.UTDSpecification.GetInstance(() => new SpecificationCommon());

        public SpecificationCommon() :base(){
            this.Kind = this.Specification;
        }

        public UTDObject Specification => this.Define(() => UTD.UTDObject());
        public UTDObject Class => this.Define(()=> UTD.UTDObject());
        public UTDObject Method => this.Define(()=> UTD.UTDObject());
    }
}
