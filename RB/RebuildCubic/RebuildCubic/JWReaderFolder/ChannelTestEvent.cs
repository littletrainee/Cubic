using System;
using JW.UHF;

namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.ChannelTestEvent

  public class ChannelTestEvent {
    public int Channel;

    public double Forward_Power;

    public double Frequency;

    public double Reverse_Power;

    internal ChannelTestEvent(byte[] data) {
      Channel = data[2];
      Frequency = JW.UHF.Channel.ChannnelToFrequency[Channel];
      byte[] array = new byte[4];
      Array.Copy(data, 3, array, 0, 4);
      string value = Util.ToHexString(array);
      Forward_Power = Convert.ToInt32(value, 16) / 10;
      byte[] array2 = new byte[4];
      Array.Copy(data, 7, array2, 0, 4);
      string value2 = Util.ToHexString(array2);
      Reverse_Power = Convert.ToInt32(value2, 16) / 10;
    }
  }

}
