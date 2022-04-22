using System;
using System.IO;
using JW.UHF;
namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.ProducerConsumerStream
  internal class ProducerConsumerStream : Stream {
    private MemoryStream innerStream = new MemoryStream();

    private long readPosition = 0L;

    private long writePosition = 0L;

    public override bool CanRead => true;

    public override bool CanSeek => false;

    public override bool CanWrite => true;

    public override long Length {
      get {
        lock (innerStream) {
          return innerStream.Length;
        }
      }
    }

    public override long Position {
      get {
        throw new NotSupportedException();
      }
      set {
        throw new NotSupportedException();
      }
    }

    public void CopyTo() {
      lock (innerStream) {
        if (writePosition > 204800) {
          int num = (int)DataPosition();
          if (num > 0) {
            SDKLog.Debug("Read Position={0},Write Position={1},Available Length={2},Stream Length={3}", readPosition, writePosition, num, innerStream.Length);
            byte[] buffer = new byte[num];
            Read(buffer, 0, num);
            readPosition = 0L;
            writePosition = 0L;
            Write(buffer, 0, num);
          } else {
            readPosition = 0L;
            writePosition = 0L;
          }
        }
      }
    }

    public long DataPosition() {
      return writePosition - readPosition;
    }

    public override void Flush() {
      lock (innerStream) {
        innerStream.Flush();
      }
    }

    public long GetReadPosition() {
      return readPosition;
    }

    public override int Read(byte[] buffer, int offset, int count) {
      lock (innerStream) {
        innerStream.Position = readPosition;
        int result = innerStream.Read(buffer, offset, count);
        readPosition = innerStream.Position;
        return result;
      }
    }

    public override int ReadByte() {
      lock (innerStream) {
        innerStream.Position = readPosition;
        int result = innerStream.ReadByte();
        readPosition = innerStream.Position;
        return result;
      }
    }

    public override long Seek(long offset, SeekOrigin origin) {
      throw new NotSupportedException();
    }

    public override void SetLength(long value) {
      throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count) {
      lock (innerStream) {
        innerStream.Position = writePosition;
        innerStream.Write(buffer, offset, count);
        writePosition = innerStream.Position;
      }
    }

    public void WriteTo(FileStream fs) {
      innerStream.WriteTo(fs);
    }
  }

}
