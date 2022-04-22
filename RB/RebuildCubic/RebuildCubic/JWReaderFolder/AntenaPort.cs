using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.AntennaPort
  public class AntennaPort {
    public int AntennaIndex { get; set; }

    public bool Exist { get; internal set; }

    public int Power { get; set; }
  }

}
