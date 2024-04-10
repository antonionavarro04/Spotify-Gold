using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT {
    public class ClsAudio {
        #region Fields and Properties

        public string Name { get; set; }

        public Stream? Stream { get; set; }

        public string? Json { get; set; }

        #endregion
        #region Constructors

        public ClsAudio() {
            this.Name = "err-undefined";
            this.Stream = null;
            this.Json = null;
        }

        public ClsAudio(string name, Stream stream, string json) {
            this.Name = name;
            this.Stream = stream;
            this.Json = json;
        }

        #endregion
    }
}
