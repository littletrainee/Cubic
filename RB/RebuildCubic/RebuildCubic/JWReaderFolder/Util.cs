using System;
using System.IO;
using System.Reflection;
using JW.UHF;

namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.Util

  internal class Util {
    private static char[] hexDigits = new char[16]
    {
    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
    'A', 'B', 'C', 'D', 'E', 'F'
    };

    private static string m_CurrentPath;

    public static string CurrentPath {
      get {
        if (Platform.Equals("WinCE")) {
          m_CurrentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        } else if (Platform.Equals("Win32NT")) {
          m_CurrentPath = Directory.GetCurrentDirectory();
        }
        return m_CurrentPath;
      }
    }

    private static string Platform => Environment.OSVersion.Platform.ToString();

    public static byte Checksum(params byte[] val) {
      if (val == null) {
        throw new ArgumentNullException("val");
      }
      byte b = 0;
      foreach (byte b2 in val) {
        b = (byte)(b ^ b2);
      }
      return b;
    }

    internal static int ConvertByteToInt(byte[] bRefArr, bool little) {
      bool flag = little;
      int num = 0;
      int num2 = bRefArr.Length;
      if (flag) {
        for (int i = 0; i < num2; i++) {
          byte b = bRefArr[i];
          num += (b & 0xFF) << 8 * i;
        }
        return num;
      }
      for (int i = 0; i < num2; i++) {
        byte b = bRefArr[i];
        num += (b & 0xFF) << 8 * (num2 - i - 1);
      }
      return num;
    }

    internal static void ConvertIntToByte(byte[] desi, int source) {
      bool flag = false;
      int num = desi.Length;
      if (flag) {
        for (int i = 0; i < num; i++) {
          desi[i] = (byte)(source >> 8 * i);
        }
      } else {
        for (int i = 0; i < num; i++) {
          desi[i] = (byte)(source >> 8 * (num - i - 1));
        }
      }
    }

    public static int DateDiff(DateTime endTime, DateTime startTime) {
      TimeSpan timeSpan = new TimeSpan(endTime.Ticks);
      TimeSpan ts = new TimeSpan(startTime.Ticks);
      return (int)timeSpan.Subtract(ts).Duration().TotalSeconds;
    }

    public static double DateDiffMillSecond(DateTime endTime, DateTime startTime) {
      TimeSpan timeSpan = new TimeSpan(endTime.Ticks);
      TimeSpan ts = new TimeSpan(startTime.Ticks);
      return timeSpan.Subtract(ts).Duration().TotalMilliseconds;
    }

    public static double HexConverToDouble(string hexstring) {
      char[] array = hexstring.ToCharArray();
      double num = 0.0;
      for (int i = 0; i < array.Length; i++) {
        char c = array[i];
        double x = 16.0;
        double y = Convert.ToDouble(array.Length - i - 1);
        switch (c) {
          case '1':
            num += 1.0 * Math.Pow(x, y);
            break;
          case '2':
            num += 2.0 * Math.Pow(x, y);
            break;
          case '3':
            num += 3.0 * Math.Pow(x, y);
            break;
          case '4':
            num += 4.0 * Math.Pow(x, y);
            break;
          case '5':
            num += 5.0 * Math.Pow(x, y);
            break;
          case '6':
            num += 6.0 * Math.Pow(x, y);
            break;
          case '7':
            num += 7.0 * Math.Pow(x, y);
            break;
          case '8':
            num += 8.0 * Math.Pow(x, y);
            break;
          case '9':
            num += 9.0 * Math.Pow(x, y);
            break;
          case 'A':
            num += 10.0 * Math.Pow(x, y);
            break;
          case 'B':
            num += 11.0 * Math.Pow(x, y);
            break;
          case 'C':
            num += 12.0 * Math.Pow(x, y);
            break;
          case 'D':
            num += 13.0 * Math.Pow(x, y);
            break;
          case 'E':
            num += 14.0 * Math.Pow(x, y);
            break;
          case 'F':
            num += 15.0 * Math.Pow(x, y);
            break;
          case 'a':
            num += 10.0 * Math.Pow(x, y);
            break;
          case 'b':
            num += 11.0 * Math.Pow(x, y);
            break;
          case 'c':
            num += 12.0 * Math.Pow(x, y);
            break;
          case 'd':
            num += 13.0 * Math.Pow(x, y);
            break;
          case 'e':
            num += 14.0 * Math.Pow(x, y);
            break;
          case 'f':
            num += 15.0 * Math.Pow(x, y);
            break;
        }
      }
      return num;
    }

    public static void PrintCustomTrace(object info) {
      CustomTraceListener.WriteLine(info);
    }

    public static byte[] ToHexByte(string hexString) {
      hexString = hexString.Replace(" ", "");
      if (hexString.Length % 2 != 0) {
        hexString += " ";
      }
      byte[] array = new byte[hexString.Length / 2];
      for (int i = 0; i < array.Length; i++) {
        array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
      }
      return array;
    }

    public static string ToHexString(byte[] bytes) {
      if (bytes != null) {
        char[] array = new char[bytes.Length * 2];
        for (int i = 0; i < bytes.Length; i++) {
          int num = bytes[i];
          array[i * 2] = hexDigits[num >> 4];
          array[i * 2 + 1] = hexDigits[num & 0xF];
        }
        return new string(array);
      }
      return null;
    }
  }

}
