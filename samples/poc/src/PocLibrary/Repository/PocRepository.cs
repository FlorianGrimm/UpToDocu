
using Poc.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Repository {
    public class PocRepository : IDisposable {
        private PocContext? _Context;
        private TodoRepository? _TodoRepository;
        public PocContext Context => this._Context ?? throw new ObjectDisposedException(nameof(Context));
        public TodoRepository TodoRepository => this._TodoRepository ?? throw new ObjectDisposedException(nameof(TodoRepository));

        public PocRepository(PocContext pocContext) {
            this._Context = pocContext;
            this._TodoRepository = new TodoRepository(this);
        }

        protected virtual void Dispose(bool disposing) {
            using (var context = this._Context) {
                using (var todoRepository = this._TodoRepository) {
                    this._Context = null;
                    this._TodoRepository = null;
                }
            }
        }

        ~PocRepository() {
            Dispose(disposing: false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
