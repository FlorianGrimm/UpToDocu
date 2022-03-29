namespace UpToDocu {
    public class UTDBoundObject {
        public UTDBoundObject(UTDService uTDService, UTDObject utd) {
            this.UTDService = uTDService;
            this.Utd = utd;
        }

        public UTDService UTDService { get; }
        public UTDObject Utd { get; }
    }
}