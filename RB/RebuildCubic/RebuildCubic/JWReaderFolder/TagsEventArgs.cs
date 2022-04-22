using System;
using JW.UHF;

namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.TagsEventArgs

  public class TagsEventArgs : EventArgs {
    public string errorCode;

    public Tag tag;

    internal TagsEventArgs(byte[] packetData) {
      tag = new Tag();
      int num = packetData[0];
      byte[] array = new byte[packetData.Length - 1];
      Array.Copy(packetData, 1, array, 0, array.Length);
      if (num == 2) {
        tag = null;
        errorCode = Util.ToHexString(array);
        return;
      }
      errorCode = "0000";
      tag.PORT = array[0];
      byte[] array2 = new byte[2];
      Array.Copy(array, 1, array2, 0, 2);
      string value = Util.ToHexString(array2);
      tag.RSSI = (float)Convert.ToInt16(value, 16) / 10f;
      byte[] array3 = new byte[array.Length - 3];
      Array.Copy(array, 3, array3, 0, array3.Length);
      string ePC = Util.ToHexString(array3);
      tag.EPC = ePC;
    }

    internal TagsEventArgs(byte[] packetData, int i) {
      int num = packetData[1];
      if (num == 2) {
        byte[] array = new byte[packetData.Length - 2];
        Array.Copy(packetData, 2, array, 0, array.Length);
        tag = null;
        errorCode = Util.ToHexString(array);
        return;
      }
      byte[] array2 = new byte[packetData[5] + 1];
      Array.Copy(packetData, 5, array2, 0, array2.Length);
      TagsEventArgs tagsEventArgs = new TagsEventArgs(array2);
      tag = new Tag();
      tag = tagsEventArgs.tag;
      int num2 = packetData[5] + 7;
      num = packetData[num2];
      byte[] array3 = new byte[num - 2];
      Array.Copy(packetData, num2 + 3, array3, 0, array3.Length);
      tag.DATA = Util.ToHexString(array3);
      errorCode = "0000";
    }
  }

}
