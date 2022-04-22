using System;
namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.UHFException

  internal class UHFException : Exception {
    public UHFException(string info)
      : base(info) {
    }

    public UHFException(string info, Exception ex)
      : base(info, ex) {
    }
  }

}
