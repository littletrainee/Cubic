using JW.UHF;

namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.GPIEvent

  public class GPIEvent {
    public GPIEventType EventType;

    public int Port = -1;

    public GPITriggerValue TriggerValue;

    internal GPIEvent(byte[] packetData) {
      if (packetData[2] == 1) {
        Port = 0;
        TriggerValue = GPITriggerValue.Inventory;
        if (packetData[4] == 0) {
          EventType = GPIEventType.Start_Inventory;
        } else {
          EventType = GPIEventType.Stop_Inventory;
        }
      } else if (packetData[2] == 2) {
        Port = 0;
        TriggerValue = GPITriggerValue.Input;
        if (packetData[4] == 0) {
          EventType = GPIEventType.Hign_Slow;
        } else {
          EventType = GPIEventType.Slow_Hign;
        }
      }
      if (packetData[3] == 1) {
        Port = 1;
        TriggerValue = GPITriggerValue.Inventory;
        if (packetData[5] == 0) {
          EventType = GPIEventType.Start_Inventory;
        } else {
          EventType = GPIEventType.Stop_Inventory;
        }
      } else if (packetData[3] == 2) {
        Port = 1;
        TriggerValue = GPITriggerValue.Input;
        if (packetData[5] == 0) {
          EventType = GPIEventType.Hign_Slow;
        } else {
          EventType = GPIEventType.Slow_Hign;
        }
      }
    }
  }

}
