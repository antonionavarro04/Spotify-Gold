using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ENT.Dto.Result {
    public class DtoResultResponse {
        #region Fields and Properties

        public string Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Thumbnail { get; set; }

        #endregion
    }
}
