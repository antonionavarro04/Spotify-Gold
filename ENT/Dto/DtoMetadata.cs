using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT.Dto {
    public class DtoMetadata {
        #region Explicit Fields

        private string id;
        private List<DtoThumbnail> thumbnails;

        #endregion
        #region Fields and Properties

        public string Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
                this.Url = $"https://www.youtube.com/watch?v={value}";
            }
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; private set; }
        public DtoAuthor Author { get; set; }
        public DateTimeOffset UploadAt { get; set; }
        public TimeSpan? Duration { get; set; }
        public List<DtoThumbnail> Thumbnails {
            get {
                return this.thumbnails;
            }
            set {
                this.thumbnails = value;
                this.OrderThumbnails();
            }
        }
        public DtoEngagement Engagement { get; set; }


        #endregion
        #region Contructors

        public DtoMetadata() {
            this.id = "";
            this.Title = "";
            this.Author = new();
            this.Description = "";
            this.UploadAt = DateTimeOffset.MinValue;
            this.Duration = TimeSpan.Zero;
            this.thumbnails = new();
            this.Engagement = new();
            this.Url = "";
        }

        #endregion
        #region Methods

        private void OrderThumbnails() {
            this.thumbnails = this.thumbnails.OrderByDescending(t => t.Size).ToList();
        }

        #endregion
    }
}
