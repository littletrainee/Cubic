using System;
using System.IO.Ports;
using System.Threading;
using JW.UHF;

namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.SerialClient

  internal class SerialClient : IClient {
    private int baudRate;

    private SerialPort com;

    private bool dtrEnable;

    private object lockObj;

    private JWReader reader;

    private ByteBuffer receiveBuffer;

    private bool rtsEnable;

    private string serialPort;

    private long startTime;

    private IParsePacket uhfParsePacket;

    public SerialClient(string _serialPort, JWReader _sdk) {
      receiveBuffer = new ByteBuffer();
      uhfParsePacket = null;
      startTime = DateTime.Now.Ticks;
      dtrEnable = false;
      rtsEnable = false;
      lockObj = new object();
      serialPort = _serialPort;
      reader = _sdk;
      uhfParsePacket = new JWParsePacket(this, reader);
    }

    public SerialClient(string _serialPort, JWReader _sdk, int _baudRate) {
      receiveBuffer = new ByteBuffer();
      uhfParsePacket = null;
      startTime = DateTime.Now.Ticks;
      dtrEnable = false;
      rtsEnable = false;
      lockObj = new object();
      serialPort = _serialPort;
      reader = _sdk;
      baudRate = _baudRate;
      uhfParsePacket = new JWParsePacket(this, reader);
    }

    public SerialClient(string _serialPort, JWReader _sdk, int _baudRate, bool _dtrEnable, bool _rtsEnable) {
      receiveBuffer = new ByteBuffer();
      uhfParsePacket = null;
      startTime = DateTime.Now.Ticks;
      dtrEnable = false;
      rtsEnable = false;
      lockObj = new object();
      serialPort = _serialPort;
      reader = _sdk;
      baudRate = _baudRate;
      dtrEnable = _dtrEnable;
      rtsEnable = _rtsEnable;
      uhfParsePacket = new JWParsePacket(this, reader);
    }

    public void ClearReceiveBuffer() {
      if (IsAlive()) {
        com.DiscardInBuffer();
      }
    }

    public void Connect() {
      com = new SerialPort(serialPort, baudRate);
      com.DataBits = 8;
      com.StopBits = StopBits.One;
      com.Parity = Parity.None;
      com.ReadBufferSize = 204800;
      com.WriteTimeout = 1000;
      com.DtrEnable = dtrEnable;
      com.RtsEnable = rtsEnable;
      try {
        com.Open();
        com.DiscardInBuffer();
        com.DiscardOutBuffer();
        reader.IsConnected = true;
        Thread thread = new Thread(ReceiveThread);
        thread.Name = "receive thread";
        thread.Priority = ThreadPriority.Highest;
        thread.Start();
      } catch (Exception ex) {
        throw new UHFException("Open Serial Failure:", ex);
      }
    }

    public void Disconnect() {
      if (com == null) {
        return;
      }
      try {
        reader.WakeUp();
        reader.WakeUpInventory();
        com.Close();
      } catch (Exception ex) {
        SDKLog.Error("{0}Disconnect Serial Exception:", ex, reader.LogHeader);
        reader.result = ResultEnum.Serial_Exception;
      } finally {
        com = null;
        reader.NoticeKeepAliveEnd();
        reader.IsConnected = false;
      }
    }

    public bool IsAlive() {
      if (com == null || !reader.IsConnected) {
        reader.result = ResultEnum.Device_Has_Disconnected;
        return false;
      }
      try {
        if (!com.IsOpen) {
          return false;
        }
        if (com.BytesToRead >= 102400) {
          SDKLog.Warn("{0}Receive Buffer Near Full.", reader.LogHeader);
        }
      } catch (Exception ex) {
        SDKLog.Error("{0}Serial Exception:", ex, reader.LogHeader);
        reader.result = ResultEnum.Serial_Exception;
        reader.WakeUpInventory();
        reader.WakeUp();
        return false;
      }
      return true;
    }

    public byte[] Receive() {
      byte[] array = null;
      try {
        if (com.BytesToRead <= 0) {
          return array;
        }
        startTime = DateTime.Now.Ticks;
        int num = com.BytesToRead;
        if (num > 10240) {
          num = 10240;
        }
        array = new byte[num];
        com.Read(array, 0, num);
      } catch (Exception ex) {
        SDKLog.Error("{0}Receive Exception:", ex, reader.LogHeader);
        reader.result = ResultEnum.Serial_Exception;
      }
      return array;
    }

    private void ReceiveThread() {
      while (IsAlive()) {
        long ticks = DateTime.Now.Ticks - startTime;
        if (new TimeSpan(ticks).TotalSeconds > (double)Constants.Max_KeepAlive_Time) {
          reader.RFID_Stop_Inventory();
          reader.result = ResultEnum.Serial_Exception;
          reader.FireReaderErrorEvent();
          Disconnect();
          break;
        }
        try {
          if (com.BytesToRead > 0) {
            uhfParsePacket.ProcessData();
          } else {
            Thread.Sleep(50);
          }
        } catch (ArgumentOutOfRangeException ex) {
          SDKLog.Error("{0}Receive Buffer OverFlow:", ex, reader.LogHeader);
          reader.result = ResultEnum.Receive_Buffer_OverFlow;
          reader.FireReaderErrorEvent();
        } catch (Exception ex2) {
          SDKLog.Error("{0}Process Data Error:", ex2, reader.LogHeader);
        }
      }
      reader.WakeUpInventory();
      reader.WakeUp();
    }

    public void Send(byte[] sendData) {
      if (!IsAlive()) {
        return;
      }
      try {
        lock (lockObj) {
          com.Write(sendData, 0, sendData.Length);
          if (reader.sendPacket is JWSend) {
            if (sendData[0] == 175) {
              Thread.Sleep(100);
            }
          } else if (sendData[4] == 175) {
            Thread.Sleep(100);
          }
        }
      } catch (Exception ex) {
        SDKLog.Error("{0}Send Exception:", ex, reader.LogHeader);
        reader.result = ResultEnum.Serial_Exception;
      }
    }

    public void SetParsePacket(ReaderVersion rv) {
      if (rv == ReaderVersion.V2) {
        uhfParsePacket = new JWParsePacket(this, reader);
      } else {
        uhfParsePacket = new LinuxEParsePacket(this, reader);
      }
    }
  }

}
