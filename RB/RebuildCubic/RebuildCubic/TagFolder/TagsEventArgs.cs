using System;

namespace RebuildCubic.TagFolder {
  internal class TagsEventArgs :EventArgs {
    public string errorCode { get; set; }
    public Tag tag;
    internal TagsEventArgs(byte[] packetData) {
      tag = new Tag();
      int num = packetData[0];
      byte[] array = new byte[packetData.Length - 1];
    }
  }
}
