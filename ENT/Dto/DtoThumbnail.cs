using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT.Dto {
    public class DtoThumbnail {
        #region Fields and Properties

        public string Url { get; set; }
        public long Size { get; set; }

        #endregion
        #region Contructors

        public DtoThumbnail() {
            Url = string.Empty;
        }

        #endregion
    }
}
