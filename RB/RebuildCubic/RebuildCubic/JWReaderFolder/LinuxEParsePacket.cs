using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using JW.UHF;
namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.LinuxEParsePacket

  internal class LinuxEParsePacket : IParsePacket {
    private IClient client;

    private Dictionary<string, byte> packetDic = EnumToList();

    private ProducerConsumerStream pcs = new ProducerConsumerStream();

    private Thread ProcessThread = null;

    private JWReader reader;

    public LinuxEParsePacket(IClient _client, JWReader _reader) {
      client = _client;
      reader = _reader;
      ProcessThread = new Thread(loopProcessData);
      ProcessThread.Start();
    }

    private static Dictionary<string, byte> EnumToList() {
      return EnumExtend.EnumToDictionary((PacketType)0);
    }

    private void ErrorProcess(byte[] uart_receivebuffer) {
      if (SDKLog.LogEnable("IsErrorEnabled")) {
        SDKLog.Error("{0}Inner Error Packet:{1},Length={2}", reader.LogHeader, Util.ToHexString(uart_receivebuffer), uart_receivebuffer.Length);
      }
      if (CustomTraceListener.HasConsole) {
        Util.PrintCustomTrace("Inner Packet Header Error");
      }
    }

    private void loopProcessData() {
      while (client.IsAlive()) {
        try {
          if (pcs.DataPosition() < 4) {
            Thread.Sleep(10);
            continue;
          }
          MemoryStream memoryStream = new MemoryStream();
          byte b = (byte)pcs.ReadByte();
          if (b != byte.MaxValue) {
            SDKLog.Error("{0}Outer First Packet Header Error,First Header {1}", reader.LogHeader, b);
            if (CustomTraceListener.HasConsole) {
              Util.PrintCustomTrace("First Packet Header Error");
            }
            memoryStream.Close();
            continue;
          }
          memoryStream.WriteByte(b);
          b = (byte)pcs.ReadByte();
          if (b != byte.MaxValue) {
            SDKLog.Error("{0}Outer Second Packet Header Error,Second Header {1}", reader.LogHeader, b);
            if (CustomTraceListener.HasConsole) {
              Util.PrintCustomTrace("Second Packet Header Error");
            }
            memoryStream.Close();
            continue;
          }
          memoryStream.WriteByte(b);
          byte b2 = (byte)pcs.ReadByte();
          memoryStream.WriteByte(b2);
          int num = pcs.ReadByte();
          memoryStream.WriteByte((byte)num);
          while (pcs.DataPosition() < num + 1) {
            if (!client.IsAlive()) {
              memoryStream.Close();
              break;
            }
            Thread.Sleep(10);
          }
          byte[] array = new byte[num];
          pcs.Read(array, 0, num);
          memoryStream.Write(array, 0, num);
          byte b3 = (byte)pcs.ReadByte();
          memoryStream.WriteByte(b3);
          memoryStream.Flush();
          if (b3 != (Util.Checksum(array) ^ num ^ b2)) {
            byte[] array2 = new byte[memoryStream.Length];
            memoryStream.Seek(0L, SeekOrigin.Begin);
            memoryStream.Read(array2, 0, (int)memoryStream.Length);
            SDKLog.Error("{0}CRC Check Error,Current Data:{1}", reader.LogHeader, Util.ToHexString(array2));
            if (CustomTraceListener.HasConsole) {
              Util.PrintCustomTrace("CRC Check Error");
            }
            memoryStream.Close();
          } else if (packetDic.ContainsValue(array[0])) {
            byte[] array3 = new byte[memoryStream.Length];
            memoryStream.Seek(0L, SeekOrigin.Begin);
            memoryStream.Read(array3, 0, (int)memoryStream.Length);
            memoryStream.Close();
            if (SDKLog.LogEnable("IsDebugEnabled")) {
              SDKLog.Debug("{0}{1}", reader.LogHeader, Util.ToHexString(array3));
            }
            if (CustomTraceListener.HasConsole) {
              Util.PrintCustomTrace(Util.ToHexString(array3));
            }
            if (reader.runningMode == RunningMode.COMMAND) {
              reader.FireCommandResponseEvent(array3);
              continue;
            }
            ParsePacket(array);
            pcs.CopyTo();
          } else {
            SDKLog.Error("{0}Inner Packet Header Error,", reader.LogHeader);
            ErrorProcess(array);
            memoryStream.Close();
          }
        } catch (Exception ex) {
          SDKLog.Error("Package Process Exception:", ex);
          if (CustomTraceListener.HasConsole) {
            Util.PrintCustomTrace("Package Process Error");
          }
        }
      }
      pcs.Close();
    }

    private void ParsePacket(byte[] packetData) {
      if (packetData.Length < 3) {
        return;
      }
      PacketType packetType = (PacketType)packetData[0];
      if (packetType == PacketType.HEART_PACK) {
        if (packetData[2] != byte.MaxValue || packetData[3] != 0) {
          reader.FireKeepAlive();
          return;
        }
        reader.result = ResultEnum.Module_Is_Closed;
        reader.WakeUpInventory();
        reader.WakeUp();
        reader.FireReaderErrorEvent();
      }
      if (packetData[1] == 2) {
        byte b = packetData[2];
        byte b2 = packetData[3];
        if (b == 0 && b2 == 0) {
          reader.result = ResultEnum.OK;
        } else if (b == 0 && b2 == 1 && packetType == PacketType.OPEN_UHF_MODEL) {
          reader.result = ResultEnum.OK;
          reader.sendPacket = new LinuxESend();
        } else if (b == 0 && b2 == 20) {
          reader.result = ResultEnum.Antenna_Not_Configure;
          reader.FireReaderErrorEvent();
        } else if (b == 240 && b2 == 0) {
          reader.result = ResultEnum.OK;
        } else if (b == byte.MaxValue && b2 == 0) {
          reader.result = ResultEnum.Module_Is_Closed;
          reader.WakeUpInventory();
          reader.WakeUp();
          reader.FireReaderErrorEvent();
        } else if (b == byte.MaxValue && b2 == 254) {
          reader.result = ResultEnum.Module_Is_Busy;
          reader.FireReaderErrorEvent();
        } else if (b == 3 && b2 == 9) {
          reader.result = ResultEnum.Reverse_Power_Too_Hign;
          reader.FireReaderErrorEvent();
        } else if (b == 3 && b2 == 54) {
          reader.result = ResultEnum.Forward_Power_InSufficient;
          reader.FireReaderErrorEvent();
        } else {
          reader.result = ResultEnum.Unknown_Exception;
          reader.FireReaderErrorEvent();
        }
        switch (packetType) {
          case PacketType.GROUP_READ_DATA:
          case PacketType.START_INVENTORY:
            reader.WakeUpInventory();
            break;
          case PacketType.STOP_INVENTORY:
            reader.WakeUpInventory();
            reader.WakeUp();
            break;
          default:
            reader.WakeUp();
            break;
        }
        return;
      }
      switch (packetType) {
        case PacketType.READ_MAC_REGISTER:
        case PacketType.READ_OEM_REGISTER:
        case PacketType.READ_STORAGE_BLOCK_DATA:
        case PacketType.GET_STORAGE_BLOCK_COUNT:
        case PacketType.GET_STORAGE_BLOCK_SIZE:
        case PacketType.READ_DATA: {
            byte[] array7 = new byte[packetData[1] - 2];
            Array.Copy(packetData, 4, array7, 0, array7.Length);
            reader.result = ResultEnum.OK;
            reader.SetReadData(Util.ToHexString(array7));
            break;
          }
        case PacketType.SET_GPIO: {
            GPIEvent gpiEvent = new GPIEvent(packetData);
            reader.FireGPIEvent(gpiEvent);
            break;
          }
        case PacketType.GET_MODEL_CONFIGURATION: {
            byte[] array8 = new byte[packetData[1]];
            Array.Copy(packetData, 2, array8, 0, array8.Length);
            RfidSetting modelConfiguration = new RfidSetting(array8, reader);
            reader.result = ResultEnum.OK;
            reader.SetModelConfiguration(modelConfiguration);
            break;
          }
        case PacketType.GET_FIRMWARE_INFO:
        case PacketType.GET_UHF_MODULE_INFO: {
            FirmwareInfo firmwareInfo = new FirmwareInfo();
            firmwareInfo.Main_Version = packetData[4];
            firmwareInfo.Sub_Version = packetData[5];
            FirmwareInfo firmwareInfo2 = firmwareInfo;
            reader.result = ResultEnum.OK;
            reader.SetFirmwareInfo(firmwareInfo2);
            break;
          }
        case PacketType.TEST_CHANNEL:
        case PacketType.GET_OPTIMAL_CHANNEL: {
            if (packetData[2] != 0 || packetData[3] != 0) {
              break;
            }
            List<int> list = new List<int>();
            byte[] array5 = new byte[4];
            Array.Copy(packetData, 4, array5, 0, 4);
            int num3 = Util.ConvertByteToInt(array5, little: true);
            for (int i = 0; i < 32; i++) {
              int num4 = (num3 >> i) & 1;
              if (num4 == 1) {
                list.Add(i);
              }
            }
            byte[] array6 = new byte[4];
            Array.Copy(packetData, 8, array6, 0, 4);
            int num5 = Util.ConvertByteToInt(array6, little: true);
            for (int i = 0; i < 18; i++) {
              int num4 = (num5 >> i) & 1;
              if (num4 == 1) {
                list.Add(i + 32);
              }
            }
            reader.result = ResultEnum.OK;
            reader.SetOptimalChannel(list);
            break;
          }
        case PacketType.GROUP_READ_DATA: {
            TagsEventArgs args2 = new TagsEventArgs(packetData, 1);
            reader.result = ResultEnum.OK;
            reader.FireEvent(args2);
            break;
          }
        case PacketType.FAST_READ: {
            byte[] array3 = new byte[packetData[5] + 1];
            Array.Copy(packetData, 5, array3, 0, array3.Length);
            TagsEventArgs tagsEventArgs = new TagsEventArgs(array3);
            Tag tag = new Tag();
            tag = tagsEventArgs.tag;
            int num = packetData[5] + 7;
            int num2 = packetData[num];
            byte[] array4 = new byte[num2 - 2];
            Array.Copy(packetData, num + 3, array4, 0, array4.Length);
            tag.DATA = Util.ToHexString(array4);
            reader.result = ResultEnum.OK;
            reader.SetFastReadData(tag);
            break;
          }
        case PacketType.CHECK_ANTENNA_CONNECTED:
          reader.AntennaCheckResult.Add(packetData[4]);
          reader.AntennaCheckResult.Add(packetData[5]);
          reader.AntennaCheckResult.Add(packetData[6]);
          reader.AntennaCheckResult.Add(packetData[7]);
          reader.WakeUp();
          break;
        case PacketType.CHANNEL_FORWARD_REVERSE_RESPONSE: {
            ChannelTestEvent channelEvent = new ChannelTestEvent(packetData);
            reader.FireChannelResponseEvent(channelEvent);
            break;
          }
        case PacketType.GET_MODEL_PRODUCT_INFO: {
            ProductInfo productInfo = new ProductInfo(packetData);
            reader.result = ResultEnum.OK;
            reader.SetProductInfo(productInfo);
            break;
          }
        case PacketType.START_INVENTORY: {
            byte[] array2 = new byte[packetData.Length - 1];
            Array.Copy(packetData, 1, array2, 0, array2.Length);
            TagsEventArgs args = new TagsEventArgs(array2);
            reader.result = ResultEnum.OK;
            reader.FireEvent(args);
            break;
          }
        case PacketType.GET_IP_CONFIGURATION: {
            byte[] array = new byte[packetData[1]];
            Array.Copy(packetData, 2, array, 0, array.Length);
            IPConfiguration iPConfiguration = new IPConfiguration(array);
            reader.result = ResultEnum.OK;
            reader.SetIPConfiguration(iPConfiguration);
            break;
          }
      }
    }

    public void ProcessData() {
      byte[] array = client.Receive();
      if (array != null) {
        pcs.Write(array, 0, array.Length);
        pcs.Flush();
      }
    }
  }

}
