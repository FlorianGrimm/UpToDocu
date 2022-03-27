
    
using System;
namespace Poc.Repository {
    public partial class PocRepositoryScoped : IDisposable {
        private PocRepository? _PocRepository;
        public PocRepository PocRepository => _PocRepository ?? throw new ObjectDisposedException("PocContext");

        public PocRepositoryScoped(PocRepository pocRepository) {
            this._PocRepository = pocRepository;
        }

        protected virtual void Dispose(bool disposing) {
            using (var pc = this._PocRepository) {
                this._PocRepository = null;
            }
        }

        ~PocRepositoryScoped() {
            this.Dispose(disposing: false);
        }

        public void Dispose() {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
