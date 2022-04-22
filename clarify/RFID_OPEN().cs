// JW.UHF.JWReader
using System;
using System.Threading;

public Result RFID_Open() {
  Result result = Result.Wait_TimeOut;
  try {
    client.Connect();
  } catch (UHFException ex) {
    if (client is NetClient) {
      result = Result.Network_Exception;
      SDKLog.Error("{0}Network Exception:", ex, LogHeader);
      return result;
    }
    result = Result.Serial_Exception;
    SDKLog.Error("{0}Serial Exception:", ex, LogHeader);
    return result;
  }
  try {
    client.SetParsePacket(ReaderVersion.V3);
    sendPacket = new LinuxESend();
    for (int i = 0; i < 5; i++) {
      result = Open();
      if (result == Result.OK) {
        break;
      }
    }
    if (result != 0) {
      client.SetParsePacket(ReaderVersion.V2);
      sendPacket = new JWSend();
      result = Open();
    }
    if (result == Result.OK) {
      result = RFID_Get_Product_Info(out pi);
    }
    if (result == Result.OK) {
      string readValue = "";
      result = RFID_Read_OEM("9D", out readValue);
      if (result == Result.OK) {
        RFS_Type = ("00000000".Equals(readValue) ? RFSType.FCC : RFSType.ETSI);
      }
    }
    if (result == Result.OK) {
      FirmwareInfo firmwareInfo = new FirmwareInfo();
      result = RFID_Get_Firmware_Info(out firmwareInfo);
      int num = firmwareInfo.Main_Version * 100 + firmwareInfo.Sub_Version;
      bool flag = false;
      if (pi.CHIP_TYPE == ChipType.M100) {
        if (result == Result.OK && num >= 130) {
          flag = true;
        }
      } else if (result == Result.OK && num >= 125) {
        flag = true;
      }
      if (flag) {
        Thread thread = new Thread(SendKeepAlive);
        thread.Priority = ThreadPriority.AboveNormal;
        thread.IsBackground = true;
        thread.Start();
      } else if (client is NetClient) {
        RFID_Set_HeartBeat();
      }
    }
    if (result != 0) {
      RFID_Close();
    }
  } catch (Exception ex2) {
    RFID_Close();
    GoToError(result, ex2);
  }
  return result;
}
