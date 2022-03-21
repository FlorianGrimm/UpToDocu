using Microsoft.AspNetCore.Mvc.Testing;

namespace UpToDocu {
    public class UpToDocuWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class {
        public UpToDocuWebApplicationFactory() {
        }
    }
}
