// JW.UHF.ISendPacket
using JW.UHF;
namespace RebuildCubic.JWReaderFolder {
  internal interface ISendPacket {
    byte[] Assemble(PacketType pt, byte[] data);
  }
}
