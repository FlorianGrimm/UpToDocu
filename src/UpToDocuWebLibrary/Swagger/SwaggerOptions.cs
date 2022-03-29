
using System;

namespace UpToDocu.Swagger {
    public class SwaggerOptions {
        public SwaggerOptions() {
            this.DocumentName = String.Empty;
            this.OutputPath = String.Empty;
            this.Host = String.Empty;
            this.Basepath = String.Empty;
        }

        public bool Generate { get; set; }
        public string DocumentName { get; set; }
        public string OutputPath { get; set; }
        public bool Yaml { get; set; }
        public bool SerializeasV2 { get; set; }
        public string Host { get; set; }
        public string Basepath { get; set; }
    }
}
