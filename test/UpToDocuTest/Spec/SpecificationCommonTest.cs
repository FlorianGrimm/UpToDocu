using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
namespace UpToDocu.Spec {
    public class SpecificationCommonTest {
        [Fact]
        public void SpecificationCommon_01_Test() {
            var act = SpecificationCommon.Instance;
            Assert.Equal(act.Specification, act.Kind);
        }
    }
}
