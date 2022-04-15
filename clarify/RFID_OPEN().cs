public Result RFID_Open() {
	// wait tiem out result = 15
	Result result = Result.Wait_TimeOut;
/////////////////////////////////////////////////////////////////////////////
// if this is demo so here will not run
/////////////////////////////////////////////////////////////////////////////
/**/try {
/**/  client.Connect();
/**/} catch (UHFException ex) {
/**/  if (client is NetClient) {
/**/    result = Result.Network_Exception;
/**/    SDKLog.Error("{0}Network Exception:", ex, LogHeader);
/**/    return result;
/**/  }
/**/  result = Result.Serial_Exception;
/**/  SDKLog.Error("{0}Serial Exception:", ex, LogHeader);
/**/  return result;
/**/}
/////////////////////////////////////////////////////////////////////////////
// if this is demo so here will not run
/////////////////////////////////////////////////////////////////////////////

// check the parse packet version
	try {
		// client set parse packet to enum v3(1) [v2(0)
		client.SetParsePacket(ReaderVersion.V3);
		// send packet is linux server side
		sendPacket = new LinuxESend();
		// linux get data?
		for (int i = 0; i < 5; i++) {
			// check error
			result = Open();
			if (result == Result.OK) {// target
				break;
			}
		}
		// result is Wait_TimeOut(15)
		if (result != 0) {
			// change clinet set parse packet to v2(0)
			client.SetParsePacket(ReaderVersion.V2);
			//instance JWSend()
			sendPacket = new JWSend();
			// check error no error reault is 15
			result = Open();
		}
///////////////////////////////////////////////////////////////////////////
// because result isn't Result.OK(0) so here will not run
///////////////////////////////////////////////////////////////////////////
/**/if (result == Result.OK) { // target
/**/	result = RFID_Get_Product_Info(out pi);
/**/}
/**/if (result == Result.OK) {
/**/	string readValue = "";
/**/	result = RFID_Read_OEM("9D", out readValue);
/**/	if (result == Result.OK) { // target
/**/		RFS_Type = ("00000000".Equals(readValue) ? RFSType.FCC : RFSType.ETSI);
/**/	}
/**/}
/**/if (result == Result.OK) { // target
/**/	FirmwareInfo firmwareInfo = new FirmwareInfo();
/**/	result = RFID_Get_Firmware_Info(out firmwareInfo);
/**/	int num = firmwareInfo.Main_Version * 100 + firmwareInfo.Sub_Version;
/**/	bool flag = false;
/**/	if (pi.CHIP_TYPE == ChipType.M100) {
/**/		if (result == Result.OK && num >= 130) { // target
/**/			flag = true;
/**/		}
/**/	} else if (result == Result.OK && num >= 125) {// target
/**/		flag = true;
/**/	}
/**/	if (flag) {
/**/		Thread thread = new Thread(SendKeepAlive);
/**/		thread.Priority = ThreadPriority.AboveNormal;
/**/		thread.IsBackground = true;
/**/		thread.Start();
///////////////////////////////////////////////////////////////////////////
// because result isn't Result.OK(0) so here will not run
///////////////////////////////////////////////////////////////////////////
} else if (client is NetClient) {
			RFID_Set_HeartBeat();
	}
}
///////////////////////////////////////////////////////////////////////////
// here is the program entry point
///////////////////////////////////////////////////////////////////////////
		if (result != 0) {
			RFID_Close();
		}
	} catch (Exception ex2) {
		RFID_Close();
		GoToError(result, ex2);
	}
	return result;
}