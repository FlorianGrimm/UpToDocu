namespace UpToDocu {
    public class UtdBound : UtdObject {
        public UtdBound (UtdService uTDService, UtdObject spec) {
            this.UTDService = uTDService;
            this.Spec = spec;
        }

        public UtdService UTDService { get; }
        public UtdObject Spec { get; }
    }
}