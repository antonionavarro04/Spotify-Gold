namespace ENT.Dto {
    public class DtoAuthor {
        #region Explicit Fields

        private string id;

        #endregion
        #region Fields and Properties

        public string Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
                this.Url = $"https://www.youtube.com/channel/{value}";
            }
        }
        public string Name { get; set; }
        public string Url { get; private set; }

        #endregion
        #region Contructors

        public DtoAuthor() {
            id = string.Empty;
            Name = string.Empty;
            Url = string.Empty;
        }

        #endregion
    }
}