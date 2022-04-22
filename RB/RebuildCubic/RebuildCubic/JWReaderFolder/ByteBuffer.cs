using System;

namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.ByteBuffer
  internal class ByteBuffer {
    private const int MAX_LENGTH = 10240;

    private int CURRENT_LENGTH;

    private byte[] TEMP_BYTE_ARRAY;

    public int Length => CURRENT_LENGTH;

    public ByteBuffer() {
      TEMP_BYTE_ARRAY = new byte[10240];
      CURRENT_LENGTH = 0;
      Initialize();
    }

    public ByteBuffer(byte[] bytes) {
      TEMP_BYTE_ARRAY = new byte[10240];
      CURRENT_LENGTH = 0;
      Initialize();
      PushByteArray(bytes);
    }

    public byte GetByteValue(int position) {
      return TEMP_BYTE_ARRAY[position];
    }

    public void Initialize() {
      TEMP_BYTE_ARRAY.Initialize();
      CURRENT_LENGTH = 0;
    }

    public byte[] PopByteArray(int Length) {
      if (Length > CURRENT_LENGTH) {
        return null;
      }
      byte[] array = new byte[Length];
      Array.Copy(TEMP_BYTE_ARRAY, 0, array, 0, Length);
      CURRENT_LENGTH -= Length;
      if (CURRENT_LENGTH > 0) {
        Array.Copy(TEMP_BYTE_ARRAY, Length, TEMP_BYTE_ARRAY, 0, CURRENT_LENGTH);
      }
      return array;
    }

    public void PushByteArray(byte[] ByteArray) {
      ByteArray.CopyTo(TEMP_BYTE_ARRAY, CURRENT_LENGTH);
      CURRENT_LENGTH += ByteArray.Length;
    }

    public byte[] ToByteArray() {
      byte[] array = new byte[CURRENT_LENGTH];
      Array.Copy(TEMP_BYTE_ARRAY, 0, array, 0, CURRENT_LENGTH);
      return array;
    }
  }

}
