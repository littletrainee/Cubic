using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebuildCubic.ConnectFolder {
  internal interface IClient {
    void ClearReceiveBuffer();
    void Connect();
    void Disconnect();
    bool IsAlive();
    byte[] Receive();
    void Send(byte[] sendData);
    void SetParsePacket(ReaderVersion rv);
  }
}
