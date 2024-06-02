using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT.Dto.Metadata
{
    public class DtoMetadataResponse
    {
        #region Explicit Fields

        private string id;
        private List<DtoThumbnailResponse> thumbnails;

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
                Url = $"https://www.youtube.com/watch?v={value}";
            }
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; private set; }
        public DtoAuthorResponse Author { get; set; }
        public string UploadAt { get; set; }
        public long? Duration { get; set; }
        public List<DtoThumbnailResponse> Thumbnails
        {
            get
            {
                return thumbnails;
            }
            set
            {
                thumbnails = value;
                OrderThumbnails();
            }
        }
        public string Thumbnail { get; set; }
        public DtoEngagementResponse Engagement { get; set; }


        #endregion
        #region Contructors

        public DtoMetadataResponse()
        {
            id = "";
            Title = "";
            Author = new();
            Description = "";
            thumbnails = new();
            Engagement = new();
            Url = "";
        }

        #endregion
        #region Methods

        private void OrderThumbnails()
        {
            thumbnails = thumbnails.OrderByDescending(t => t.Size).ToList();
        }

        #endregion
    }
}
