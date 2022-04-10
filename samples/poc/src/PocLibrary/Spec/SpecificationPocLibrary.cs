using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpToDocu;
namespace Poc.Spec {
    public class SpecificationPocLibrary : UtdSpecification {
        public static SpecificationPocLibrary Instance => UpToDocu.UtdSpecification.GetInstance(() => new SpecificationPocLibrary());

        public SpecificationPocLibrary() {
        }
    }
}
