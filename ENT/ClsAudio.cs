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

        #endregion
        #region Constructors

        public ClsAudio() {
            this.Name = "err-undefined";
            this.Stream = null;
        }

        public ClsAudio(string name, Stream stream) {
            this.Name = name;
            this.Stream = stream;
        }

        #endregion
    }
}
