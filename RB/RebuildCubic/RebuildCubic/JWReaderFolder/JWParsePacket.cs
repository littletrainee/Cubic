using System;
using System.Collections.Generic;
using JW.UHF;

namespace RebuildCubic.JWReaderFolder {
	// JW.UHF.JWParsePacket

	internal class JWParsePacket : IParsePacket {
		private IClient client;

		private Dictionary<string, byte> packetDic = EnumToList();

		private JWReader reader;

		private int uart_bufferlength = 0;

		private byte[] uart_receivebuffer = new byte[204800];

		public JWParsePacket(IClient _client, JWReader _reader) {
			client = _client;
			reader = _reader;
		}

		private static Dictionary<string, byte> EnumToList() {
			return EnumExtend.EnumToDictionary((PacketType)0);
		}

		private void ErrorProcess() {
			byte[] array = null;
			array = ((uart_bufferlength % 2 != 0) ? new byte[uart_bufferlength + 1] : new byte[uart_bufferlength]);
			Array.Copy(uart_receivebuffer, 0, array, 0, uart_bufferlength);
			if (SDKLog.LogEnable("IsErrorEnabled")) {
				SDKLog.Error("{0}Error Packet:{1},Length={2}", reader.LogHeader, Util.ToHexString(array), uart_bufferlength);
			}
			reader.RFID_Stop_Inventory();
			reader.result = ResultEnum.Parse_Packet_Data_Error;
			reader.FireReaderErrorEvent();
			reader.WakeUp();
			reader.WakeUpInventory();
			uart_bufferlength = 0;
			client.ClearReceiveBuffer();
		}

		private void loopProcessData() {
			if (packetDic.ContainsValue(uart_receivebuffer[0])) {
				int num = uart_receivebuffer[1] + 2;
				if (uart_bufferlength >= num) {
					byte[] array = new byte[num];
					Array.Copy(uart_receivebuffer, 0, array, 0, num);
					uart_bufferlength -= num;
					try {
						ParsePacket(array);
					} catch (Exception ex) {
						SDKLog.Error("{0}Process Packet Error:", ex, reader.LogHeader);
						ErrorProcess();
						return;
					}
					if (uart_bufferlength > 0) {
						Array.Copy(uart_receivebuffer, num, uart_receivebuffer, 0, uart_bufferlength);
					}
					if (uart_bufferlength >= 2) {
						loopProcessData();
					}
				}
			} else {
				SDKLog.Error("{0}Packet Header Error.", reader.LogHeader);
				ErrorProcess();
			}
		}

		private void ParsePacket(byte[] packetData) {
			if (SDKLog.LogEnable("IsDebugEnabled")) {
				SDKLog.Debug("{0}{1}", reader.LogHeader, Util.ToHexString(packetData));
			}
			if (CustomTraceListener.HasConsole) {
				Util.PrintCustomTrace(Util.ToHexString(packetData));
			}
			if (reader.runningMode == RunningMode.COMMAND) {
				reader.FireCommandResponseEvent(packetData);
			} else {
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
		}

		public void ProcessData() {
			byte[] array = client.Receive();
			if (array != null) {
				Array.Copy(array, 0, uart_receivebuffer, uart_bufferlength, array.Length);
				uart_bufferlength += array.Length;
				if (uart_bufferlength >= 2) {
					loopProcessData();
				}
			}
		}
	}

}
