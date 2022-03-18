using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UpToDocu;
namespace Poc.Consideration {
    public class Step1 : UTDSpecification {
        public Step1() : base() {
            this.SqlServer = this.Define("MS SQL");
            this.Database = this.Define(SqlServer / "TodoDB");
        }

        public UTDObject SqlServer { get; }
        public UTDObject Database { get; }
    }
}
