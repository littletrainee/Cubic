using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebuildCubic.JWReaderFolder {
	// JW.UHF.RfidSetting
	using System;
	using System.Collections.Generic;
	using JW.UHF;

	public class RfidSetting {
		public List<AntennaPort> AntennaPort_List;

		public GPIOConfig GPIO_Config;

		public InventoryMode Inventory_Mode;

		public int Inventory_Time;

		public RegionList Region_List;

		public RSSIFilter RSSI_Filter;

		public SpeedMode Speed_Mode;

		public TagGroup Tag_Group;

		internal WorkMode Work_Mode;

		public RfidSetting() {
			Inventory_Mode = InventoryMode.Continue;
		}

		internal RfidSetting(byte[] packetData, JWReader reader) {
			Inventory_Mode = InventoryMode.Continue;
			AntennaPort_List = new List<AntennaPort>();
			for (int i = 0; i < 4; i++) {
				AntennaPort item = new AntennaPort {
					AntennaIndex = i,
					Power = packetData[i],
					Exist = reader.antennaPortExists[i]
				};
				AntennaPort_List.Add(item);
			}
			Region_List = (RegionList)packetData[4];
			Speed_Mode = (SpeedMode)packetData[5];
			byte[] array = new byte[4];
			Array.Copy(packetData, 6, array, 0, 4);
			Inventory_Time = Util.ConvertByteToInt(array, little: false);
			if (reader.ProductType == ProductType.E) {
				Inventory_Mode = (InventoryMode)packetData[10];
				GPIO_Config = new GPIOConfig();
				GPIO_Config.GPO0_VALUE = (GPOTriggerValue)packetData[11];
				GPIO_Config.GPO1_VALUE = (GPOTriggerValue)packetData[12];
				GPIO_Config.GPI0_VALUE = (GPITriggerValue)packetData[13];
				GPIO_Config.GPI1_VALUE = (GPITriggerValue)packetData[14];
				Work_Mode = (WorkMode)packetData[15];
				Tag_Group = new TagGroup();
				Tag_Group.Session = (Session)packetData[16];
				Tag_Group.SessionTarget = (SessionTarget)packetData[17];
				Tag_Group.SearchMode = (SearchMode)packetData[18];
				RSSI_Filter = new RSSIFilter();
				RSSI_Filter.Enable = packetData[19] == 1;
				string value = Util.ToHexString(new byte[2]
				{
				packetData[20],
				packetData[21]
				});
				RSSI_Filter.RSSIValue = (float)Convert.ToInt16(value, 16) / 10f;
			} else {
				GPIO_Config = new GPIOConfig();
				GPIO_Config.GPO0_VALUE = (GPOTriggerValue)packetData[10];
				GPIO_Config.GPO1_VALUE = (GPOTriggerValue)packetData[11];
				GPIO_Config.GPI0_VALUE = (GPITriggerValue)packetData[12];
				GPIO_Config.GPI1_VALUE = (GPITriggerValue)packetData[13];
				Tag_Group = new TagGroup();
				Tag_Group.Session = (Session)packetData[14];
				Tag_Group.SessionTarget = (SessionTarget)packetData[15];
				Tag_Group.SearchMode = (SearchMode)packetData[16];
				RSSI_Filter = new RSSIFilter();
				RSSI_Filter.Enable = packetData[17] == 1;
				string value = Util.ToHexString(new byte[2]
				{
				packetData[18],
				packetData[19]
				});
				RSSI_Filter.RSSIValue = (float)Convert.ToInt16(value, 16) / 10f;
			}
		}
	}

}
