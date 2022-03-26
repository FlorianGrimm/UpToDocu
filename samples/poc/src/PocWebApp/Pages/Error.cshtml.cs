using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using System.Diagnostics;

namespace PocWebApp.Pages {
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);

        private readonly ILogger<ErrorModel> _Logger;

        public ErrorModel(ILogger<ErrorModel> logger) {
            this._Logger = logger;
            this.RequestId = string.Empty;
        }

        public void OnGet() {
            this.RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier;
        }
    }
}
