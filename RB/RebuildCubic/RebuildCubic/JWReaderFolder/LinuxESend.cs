using System;
using JW.UHF;

namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.LinuxESend

  internal class LinuxESend : ISendPacket {
    public byte[] Assemble(PacketType pt, byte[] data) {
      byte[] array = new byte[data.Length + 6];
      array[0] = byte.MaxValue;
      array[1] = byte.MaxValue;
      array[2] = 0;
      array[3] = (byte)(data.Length + 1);
      array[4] = (byte)pt;
      array[array.Length - 1] = 0;
      Array.Copy(data, 0, array, 5, data.Length);
      array[array.Length - 1] = Util.Checksum(array);
      return array;
    }
  }

}
