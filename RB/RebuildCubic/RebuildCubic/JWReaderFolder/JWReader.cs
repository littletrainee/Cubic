using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using JW.UHF;

namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.JWReader
  public class JWReader {
    public delegate void ChannelTestEventHandler(JWReader reader, ChannelTestEvent channelEvent);

    public delegate void CommandResponseEventHandler(JWReader reader, byte[] responseData);

    public delegate void GPIEventHandler(JWReader reader, GPIEvent gpiEvent);

    public delegate void KeepAliveEventHandler(JWReader reader);

    public delegate void ReaderErrorEventHandler(JWReader reader, Result result);

    public delegate void TagsEventArrayHandler(JWReader reader, TagsEventArgs[] args);

    public delegate void TagsEventHandler(JWReader reader, TagsEventArgs args);

    internal List<int> AntennaCheckResult;

    internal Dictionary<int, bool> antennaPortExists;

    internal BackgroundWorker backGroundWorker;

    internal ChipType ChipType;

    private IClient client;

    private FirmwareInfo fi;

    private ManualResetEvent inventoryResetEvent;

    private string ip;

    private IPConfiguration ipConfiguration;

    public bool IsConnected;

    private ManualResetEvent keepAliveResetEvent;

    internal string LogHeader;

    private ManualResetEvent myResetEvent;

    private List<int> optimal_channel;

    private ProductInfo pi;

    private int port;

    private bool processFinish;

    internal ProductType ProductType;

    private string readData;

    internal Result result;

    private RfidSetting rfidSetting;

    public RFSType RFS_Type;

    private bool RUNNING;

    internal RunningMode runningMode;

    internal ISendPacket sendPacket;

    private string serialPort;

    private Tag tagData;

    private List<TagsEventArgs> TagList;

    public event ChannelTestEventHandler channelEventReported;

    public event CommandResponseEventHandler commandResponseEventReported;

    public event GPIEventHandler gpiEventReported;

    public event KeepAliveEventHandler keepAliveEventReported;

    public event ReaderErrorEventHandler readerErrorEventReported;

    public event TagsEventArrayHandler TagsArrayReported;

    public event TagsEventHandler TagsReported;

    public JWReader(string _comPort) {
      runningMode = RunningMode.API;
      rfidSetting = null;
      fi = null;
      readData = "";
      tagData = null;
      pi = null;
      ipConfiguration = null;
      optimal_channel = null;
      myResetEvent = new ManualResetEvent(initialState: false);
      inventoryResetEvent = new ManualResetEvent(initialState: false);
      keepAliveResetEvent = new ManualResetEvent(initialState: false);
      sendPacket = null;
      backGroundWorker = new BackgroundWorker();
      RUNNING = false;
      IsConnected = false;
      RFS_Type = RFSType.FCC;
      AntennaCheckResult = new List<int>();
      LogHeader = "";
      antennaPortExists = new Dictionary<int, bool>();
      ProductType = ProductType.E;
      ChipType = ChipType.R2000;
      TagList = new List<TagsEventArgs>();
      processFinish = true;
      sendPacket = new JWSend();
      serialPort = _comPort;
      client = new SerialClient(serialPort, this, 115200);
      LogHeader = "[" + serialPort + "] ";
      Init();
    }

    public JWReader(string _ip, int _port) {
      runningMode = RunningMode.API;
      rfidSetting = null;
      fi = null;
      readData = "";
      tagData = null;
      pi = null;
      ipConfiguration = null;
      optimal_channel = null;
      myResetEvent = new ManualResetEvent(initialState: false);
      inventoryResetEvent = new ManualResetEvent(initialState: false);
      keepAliveResetEvent = new ManualResetEvent(initialState: false);
      sendPacket = null;
      backGroundWorker = new BackgroundWorker();
      RUNNING = false;
      IsConnected = false;
      RFS_Type = RFSType.FCC;
      AntennaCheckResult = new List<int>();
      LogHeader = "";
      antennaPortExists = new Dictionary<int, bool>();
      ProductType = ProductType.E;
      ChipType = ChipType.R2000;
      TagList = new List<TagsEventArgs>();
      processFinish = true;
      sendPacket = new JWSend();
      ip = _ip;
      port = _port;
      client = new NetClient(Encoding.UTF8, ip, port, this);
      LogHeader = "[" + ip + "] ";
      Init();
    }

    public JWReader(string _comPort, int baudRate, int flag) {
      runningMode = RunningMode.API;
      rfidSetting = null;
      fi = null;
      readData = "";
      tagData = null;
      pi = null;
      ipConfiguration = null;
      optimal_channel = null;
      myResetEvent = new ManualResetEvent(initialState: false);
      inventoryResetEvent = new ManualResetEvent(initialState: false);
      keepAliveResetEvent = new ManualResetEvent(initialState: false);
      sendPacket = null;
      backGroundWorker = new BackgroundWorker();
      RUNNING = false;
      IsConnected = false;
      RFS_Type = RFSType.FCC;
      AntennaCheckResult = new List<int>();
      LogHeader = "";
      antennaPortExists = new Dictionary<int, bool>();
      ProductType = ProductType.E;
      ChipType = ChipType.R2000;
      TagList = new List<TagsEventArgs>();
      processFinish = true;
      sendPacket = new JWSend();
      serialPort = _comPort;
      client = new SerialClient(serialPort, this, baudRate);
      LogHeader = "[" + serialPort + "] ";
      Init();
    }

    public JWReader(string _comPort, int baudRate, int maxKeepAliveTime, int flag) {
      runningMode = RunningMode.API;
      rfidSetting = null;
      fi = null;
      readData = "";
      tagData = null;
      pi = null;
      ipConfiguration = null;
      optimal_channel = null;
      myResetEvent = new ManualResetEvent(initialState: false);
      inventoryResetEvent = new ManualResetEvent(initialState: false);
      keepAliveResetEvent = new ManualResetEvent(initialState: false);
      sendPacket = null;
      backGroundWorker = new BackgroundWorker();
      RUNNING = false;
      IsConnected = false;
      RFS_Type = RFSType.FCC;
      AntennaCheckResult = new List<int>();
      LogHeader = "";
      antennaPortExists = new Dictionary<int, bool>();
      ProductType = ProductType.E;
      ChipType = ChipType.R2000;
      TagList = new List<TagsEventArgs>();
      processFinish = true;
      sendPacket = new JWSend();
      serialPort = _comPort;
      Constants.Max_KeepAlive_Time = maxKeepAliveTime;
      client = new SerialClient(serialPort, this, baudRate);
      LogHeader = "[" + serialPort + "] ";
      Init();
    }

    public JWReader(string _comPort, int baudRate, bool dtrEnable, bool rtsEnable, int flag) {
      runningMode = RunningMode.API;
      rfidSetting = null;
      fi = null;
      readData = "";
      tagData = null;
      pi = null;
      ipConfiguration = null;
      optimal_channel = null;
      myResetEvent = new ManualResetEvent(initialState: false);
      inventoryResetEvent = new ManualResetEvent(initialState: false);
      keepAliveResetEvent = new ManualResetEvent(initialState: false);
      sendPacket = null;
      backGroundWorker = new BackgroundWorker();
      RUNNING = false;
      IsConnected = false;
      RFS_Type = RFSType.FCC;
      AntennaCheckResult = new List<int>();
      LogHeader = "";
      antennaPortExists = new Dictionary<int, bool>();
      ProductType = ProductType.E;
      ChipType = ChipType.R2000;
      TagList = new List<TagsEventArgs>();
      processFinish = true;
      sendPacket = new JWSend();
      serialPort = _comPort;
      client = new SerialClient(serialPort, this, baudRate, dtrEnable, rtsEnable);
      LogHeader = "[" + serialPort + "] ";
      Init();
    }

    public JWReader(string _ip, int _port, int tcpAliveTime, int tcpAliveInterval, int connectTimeout) {
      runningMode = RunningMode.API;
      rfidSetting = null;
      fi = null;
      readData = "";
      tagData = null;
      pi = null;
      ipConfiguration = null;
      optimal_channel = null;
      myResetEvent = new ManualResetEvent(initialState: false);
      inventoryResetEvent = new ManualResetEvent(initialState: false);
      keepAliveResetEvent = new ManualResetEvent(initialState: false);
      sendPacket = null;
      backGroundWorker = new BackgroundWorker();
      RUNNING = false;
      IsConnected = false;
      RFS_Type = RFSType.FCC;
      AntennaCheckResult = new List<int>();
      LogHeader = "";
      antennaPortExists = new Dictionary<int, bool>();
      ProductType = ProductType.E;
      ChipType = ChipType.R2000;
      TagList = new List<TagsEventArgs>();
      processFinish = true;
      sendPacket = new JWSend();
      ip = _ip;
      port = _port;
      client = new NetClient(Encoding.UTF8, ip, port, tcpAliveTime, tcpAliveInterval, connectTimeout, this);
      LogHeader = "[" + ip + "] ";
      Init();
    }

    public JWReader(string _comPort, int baudRate, int maxKeepAliveTime, bool dtrEnable, bool rtsEnable, int flag) {
      runningMode = RunningMode.API;
      rfidSetting = null;
      fi = null;
      readData = "";
      tagData = null;
      pi = null;
      ipConfiguration = null;
      optimal_channel = null;
      myResetEvent = new ManualResetEvent(initialState: false);
      inventoryResetEvent = new ManualResetEvent(initialState: false);
      keepAliveResetEvent = new ManualResetEvent(initialState: false);
      sendPacket = null;
      backGroundWorker = new BackgroundWorker();
      RUNNING = false;
      IsConnected = false;
      RFS_Type = RFSType.FCC;
      AntennaCheckResult = new List<int>();
      LogHeader = "";
      antennaPortExists = new Dictionary<int, bool>();
      ProductType = ProductType.E;
      ChipType = ChipType.R2000;
      TagList = new List<TagsEventArgs>();
      processFinish = true;
      sendPacket = new JWSend();
      serialPort = _comPort;
      Constants.Max_KeepAlive_Time = maxKeepAliveTime;
      client = new SerialClient(serialPort, this, baudRate, dtrEnable, rtsEnable);
      LogHeader = "[" + serialPort + "] ";
      Init();
    }

    public JWReader(string _ip, int _port, int tcpAliveTime, int tcpAliveInterval, int connectTimeout, int maxKeepAliveTime) {
      runningMode = RunningMode.API;
      rfidSetting = null;
      fi = null;
      readData = "";
      tagData = null;
      pi = null;
      ipConfiguration = null;
      optimal_channel = null;
      myResetEvent = new ManualResetEvent(initialState: false);
      inventoryResetEvent = new ManualResetEvent(initialState: false);
      keepAliveResetEvent = new ManualResetEvent(initialState: false);
      sendPacket = null;
      backGroundWorker = new BackgroundWorker();
      RUNNING = false;
      IsConnected = false;
      RFS_Type = RFSType.FCC;
      AntennaCheckResult = new List<int>();
      LogHeader = "";
      antennaPortExists = new Dictionary<int, bool>();
      ProductType = ProductType.E;
      ChipType = ChipType.R2000;
      TagList = new List<TagsEventArgs>();
      processFinish = true;
      sendPacket = new JWSend();
      ip = _ip;
      port = _port;
      Constants.Max_KeepAlive_Time = maxKeepAliveTime;
      client = new NetClient(Encoding.UTF8, ip, port, tcpAliveTime, tcpAliveInterval, connectTimeout, this);
      LogHeader = "[" + ip + "] ";
      Init();
    }

    private void backGroundWorker_DoWork(object sender, DoWorkEventArgs e) {
      this.TagsArrayReported(this, (TagsEventArgs[])e.Argument);
    }

    private void backGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
      processFinish = true;
    }

    private Result Close() {
      result = Result.Wait_TimeOut;
      try {
        result = RFID_Stop_Inventory();
        if (result == Result.OK) {
          byte[] data = (byte[])Constants.Basic_Data.Clone();
          SendData(PacketType.CLOSE_UHF_MODEL, data);
        }
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    ~JWReader() {
      if (client != null && client.IsAlive()) {
        client.Disconnect();
      }
      client = null;
    }

    internal void FireChannelResponseEvent(ChannelTestEvent channelEvent) {
      if (this.channelEventReported != null) {
        this.channelEventReported(this, channelEvent);
      }
    }

    internal void FireCommandResponseEvent(byte[] data) {
      if (runningMode == RunningMode.COMMAND && this.commandResponseEventReported != null) {
        this.commandResponseEventReported(this, data);
      }
    }

    internal void FireEvent(TagsEventArgs args) {
      if (this.TagsReported != null) {
        if (args != null) {
          this.TagsReported(this, args);
        }
      } else if (this.TagsArrayReported != null) {
        if (args != null) {
          TagList.Add(args);
        }
        if (processFinish) {
          processFinish = false;
          TagsEventArgs[] argument = TagList.ToArray();
          TagList.Clear();
          backGroundWorker.RunWorkerAsync(argument);
        }
      }
    }

    internal void FireGPIEvent(GPIEvent gpiEvent) {
      if (this.gpiEventReported != null) {
        this.gpiEventReported(this, gpiEvent);
      }
    }

    internal void FireKeepAlive() {
      if (this.keepAliveEventReported != null) {
        this.keepAliveEventReported(this);
      }
    }

    internal void FireReaderErrorEvent() {
      if (this.readerErrorEventReported != null) {
        this.readerErrorEventReported(this, result);
      }
    }

    public Result Get_ECube_FirmWare_Info(out FirmwareInfo fi) {
      result = Result.Interface_Not_Avaliable;
      if (ProductType == ProductType.E) {
        result = Result.Wait_TimeOut;
        this.fi = null;
        try {
          byte[] data = (byte[])Constants.Basic_Data.Clone();
          SendData(PacketType.GET_FIRMWARE_INFO, data);
        } catch (Exception ex) {
          GoToError(result, ex);
        }
      }
      fi = this.fi;
      return result;
    }

    private void GoToError(Result result, Exception ex) {
      result = Result.Unknown_Exception;
      FireReaderErrorEvent();
      SDKLog.Error("{0}Unknown Exception:", ex, LogHeader);
    }

    private void Init() {
      SDKLog.Init();
      backGroundWorker.RunWorkerCompleted += backGroundWorker_RunWorkerCompleted;
      backGroundWorker.DoWork += backGroundWorker_DoWork;
    }

    public Result IP_Get_Configuration(out IPConfiguration ipConfiguration) {
      result = Result.Interface_Not_Avaliable;
      ipConfiguration = null;
      if (ProductType == ProductType.E) {
        result = Result.Wait_TimeOut;
        this.ipConfiguration = null;
        try {
          byte[] data = (byte[])Constants.Basic_Data.Clone();
          SendData(PacketType.GET_IP_CONFIGURATION, data);
        } catch (Exception ex) {
          GoToError(result, ex);
        }
        ipConfiguration = this.ipConfiguration;
      }
      return result;
    }

    public Result IP_Set_Address(string ipAddress) {
      result = Result.Interface_Not_Avaliable;
      if (ProductType == ProductType.E) {
        result = SET_IP(PacketType.SET_IP_ADDRESS, ipAddress);
      }
      return result;
    }

    public Result IP_Set_Configuration(IPConfiguration ipConfiguration) {
      result = Result.Interface_Not_Avaliable;
      if (ProductType == ProductType.E) {
        result = Result.Wait_TimeOut;
        try {
          result = IP_Set_Address(ipConfiguration.IP);
          if (result == Result.OK) {
            result = IP_Set_Port(ipConfiguration.Port);
          }
          if (result == Result.OK) {
            result = IP_Set_SubNet_Mask(ipConfiguration.SubNet_Mask);
          }
          if (result == Result.OK) {
            result = IP_Set_GateWay(ipConfiguration.Gateway);
          }
          if (result == Result.OK) {
            result = IP_Set_DHCP(ipConfiguration.Dhcp);
          }
        } catch (Exception ex) {
          GoToError(result, ex);
        }
      }
      return result;
    }

    public Result IP_Set_DHCP(bool enable) {
      result = Result.Interface_Not_Avaliable;
      if (ProductType == ProductType.E) {
        result = Result.Wait_TimeOut;
        try {
          byte[] array = (byte[])Constants.Basic_Data.Clone();
          array[0] = (byte)(enable ? 1 : 0);
          SendData(PacketType.SET_IP_DHCP, array);
        } catch (Exception ex) {
          GoToError(result, ex);
        }
      }
      return result;
    }

    public Result IP_Set_GateWay(string gateWay) {
      return SET_IP(PacketType.SET_IP_GATEWAY, gateWay);
    }

    public Result IP_Set_Port(int port) {
      result = Result.Interface_Not_Avaliable;
      if (ProductType == ProductType.E) {
        result = Result.Wait_TimeOut;
        try {
          byte[] array = (byte[])Constants.Basic_Data.Clone();
          Util.ConvertIntToByte(array, port);
          SendData(PacketType.SET_IP_PORT, array);
        } catch (Exception ex) {
          GoToError(result, ex);
        }
      }
      return result;
    }

    public Result IP_Set_SubNet_Mask(string subnetMask) {
      return SET_IP(PacketType.SET_IP_SUBNET_MASK, subnetMask);
    }

    internal void NoticeKeepAliveEnd() {
      keepAliveResetEvent.Set();
    }

    private Result Open() {
      result = Result.Wait_TimeOut;
      try {
        byte[] data = (byte[])Constants.Basic_Data.Clone();
        SendData(PacketType.OPEN_UHF_MODEL, data, 500);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    private Result Permlock(LockMemory lockMemory, PacketType pt) {
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        if (lockMemory == LockMemory.EPC) {
          array[0] = 1;
        } else if (lockMemory == LockMemory.EPC) {
          array[1] = 1;
        } else {
          array[0] = 1;
          array[1] = 1;
        }
        SendData(pt, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    private Result PwdProcess(string accessPwd, string pwd, PacketType pt, bool setAccess) {
      result = Result.Wait_TimeOut;
      if (pwd == null || "".Equals(pwd)) {
        result = Result.Pwd_Is_Null;
      } else if (pwd.Length != 8) {
        result = Result.Pwd_Length_Is_Error;
      } else {
        if (setAccess) {
          SetAccessPassword(accessPwd);
        }
        byte[] data = Util.ToHexByte(pwd);
        try {
          SendData(pt, data);
        } catch (Exception ex) {
          GoToError(result, ex);
        }
      }
      return result;
    }

    public Result RFID_Block_Write(AccessParam ap, string writeData) {
      if (pi != null && pi.CHIP_TYPE == ChipType.M100) {
        return Result.Interface_Not_Avaliable;
      }
      return Write(ap, writeData, PacketType.BLOCK_WRITE);
    }

    public Result RFID_Check_Antenna(int antennaIndex) {
      if (pi != null && pi.CHIP_TYPE == ChipType.M100) {
        return Result.Interface_Not_Avaliable;
      }
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        if (antennaIndex < 0 || antennaIndex > 3) {
          return Result.Unknown_Exception;
        }
        AntennaCheckResult.Clear();
        array[antennaIndex] = 1;
        SendData(PacketType.CHECK_ANTENNA_CONNECTED, array);
        if (AntennaCheckResult[antennaIndex] == 1) {
          return Result.OK;
        }
        return Result.Antenna_Not_Connected;
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Clear_Criteria() {
      result = Result.Wait_TimeOut;
      try {
        byte[] data = (byte[])Constants.Basic_Data.Clone();
        SendData(PacketType.CLEAR_FILTER_CRITERIA, data);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Close() {
      result = Result.OK;
      try {
        Close();
        client.Disconnect();
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Fast_Read(AccessParam ap, out Tag tag) {
      result = Result.Wait_TimeOut;
      tagData = null;
      try {
        tag = null;
        if (ap.Bank == MemoryBank.RESERVED) {
          SetAccessPassword(ap.AccessPassword);
        }
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = (byte)ap.Bank;
        array[1] = (byte)ap.OffSet;
        if (ap.OffSet > 61440 || ap.OffSet % 2 != 0) {
          result = Result.Param_Error;
          return result;
        }
        if (ap.Count > 128 || ap.Count % 2 != 0) {
          result = Result.Param_Error;
          return result;
        }
        if ((pi != null && pi.CHIP_TYPE == ChipType.M100 && ap.Count > 64) || ap.Count % 2 != 0) {
          result = Result.Param_Error;
          return result;
        }
        array[2] = (byte)ap.Count;
        array[3] = (byte)(ap.OffSet >> 8);
        SendData(PacketType.FAST_READ, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      tag = tagData;
      if (tagData == null) {
        result = Result.Read_Data_Is_Empty;
      }
      return result;
    }

    public Result RFID_Get_Config(out RfidSetting rs) {
      result = Result.Wait_TimeOut;
      rfidSetting = null;
      try {
        byte[] data = (byte[])Constants.Basic_Data.Clone();
        SendData(PacketType.GET_MODEL_CONFIGURATION, data);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      rs = rfidSetting;
      return result;
    }

    public Result RFID_Get_Firmware_Info(out FirmwareInfo fi) {
      result = Result.Wait_TimeOut;
      this.fi = null;
      try {
        byte[] data = (byte[])Constants.Basic_Data.Clone();
        if (ProductType == ProductType.E) {
          SendData(PacketType.GET_UHF_MODULE_INFO, data);
        } else {
          SendData(PacketType.GET_FIRMWARE_INFO, data);
        }
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      fi = this.fi;
      return result;
    }

    public Result RFID_Get_FrequencyList(out List<double> frequencyList) {
      frequencyList = new List<double>();
      if (pi != null && pi.CHIP_TYPE == ChipType.M100) {
        return Result.Interface_Not_Avaliable;
      }
      result = Result.Wait_TimeOut;
      string readValue = "";
      result = RFID_Read_Mac_Register("010A", out readValue);
      if (result == Result.OK) {
        double num = (double)Convert.ToInt32(readValue, 16) / 1000.0;
        if (num == 0.0) {
          for (int i = 0; i < 50; i++) {
            result = RFID_Write_Mac_Register("0C01", Convert.ToString(i, 16));
            Thread.Sleep(40);
            if (result != 0) {
              break;
            }
            result = RFID_Read_Mac_Register("0C02", out readValue);
            if (result != 0) {
              break;
            }
            if (Convert.ToInt16(readValue, 16) == 1) {
              frequencyList.Add(Channel.ChannnelToFrequency[i]);
            }
          }
        } else {
          frequencyList.Add(num);
        }
      }
      return result;
    }

    public Result RFID_Get_Optimal_Channel(out List<int> channelList) {
      channelList = null;
      if (pi != null && pi.CHIP_TYPE == ChipType.M100) {
        return Result.Interface_Not_Avaliable;
      }
      result = Result.Wait_TimeOut;
      optimal_channel = null;
      try {
        byte[] data = (byte[])Constants.Basic_Data.Clone();
        SendData(PacketType.GET_OPTIMAL_CHANNEL, data);
        channelList = optimal_channel;
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Get_Product_Info(out ProductInfo productInfo) {
      result = Result.Wait_TimeOut;
      pi = null;
      try {
        byte[] data = (byte[])Constants.Basic_Data.Clone();
        SendData(PacketType.GET_MODEL_PRODUCT_INFO, data);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      productInfo = pi;
      return result;
    }

    public ReversePowerDetectStatus RFID_Get_Reverse_Power_Detect_Status() {
      if (pi == null || pi.CHIP_TYPE != ChipType.M100) {
        result = Result.Wait_TimeOut;
        readData = "";
        try {
          byte[] array = (byte[])Constants.Basic_Data.Clone();
          array[0] = 0;
          array[1] = 159;
          SendData(PacketType.READ_OEM_REGISTER, array);
          if (result == Result.OK) {
            byte[] array2 = Util.ToHexByte(readData);
            if (array2[3] == 0) {
              return ReversePowerDetectStatus.ON;
            }
            if (array2[3] == 16) {
              return ReversePowerDetectStatus.OFF;
            }
          }
        } catch (Exception ex) {
          GoToError(result, ex);
        }
      }
      return ReversePowerDetectStatus.UnKnown;
    }

    public Result RFID_Get_Temperature(out int temperature) {
      result = Result.Wait_TimeOut;
      readData = "";
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = 11;
        array[1] = 6;
        SendData(PacketType.READ_MAC_REGISTER, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      byte[] array2 = Util.ToHexByte(readData);
      temperature = 0;
      if (array2.Length >= 4) {
        temperature = (array2[0] << 24) | (array2[1] << 16) | (array2[2] << 8) | array2[3];
      }
      return result;
    }

    public Result RFID_GroupRead(AccessParam ap) {
      result = Result.Wait_TimeOut;
      RFID_GroupRead(ap, async: false);
      return result;
    }

    public void RFID_GroupRead(AccessParam ap, bool async) {
      try {
        byte[] array = new byte[4];
        if (ap.OffSet > 61440 || ap.OffSet % 2 != 0) {
          result = Result.Param_Error;
          return;
        }
        if (ap.Count > 128 || ap.Count % 2 != 0) {
          result = Result.Param_Error;
        }
        array[0] = (byte)ap.Bank;
        array[1] = (byte)ap.OffSet;
        array[2] = (byte)ap.Count;
        array[3] = (byte)(ap.OffSet >> 8);
        byte[] array2 = sendPacket.Assemble(PacketType.GROUP_READ_DATA, array);
        if (!async) {
          inventoryResetEvent.Reset();
          RUNNING = true;
        }
        TraceData(array2);
        client.Send(array2);
        if (!async) {
          inventoryResetEvent.WaitOne();
          RUNNING = false;
        }
      } catch (Exception ex) {
        GoToError(result, ex);
      }
    }

    public Result RFID_Kill(string accessPwd, string killPwd) {
      return PwdProcess(accessPwd, killPwd, PacketType.KILL, setAccess: true);
    }

    public Result RFID_Lock(string lockPwd) {
      return PwdProcess(null, lockPwd, PacketType.LOCK, setAccess: false);
    }

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

    public Result RFID_PermLock(LockMemory lockMemory) {
      return Permlock(lockMemory, PacketType.PERM_LOCK);
    }

    public Result RFID_PermUnLock(LockMemory lockMemory) {
      return Permlock(lockMemory, PacketType.PERM_UNLOCK);
    }

    public Result RFID_Read(AccessParam ap, out string readData) {
      result = Result.Wait_TimeOut;
      this.readData = "";
      try {
        readData = "";
        if (ap.Bank == MemoryBank.RESERVED) {
          SetAccessPassword(ap.AccessPassword);
        }
        if (ap.OffSet > 61440 || ap.OffSet % 2 != 0) {
          result = Result.Param_Error;
          return result;
        }
        if (ap.Count > 128 || ap.Count % 2 != 0) {
          result = Result.Param_Error;
          return result;
        }
        if ((pi != null && pi.CHIP_TYPE == ChipType.M100 && ap.Count > 64) || ap.Count % 2 != 0) {
          result = Result.Param_Error;
          return result;
        }
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = (byte)ap.Bank;
        array[1] = (byte)ap.OffSet;
        array[2] = (byte)ap.Count;
        array[3] = (byte)(ap.OffSet >> 8);
        SendData(PacketType.READ_DATA, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      readData = this.readData;
      if ("".Equals(readData)) {
        result = Result.Read_Data_Is_Empty;
      }
      return result;
    }

    internal Result RFID_Read_Mac_Register(string address, out string readValue) {
      result = Result.Wait_TimeOut;
      readData = "";
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        if (address.Length < 4) {
          address = address.PadLeft(4, '0');
        } else if (address.Length > 4) {
          address = address.Substring(0, 4);
        }
        byte[] array2 = Util.ToHexByte(address);
        array[0] = array2[0];
        array[1] = array2[1];
        SendData(PacketType.READ_MAC_REGISTER, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      readValue = readData;
      return result;
    }

    internal Result RFID_Read_OEM(string address, out string readValue) {
      result = Result.Wait_TimeOut;
      readData = "";
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        if (address.Length < 4) {
          address = address.PadLeft(4, '0');
        } else if (address.Length > 4) {
          address = address.Substring(0, 4);
        }
        byte[] array2 = Util.ToHexByte(address);
        array[0] = array2[0];
        array[1] = array2[1];
        SendData(PacketType.READ_OEM_REGISTER, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      readValue = readData;
      return result;
    }

    public Result RFID_Reset() {
      result = Result.Wait_TimeOut;
      try {
        byte[] data = (byte[])Constants.Basic_Data.Clone();
        SendData(PacketType.RESET_UHF_MODEL, data);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_Antenna(List<AntennaPort> AntennaPortList) {
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        bool flag = true;
        foreach (AntennaPort AntennaPort in AntennaPortList) {
          if (antennaPortExists[AntennaPort.AntennaIndex]) {
            array[AntennaPort.AntennaIndex] = (byte)AntennaPort.Power;
          } else {
            array[AntennaPort.AntennaIndex] = 0;
          }
        }
        if (flag) {
          SendData(PacketType.SET_POWER, array);
        }
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_Config(RfidSetting rs) {
      result = Result.Wait_TimeOut;
      try {
        if (rs.AntennaPort_List != null) {
          result = RFID_Set_Antenna(rs.AntennaPort_List);
        }
        if (result == Result.OK) {
          result = RFID_Set_RegionList(rs.Region_List);
        }
        if (result == Result.OK && rs.RSSI_Filter != null) {
          result = RFID_Set_RSSIFilter(rs.RSSI_Filter);
        }
        if (result == Result.OK && rs.GPIO_Config != null) {
          result = RFID_Set_GPIO(rs.GPIO_Config);
        }
        if (result == Result.OK) {
          result = RFID_Set_Inventory_Mode(rs.Inventory_Mode);
        }
        if (result == Result.OK) {
          result = RFID_Set_Inventory_Time(rs.Inventory_Time);
        }
        if (result == Result.OK) {
          result = RFID_Set_SpeedMode(rs.Speed_Mode);
        }
        if (result == Result.OK && rs.Tag_Group != null) {
          result = RFID_Set_Tag_Group(rs.Tag_Group);
        }
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_Criteria(RfidCriteria criteria) {
      result = Result.Wait_TimeOut;
      try {
        if (criteria.OffSet > 250 || criteria.OffSet % 2 != 0) {
          result = Result.Param_Error;
          return result;
        }
        if (criteria.Count > 31) {
          criteria.Count = 31;
        }
        if (criteria.Count > criteria.Mask.Length) {
          criteria.Count = criteria.Mask.Length;
        } else if (criteria.Count < criteria.Mask.Length) {
          byte[] array = new byte[criteria.Count];
          Array.Copy(criteria.Mask, 0, array, 0, criteria.Count);
          criteria.Mask = array;
        }
        byte[] array2 = new byte[4 + criteria.Mask.Length];
        array2[0] = (byte)criteria.Bank;
        array2[1] = (byte)criteria.OffSet;
        array2[2] = (byte)criteria.Count;
        array2[3] = (byte)((!criteria.Match) ? 1 : 0);
        Array.Copy(criteria.Mask, 0, array2, 4, criteria.Mask.Length);
        SendData(PacketType.SET_FILTER_CRITERIA, array2);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_DWellTime(int dwellTime) {
      if (pi != null && pi.CHIP_TYPE == ChipType.M100) {
        return Result.Interface_Not_Avaliable;
      }
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = (byte)(dwellTime >> 24);
        array[1] = (byte)(dwellTime >> 16);
        array[2] = (byte)(dwellTime >> 8);
        array[3] = (byte)dwellTime;
        SendData(PacketType.SET_DWELL_TIME, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_Fix_Frequency(double frequency) {
      frequency = double.Parse(frequency.ToString("0.00"));
      result = Result.Wait_TimeOut;
      try {
        if (ChipType == ChipType.M100) {
          result = Result.Channel_Not_Supported;
        } else {
          if (RFS_Type != RFSType.FCC) {
            return Result.OK;
          }
          if (!Channel.FrequencyToChannel.ContainsKey(frequency)) {
            result = Result.Channel_Not_Supported;
          } else {
            byte[] array = (byte[])Constants.Basic_Data.Clone();
            array[0] = Channel.FrequencyToChannel[frequency];
            SendData(PacketType.SET_FIX_FREQUENCY, array);
          }
        }
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_Fix_Frequency(int port, int power, double frequency) {
      frequency = double.Parse(frequency.ToString("0.00"));
      result = Result.Wait_TimeOut;
      try {
        if (RFS_Type != RFSType.FCC) {
          return Result.OK;
        }
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[1] = (byte)port;
        array[2] = (byte)power;
        frequency *= 1000.0;
        if (frequency < 840250.0) {
          frequency = 840250.0;
        } else if (frequency > 968000.0) {
          frequency = 968000.0;
        }
        int num = (int)((frequency - 840250.0) / 500.0);
        array[3] = (byte)num;
        SendData(PacketType.SET_FIX_FREQUENCY, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_GPI(GPIOConfig gpioConfig) {
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = (byte)gpioConfig.GPI0_VALUE;
        array[1] = (byte)gpioConfig.GPI1_VALUE;
        SendData(PacketType.SET_GPI, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_GPIO(GPIOConfig gpioConfig) {
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = (byte)gpioConfig.GPO0_VALUE;
        array[1] = (byte)gpioConfig.GPO1_VALUE;
        array[2] = (byte)gpioConfig.GPI0_VALUE;
        array[3] = (byte)gpioConfig.GPI1_VALUE;
        SendData(PacketType.SET_GPIO, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_GPO(GPIOConfig gpioConfig) {
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = (byte)gpioConfig.GPO0_VALUE;
        array[1] = (byte)gpioConfig.GPO1_VALUE;
        SendNoCheckRunningData(PacketType.SET_GPO, array, 2000);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    private void RFID_Set_HeartBeat() {
      byte[] data = new byte[4] { 1, 0, 0, 0 };
      SendData(PacketType.SET_HEART_BEAT, data);
    }

    public Result RFID_Set_Inventory_Mode(InventoryMode mode) {
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = (byte)mode;
        SendData(PacketType.SET_INVENTORY_MODE, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_Inventory_Time(int invTime) {
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        Util.ConvertIntToByte(array, invTime);
        SendData(PacketType.SET_INVENTORY_TIME, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_QT(QT qt) {
      if (pi != null && pi.CHIP_TYPE == ChipType.M100) {
        return Result.Interface_Not_Avaliable;
      }
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = (byte)qt;
        SendData(PacketType.SET_QT, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_RegionList(RegionList region) {
      result = Result.Wait_TimeOut;
      try {
        if (RFS_Type != RFSType.FCC) {
          return Result.OK;
        }
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        if (region != RegionList.OPTIMAL) {
          array[0] = (byte)region;
          SendData(PacketType.SET_REGION, array);
        } else {
          SendData(PacketType.SET_OPTIMAL_CHANNEL, array);
        }
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_Reverse_Power_Detect_Status(ReversePowerDetectStatus rps) {
      if (pi != null && pi.CHIP_TYPE == ChipType.M100) {
        return Result.Interface_Not_Avaliable;
      }
      result = Result.Wait_TimeOut;
      try {
        switch (rps) {
          case ReversePowerDetectStatus.ON:
            if (ChipType == ChipType.R2000) {
              SendData(PacketType.WRITE_OEM_REGISTER, new byte[8] { 0, 159, 0, 0, 1, 16, 2, 0 });
            } else if (ChipType == ChipType.R500) {
              SendData(PacketType.WRITE_OEM_REGISTER, new byte[8] { 0, 159, 0, 0, 1, 16, 0, 0 });
            } else {
              result = Result.Interface_Not_Avaliable;
            }
            break;
          case ReversePowerDetectStatus.OFF:
            if (ChipType == ChipType.R2000) {
              SendData(PacketType.WRITE_OEM_REGISTER, new byte[8] { 0, 159, 0, 0, 1, 16, 2, 16 });
            } else if (ChipType == ChipType.R500) {
              SendData(PacketType.WRITE_OEM_REGISTER, new byte[8] { 0, 159, 0, 0, 1, 16, 0, 16 });
            } else {
              result = Result.Interface_Not_Avaliable;
            }
            break;
          default:
            result = Result.Interface_Not_Avaliable;
            break;
        }
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_RSSIFilter(RSSIFilter rssi) {
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = (byte)(rssi.Enable ? 1 : 0);
        byte[] array2 = new byte[2];
        Util.ConvertIntToByte(array2, (int)(rssi.RSSIValue * 10f));
        array[1] = array2[0];
        array[2] = array2[1];
        SendData(PacketType.SET_RECEIVE_SENSETIVITY, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_SpeedMode(SpeedMode speedMode) {
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = (byte)speedMode;
        SendData(PacketType.SET_SPEED_MODE, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_Tag_Group(TagGroup tagGroup) {
      result = Result.Wait_TimeOut;
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = (byte)tagGroup.Session;
        array[1] = (byte)tagGroup.SessionTarget;
        array[2] = (byte)tagGroup.SearchMode;
        SendData(PacketType.SET_TAG_GROUP, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Set_Work_Mode(WorkMode workMode) {
      result = Result.Wait_TimeOut;
      try {
        byte[] data = (byte[])Constants.Basic_Data.Clone();
        SendData(PacketType.SET_WORKING_MODE, data);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Start_Inventory() {
      result = Result.OK;
      RFID_Start_Inventory(TagMode.More, async: false);
      return result;
    }

    public Result RFID_Start_Inventory(TagMode tagMode) {
      result = Result.OK;
      RFID_Start_Inventory(tagMode, async: false);
      return result;
    }

    public void RFID_Start_Inventory(bool async) {
      RFID_Start_Inventory(TagMode.More, async);
    }

    public void RFID_Start_Inventory(TagMode tagMode, bool async) {
      try {
        byte[] array = (byte[])Constants.Basic_Data.Clone();
        array[0] = (byte)tagMode;
        if (!async) {
          inventoryResetEvent.Reset();
          RUNNING = true;
        }
        byte[] array2 = sendPacket.Assemble(PacketType.START_INVENTORY, array);
        TraceData(array2);
        client.Send(array2);
        if (!async) {
          inventoryResetEvent.WaitOne();
          RUNNING = false;
        }
      } catch (Exception ex) {
        GoToError(result, ex);
      }
    }

    public Result RFID_Stop_Inventory() {
      result = Result.Wait_TimeOut;
      try {
        byte[] data = (byte[])Constants.Basic_Data.Clone();
        SendInnerData(PacketType.STOP_INVENTORY, data, 2000);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_Test_Channel(ChannelPerformance cpf, out List<int> channelList) {
      channelList = null;
      if (pi != null && pi.CHIP_TYPE == ChipType.M100) {
        return Result.Interface_Not_Avaliable;
      }
      result = Result.Wait_TimeOut;
      optimal_channel = null;
      try {
        SendData(PacketType.TEST_CHANNEL, new byte[4]
        {
        (byte)cpf.Antenna_Port,
        (byte)cpf.Antenna_Power,
        (byte)cpf.Reverse_Power_Value,
        0
        }, 60000);
        channelList = optimal_channel;
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result RFID_UnLock(string lockPwd) {
      return PwdProcess(null, lockPwd, PacketType.UNLOCK, setAccess: false);
    }

    public Result RFID_Write(AccessParam ap, string writeData) {
      return Write(ap, writeData, PacketType.WRITE_DATA);
    }

    internal Result RFID_Write_Mac_Register(string address, string writeValue) {
      result = Result.Wait_TimeOut;
      readData = "";
      try {
        byte[] array = new byte[8];
        if (address.Length < 4) {
          address = address.PadLeft(4, '0');
        } else if (address.Length > 4) {
          address = address.Substring(0, 4);
        }
        if (writeValue.Length < 8) {
          writeValue = writeValue.PadLeft(8, '0');
        } else if (writeValue.Length > 8) {
          writeValue = writeValue.Substring(0, 8);
        }
        byte[] array2 = Util.ToHexByte(address);
        array[0] = array2[0];
        array[1] = array2[1];
        Array.Copy(Util.ToHexByte(writeValue), 0, array, 4, 4);
        SendData(PacketType.WRITE_MAC_REGISTER, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    internal Result RFID_Write_OEM(string address, string writeValue) {
      result = Result.Wait_TimeOut;
      readData = "";
      try {
        byte[] array = new byte[8];
        if (address.Length < 4) {
          address = address.PadLeft(4, '0');
        } else if (address.Length > 4) {
          address = address.Substring(0, 4);
        }
        if (writeValue.Length < 8) {
          writeValue = writeValue.PadLeft(8, '0');
        } else if (writeValue.Length > 8) {
          writeValue = writeValue.Substring(0, 8);
        }
        Array.Copy(Util.ToHexByte(address), 0, array, 0, 2);
        Array.Copy(Util.ToHexByte(writeValue), 0, array, 4, 4);
        SendData(PacketType.WRITE_OEM_REGISTER, array);
      } catch (Exception ex) {
        GoToError(result, ex);
      }
      return result;
    }

    public Result Save_Config() {
      result = Result.Interface_Not_Avaliable;
      if (ProductType == ProductType.E || ProductType == ProductType.R_U) {
        result = Result.Wait_TimeOut;
        try {
          byte[] data = (byte[])Constants.Basic_Data.Clone();
          SendData(PacketType.SAVE_UHF_IP_CONFIG, data);
        } catch (Exception ex) {
          GoToError(result, ex);
        }
      }
      return result;
    }

    public void Send_Command(byte[] inputCommand) {
      client.Send(inputCommand);
    }

    private void SendData(PacketType pt, byte[] data) {
      if (!RUNNING) {
        SendInnerData(pt, data, 2000);
      } else {
        result = Result.Module_Is_Busy;
      }
    }

    private void SendData(PacketType pt, byte[] data, int timeout) {
      if (!RUNNING) {
        SendInnerData(pt, data, timeout);
      } else {
        result = Result.Module_Is_Busy;
      }
    }

    private void SendInnerData(PacketType pt, byte[] data, int timeout) {
      if (runningMode == RunningMode.API) {
        RUNNING = true;
        byte[] array = sendPacket.Assemble(pt, data);
        TraceData(array);
        myResetEvent.Reset();
        client.Send(array);
        myResetEvent.WaitOne(timeout, exitContext: true);
        RUNNING = false;
      }
    }

    private void SendKeepAlive() {
      byte[] array = sendPacket.Assemble(PacketType.HEART_PACK, (byte[])Constants.Basic_Data.Clone());
      while (client.IsAlive()) {
        if (SDKLog.LogEnable("IsDebugEnabled")) {
          SDKLog.Debug("{0}{1}", LogHeader, Util.ToHexString(array));
        }
        keepAliveResetEvent.Reset();
        client.Send(array);
        keepAliveResetEvent.WaitOne(2000, exitContext: true);
      }
    }

    private void SendNoCheckRunningData(PacketType pt, byte[] data, int timeout) {
      if (runningMode == RunningMode.API) {
        byte[] array = sendPacket.Assemble(pt, data);
        TraceData(array);
        myResetEvent.Reset();
        client.Send(array);
        myResetEvent.WaitOne(timeout, exitContext: true);
      }
    }

    private Result SET_IP(PacketType pt, string ipAddress) {
      result = Result.Interface_Not_Avaliable;
      if (ProductType == ProductType.E) {
        result = Result.Wait_TimeOut;
        try {
          string[] array = ipAddress.Split('.');
          byte[] array2 = new byte[4];
          for (int i = 0; i < array.Length; i++) {
            array2[i] = byte.Parse(array[i]);
          }
          SendData(pt, array2);
        } catch (Exception ex) {
          GoToError(result, ex);
        }
      }
      return result;
    }

    public void Set_Running_Mode(RunningMode _runMode) {
      runningMode = _runMode;
    }

    private void SetAccessPassword(string accessPassword) {
      byte[] array = new byte[5];
      byte[] data = Util.ToHexByte(accessPassword);
      SendData(PacketType.SET_ACCESS_PASSWORD, data, 0);
    }

    internal void SetFastReadData(Tag _tag) {
      tagData = _tag;
      WakeUp();
    }

    internal void SetFirmwareInfo(FirmwareInfo _fi) {
      fi = _fi;
      WakeUp();
    }

    internal void SetIPConfiguration(IPConfiguration _ip) {
      ipConfiguration = _ip;
      WakeUp();
    }

    internal void SetModelConfiguration(RfidSetting _ms) {
      rfidSetting = _ms;
      WakeUp();
    }

    internal void SetOptimalChannel(List<int> optimalChannel) {
      optimal_channel = optimalChannel;
      WakeUp();
    }

    internal void SetProductInfo(ProductInfo _pi) {
      pi = _pi;
      ProductType = pi.PRODUCT_TYPE;
      ChipType = pi.CHIP_TYPE;
      foreach (AntennaPort item in pi.ANTENNA_PORT_EXISTS_LIST) {
        if (antennaPortExists.ContainsKey(item.AntennaIndex)) {
          antennaPortExists[item.AntennaIndex] = item.Exist;
        } else {
          antennaPortExists.Add(item.AntennaIndex, item.Exist);
        }
      }
      WakeUp();
    }

    internal void SetReadData(string _readData) {
      readData = _readData;
      WakeUp();
    }

    public Result Storage_Get_Block_Count(out ushort count) {
      result = Result.Interface_Not_Avaliable;
      count = 0;
      if (ProductType == ProductType.E) {
        result = Result.Wait_TimeOut;
        readData = "0";
        try {
          byte[] data = (byte[])Constants.Basic_Data.Clone();
          SendData(PacketType.GET_STORAGE_BLOCK_COUNT, data);
        } catch (Exception ex) {
          GoToError(result, ex);
        }
        count = Convert.ToUInt16(readData, 16);
      }
      return result;
    }

    public Result Storage_Get_Block_Size(out byte size) {
      result = Result.Interface_Not_Avaliable;
      size = 0;
      if (ProductType == ProductType.E) {
        result = Result.Wait_TimeOut;
        readData = "0";
        try {
          byte[] data = (byte[])Constants.Basic_Data.Clone();
          SendData(PacketType.GET_STORAGE_BLOCK_SIZE, data);
        } catch (Exception ex) {
          GoToError(result, ex);
        }
        size = Convert.ToByte(readData, 16);
      }
      return result;
    }

    public Result Storage_Read_Block_Data(ushort blockIndex, out string readData) {
      result = Result.Interface_Not_Avaliable;
      readData = string.Empty;
      if (ProductType == ProductType.E) {
        result = Result.Wait_TimeOut;
        this.readData = "";
        try {
          byte[] array = (byte[])Constants.Basic_Data.Clone();
          byte[] array2 = new byte[2];
          Util.ConvertIntToByte(array2, blockIndex);
          Array.Copy(array2, 0, array, 0, 2);
          SendData(PacketType.READ_STORAGE_BLOCK_DATA, array);
        } catch (Exception ex) {
          GoToError(result, ex);
        }
        readData = this.readData;
      }
      return result;
    }

    public Result Storage_Set_Block_Size(byte size) {
      result = Result.Interface_Not_Avaliable;
      if (ProductType == ProductType.E) {
        result = Result.Wait_TimeOut;
        try {
          byte[] array = (byte[])Constants.Basic_Data.Clone();
          array[0] = size;
          SendData(PacketType.SET_STORAGE_BLOCK_SIZE, array);
        } catch (Exception ex) {
          GoToError(result, ex);
        }
      }
      return result;
    }

    public Result Storage_Write_Block_Data(ushort blockIndex, string writeData) {
      result = Result.Interface_Not_Avaliable;
      if (ProductType == ProductType.E) {
        result = Result.Wait_TimeOut;
        if (writeData == null || "".Equals(writeData)) {
          result = Result.Write_Data_Is_Null;
        } else {
          byte[] array = Util.ToHexByte(writeData);
          try {
            byte[] array2 = new byte[4 + array.Length];
            Array.Copy(array, 0, array2, 4, array.Length);
            byte[] array3 = new byte[2];
            Util.ConvertIntToByte(array3, blockIndex);
            Array.Copy(array3, 0, array2, 0, 2);
            SendData(PacketType.WRITE_STORAGE_BLOCK_DATA, array2);
          } catch (Exception ex) {
            GoToError(result, ex);
          }
        }
      }
      return result;
    }

    private void TraceData(byte[] data) {
      if (SDKLog.LogEnable("IsDebugEnabled")) {
        SDKLog.Debug("{0}{1}", LogHeader, Util.ToHexString(data));
      }
      if (sendPacket is LinuxESend) {
        if (CustomTraceListener.HasConsole) {
          Util.PrintCustomTrace(Util.ToHexString(data) + "(" + ((PacketType)data[4]).ToString() + ")");
        }
      } else if (CustomTraceListener.HasConsole) {
        Util.PrintCustomTrace(Util.ToHexString(data) + "(" + ((PacketType)data[0]).ToString() + ")");
      }
    }

    internal void WakeUp() {
      myResetEvent.Set();
    }

    internal void WakeUpInventory() {
      FireEvent(null);
      inventoryResetEvent.Set();
    }

    private Result Write(AccessParam ap, string writeData, PacketType pt) {
      result = Result.Wait_TimeOut;
      if (writeData == null || "".Equals(writeData)) {
        result = Result.Write_Data_Is_Null;
      } else {
        byte[] array = Util.ToHexByte(writeData);
        if (pt == PacketType.BLOCK_WRITE && array.Length > 128) {
          return Result.Write_Data_Too_Long;
        }
        if (pt == PacketType.WRITE_DATA && pi != null && pi.CHIP_TYPE == ChipType.M100 && array.Length > 128) {
          return Result.Write_Data_Too_Long;
        }
        if (pt == PacketType.WRITE_DATA && array.Length > 128) {
          return Result.Write_Data_Too_Long;
        }
        if (ap.OffSet > 61440 || ap.OffSet % 2 != 0) {
          result = Result.Param_Error;
          return result;
        }
        if (ap.Count % 2 != 0) {
          result = Result.Param_Error;
          return result;
        }
        if (ap.Count > array.Length) {
          ap.Count = array.Length;
        } else if (ap.Count < array.Length) {
          byte[] array2 = new byte[ap.Count];
          Array.Copy(array, 0, array2, 0, ap.Count);
          array = array2;
        }
        try {
          SetAccessPassword(ap.AccessPassword);
          byte[] array3 = new byte[4 + array.Length];
          array3[0] = (byte)ap.Bank;
          array3[1] = (byte)ap.OffSet;
          array3[2] = (byte)ap.Count;
          array3[3] = (byte)(ap.OffSet >> 8);
          Array.Copy(array, 0, array3, 4, array.Length);
          SendData(pt, array3, 10000);
        } catch (Exception ex) {
          GoToError(result, ex);
        }
      }
      return result;
    }
  }

}
