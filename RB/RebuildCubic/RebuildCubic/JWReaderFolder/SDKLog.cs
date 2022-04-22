using System;
using System.Reflection;
using JW.UHF;

namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.SDKLog

  internal class SDKLog {
    private static object logObj = null;

    public static void Debug(string str) {
      Print(str, null, "IsDebugEnabled", "Debug", null);
    }

    public static void Debug(string str, Exception ex) {
      Print(str, ex, "IsDebugEnabled", "Debug", null);
    }

    public static void Debug(string str, params object[] objs) {
      Print(str, null, "IsDebugEnabled", "Debug", objs);
    }

    public static void Error(string str) {
      Print(str, null, "IsErrorEnabled", "Error", null);
    }

    public static void Error(string str, Exception ex) {
      Print(str, ex, "IsErrorEnabled", "Error", null);
    }

    public static void Error(string str, params object[] objs) {
      Print(str, null, "IsErrorEnabled", "Error", objs);
    }

    public static void Error(string str, Exception ex, params object[] objs) {
      Print(str, ex, "IsErrorEnabled", "Error", objs);
    }

    public static void Info(string str) {
      Print(str, null, "IsInfoEnabled", "Info", null);
    }

    public static void Info(string str, Exception ex) {
      Print(str, ex, "IsInfoEnabled", "Info", null);
    }

    public static void Info(string str, params object[] objs) {
      Print(str, null, "IsInfoEnabled", "Info", objs);
    }

    public static void Init() {
      try {
        string text = "";
        text = Util.CurrentPath;
        if (text.StartsWith("file:\\")) {
          text = text.Substring(6);
        }
        Assembly assembly = Assembly.LoadFrom(text + "\\log4net.dll");
        if (assembly != null) {
          Type type = assembly.GetType("log4net.LogManager");
          object[] args = new object[1] { "SDKLog" };
          logObj = type.InvokeMember("GetLogger", BindingFlags.InvokeMethod, null, null, args);
        }
      } catch (Exception ex) {
        string text2 = ex.ToString();
      }
    }

    public static bool LogEnable(string enableLog) {
      return logObj != null && (bool)logObj.GetType().GetProperty(enableLog).GetValue(logObj, null);
    }

    private static void Print(string str, Exception ex, string enableLog, string logLevel, params object[] objs) {
      if (logObj == null) {
        return;
      }
      try {
        Type type = logObj.GetType();
        if ((bool)type.GetProperty(enableLog).GetValue(logObj, null)) {
          if (ex == null && objs == null) {
            object[] parameters = new object[1] { str };
            type.GetMethod(logLevel, new Type[1] { typeof(string) }).Invoke(logObj, parameters);
          } else if (objs == null) {
            object[] parameters = new object[2] { str, ex };
            type.GetMethod(logLevel, new Type[2]
            {
            typeof(string),
            typeof(Exception)
            }).Invoke(logObj, parameters);
          } else if (ex == null) {
            object[] parameters = new object[1] { string.Format(str, objs) };
            type.GetMethod(logLevel, new Type[1] { typeof(string) }).Invoke(logObj, parameters);
          } else {
            object[] parameters = new object[2]
            {
            string.Format(str, objs),
            ex
            };
            type.GetMethod(logLevel, new Type[2]
            {
            typeof(string),
            typeof(Exception)
            }).Invoke(logObj, parameters);
          }
        }
      } catch (Exception ex2) {
        string text = ex2.ToString();
      }
    }

    public static void Warn(string str) {
      Print(str, null, "IsWarnEnabled", "Warn", null);
    }

    public static void Warn(string str, Exception ex) {
      Print(str, ex, "IsWarnEnabled", "Warn", null);
    }

    public static void Warn(string str, params object[] objs) {
      Print(str, null, "IsWarnEnabled", "Warn", objs);
    }
  }

}
