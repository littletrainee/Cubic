using System;
using System.Collections.Generic;
using System.Reflection;

namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.EnumExtend

  internal class EnumExtend {
    public static Dictionary<string, byte> EnumToDictionary(Enum enumeration) {
      FieldInfo[] fields = enumeration.GetType().GetFields(BindingFlags.Static | BindingFlags.Public);
      Enum[] array = new Enum[fields.Length];
      Dictionary<string, byte> dictionary = new Dictionary<string, byte>();
      for (int i = 0; i < fields.Length; i++) {
        dictionary.Add(fields[i].Name, (byte)fields[i].GetValue(enumeration));
      }
      return dictionary;
    }
  }

}
