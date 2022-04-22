using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using JW.UHF;
namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.NetClient

  internal class NetClient : IClient {
    private int connectWaitTime;

    private Encoding encoding;

    private string ip;

    private int keepAliveInterval;

    private int keepAliveTime;

    private object lockObj;

    private int port;

    private JWReader reader;

    private Socket socket;

    private long startTime;

    private IParsePacket uhfParsePacket;

    public NetClient(Encoding _encoding, string _ip, int _port, JWReader _reader) {
      socket = null;
      keepAliveTime = 1000;
      keepAliveInterval = 1000;
      connectWaitTime = 20000;
      startTime = DateTime.Now.Ticks;
      uhfParsePacket = null;
      lockObj = new object();
      encoding = _encoding;
      ip = _ip;
      port = _port;
      reader = _reader;
      uhfParsePacket = new JWParsePacket(this, reader);
    }

    public NetClient(Encoding _encoding, string _ip, int _port, int _keepAliveTime, int _keepAliveInterval, int _connectWaitTime, JWReader _reader) {
      socket = null;
      keepAliveTime = 1000;
      keepAliveInterval = 1000;
      connectWaitTime = 20000;
      startTime = DateTime.Now.Ticks;
      uhfParsePacket = null;
      lockObj = new object();
      encoding = _encoding;
      ip = _ip;
      port = _port;
      reader = _reader;
      keepAliveTime = _keepAliveTime;
      keepAliveInterval = _keepAliveInterval;
      connectWaitTime = _connectWaitTime;
      uhfParsePacket = new JWParsePacket(this, reader);
    }

    public void ClearReceiveBuffer() {
      if (IsAlive()) {
        byte[] buffer = new byte[socket.Available];
        socket.Receive(buffer);
      }
    }

    public void Connect() {
      Socket socket = null;
      startTime = DateTime.Now.Ticks;
      try {
        IPAddress address = IPAddress.Parse(ip);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, optionValue: true);
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1000);
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 204800);
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, optionValue: true);
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, optionValue: true);
        socket.IOControl(IOControlCode.KeepAliveValues, GetKeepAliveSetting(1, keepAliveTime, keepAliveInterval), null);
        if (!socket.BeginConnect(address, port, null, null).AsyncWaitHandle.WaitOne(connectWaitTime, exitContext: true)) {
          Disconnect();
          throw new UHFException("Connect Server Error.");
        }
        if (!socket.Connected) {
          Disconnect();
          throw new UHFException("Connect Server Error.");
        }
        this.socket = socket;
        reader.IsConnected = true;
        Thread thread = new Thread(ReceiveThread);
        thread.Name = "receive thread";
        thread.Priority = ThreadPriority.Highest;
        thread.Start();
      } catch (Exception ex) {
        throw new UHFException("Connect Server Error.", ex);
      }
    }

    public void Disconnect() {
      reader.WakeUp();
      reader.WakeUpInventory();
      try {
        if (socket != null) {
          socket.Shutdown(SocketShutdown.Both);
          socket.Disconnect(reuseSocket: true);
          socket.Close();
        }
      } catch (Exception ex) {
        SDKLog.Error("{0}Disconnect Network Exception:", ex, reader.LogHeader);
        reader.result = ResultEnum.Network_Exception;
      } finally {
        reader.IsConnected = false;
        reader.NoticeKeepAliveEnd();
        socket = null;
      }
    }

    private byte[] GetKeepAliveSetting(int onOff, int keepAliveTime, int keepAliveInterval) {
      byte[] array = new byte[12];
      BitConverter.GetBytes(onOff).CopyTo(array, 0);
      BitConverter.GetBytes(keepAliveTime).CopyTo(array, 4);
      BitConverter.GetBytes(keepAliveInterval).CopyTo(array, 8);
      return array;
    }

    public bool IsAlive() {
      if (socket == null || !reader.IsConnected) {
        reader.result = ResultEnum.Device_Has_Disconnected;
        return false;
      }
      try {
        if (!socket.Connected) {
          reader.result = ResultEnum.Device_Has_Disconnected;
          return false;
        }
        if (!socket.Poll(1000, SelectMode.SelectRead) || socket.Available == 0) {
        }
        if (socket.Available >= 102400) {
          SDKLog.Warn("{0}Receive Buffer Near Full.", reader.LogHeader);
        }
      } catch (Exception ex) {
        SDKLog.Error("{0}Network Exception:", ex, reader.LogHeader);
        reader.result = ResultEnum.Network_Exception;
        Disconnect();
        return false;
      }
      return true;
    }

    public byte[] Receive() {
      byte[] array = null;
      try {
        if (socket.Available <= 0) {
          return array;
        }
        startTime = DateTime.Now.Ticks;
        int num = socket.Available;
        if (num > 10240) {
          num = 10240;
        }
        array = new byte[num];
        socket.Receive(array);
      } catch (Exception ex) {
        SDKLog.Error("{0}Receive Exception:", ex, reader.LogHeader);
        reader.result = ResultEnum.Network_Exception;
      }
      return array;
    }

    private void ReceiveThread() {
      while (IsAlive()) {
        long ticks = DateTime.Now.Ticks - startTime;
        if (new TimeSpan(ticks).TotalSeconds > (double)Constants.Max_KeepAlive_Time) {
          reader.RFID_Stop_Inventory();
          reader.result = ResultEnum.Network_Exception;
          reader.FireReaderErrorEvent();
          Disconnect();
          break;
        }
        try {
          if (socket.Available > 0) {
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
      if (IsAlive()) {
        try {
          lock (lockObj) {
            socket.Send(sendData, sendData.Length, SocketFlags.None);
            if (reader.sendPacket is JWSend) {
              if (sendData[0] == 175) {
                Thread.Sleep(100);
              }
            } else if (sendData[4] == 175) {
              Thread.Sleep(100);
            }
            return;
          }
        } catch (Exception ex) {
          SDKLog.Error("{0}Send Exception:", ex, reader.LogHeader);
          reader.result = ResultEnum.Network_Exception;
          return;
        }
      }
      Disconnect();
      reader.result = ResultEnum.Network_Exception;
      reader.WakeUp();
      reader.WakeUpInventory();
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
