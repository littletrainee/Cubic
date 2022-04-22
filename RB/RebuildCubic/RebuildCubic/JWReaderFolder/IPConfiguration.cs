using System;
using JW.UHF;
namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.IPConfiguration

  public class IPConfiguration {
    public bool Dhcp;

    public string Gateway;

    public string IP;

    public int Port;

    public string SubNet_Mask;

    public IPConfiguration() {
    }

    internal IPConfiguration(byte[] data) {
      IP = data[0] + "." + data[1] + "." + data[2] + "." + data[3];
      byte[] array = new byte[4];
      Array.Copy(data, 4, array, 0, 4);
      Port = Util.ConvertByteToInt(array, little: false);
      SubNet_Mask = data[8] + "." + data[9] + "." + data[10] + "." + data[11];
      Gateway = data[12] + "." + data[13] + "." + data[14] + "." + data[15];
      Dhcp = data[16] == 1;
    }
  }

}
