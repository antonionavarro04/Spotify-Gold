namespace ENT {
    public class ClsLog {

        #region Fields and Properties

        public string Message { get; set; }
        public string Receiver { get; set; }
        public DateTime Date { get; }

        #endregion
        #region Constructors

        public ClsLog() {
            this.Message = string.Empty;
            this.Receiver = string.Empty;
            this.Date = DateTime.Now;
        }

        public ClsLog(string receiver, string message) {
            this.Message = message;
            this.Receiver = receiver;
            this.Date = DateTime.Now;
        }

        #endregion
        #region Methods

        public override string ToString() {
            return $"[{this.Date}] {this.Receiver}: {this.Message}";
        }

        #endregion
    }
}
