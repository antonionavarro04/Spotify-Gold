namespace ENT.Dto.Metadata
{
    public class DtoAuthorResponse
    {
        #region Explicit Fields

        private string id;

        #endregion
        #region Fields and Properties

        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                Url = $"https://www.youtube.com/channel/{value}";
            }
        }
        public string Name { get; set; }
        public string Url { get; private set; }

        #endregion
        #region Contructors

        public DtoAuthorResponse()
        {
            id = string.Empty;
            Name = string.Empty;
            Url = string.Empty;
        }

        #endregion
    }
}