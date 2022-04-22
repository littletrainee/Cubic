using System;
namespace RebuildCubic.JWReaderFolder {
	// JW.UHF.JWSend
	internal class JWSend : ISendPacket {
		public byte[] Assemble(PacketType pt, byte[] data) {
			byte[] array = new byte[data.Length + 1];
			array[0] = (byte)pt;
			Array.Copy(data, 0, array, 1, data.Length);
			return array;
		}
	}

}
