namespace COM {
    public class ClsTimer {
        #region Fields & Properties

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; } = DateTime.MinValue;

        #endregion
        #region Constructors

        public ClsTimer() {
            // Empty
        }

        #endregion
        #region Methods

        public void Start() {
			this.StartTime = DateTime.Now;
            this.EndTime = DateTime.MinValue;
		}

        public void Stop() {
            this.EndTime = DateTime.Now;
        }

        public TimeSpan GetElapsedTime() {
			if (this.EndTime == DateTime.MinValue) {
				return DateTime.Now - this.StartTime;
			} else {
				return this.EndTime - this.StartTime;
			}
		}

        #endregion
    }
}
