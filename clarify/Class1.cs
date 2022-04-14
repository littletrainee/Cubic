public JWReader(string _comPort) {
	// RunningMode enum  API= 0; COMMAND = 1
	runningMode = RunningMode.API;
	// RfidSetting class = null
	rfidSetting = null;
	// FireWareInfo class1 = null
	fi = null;
	// string
	readData = "";
	// Tag Class (DATA(資料) EPC(電子產品碼) PORT(通訊桿) RSSI(無線接收信號強度)
	tagData = null;
	// ProductInfo class = null
	pi = null;
	// IPConfiguration class = null
	ipConfiguration = null;
	// List<int>
	optimal_channel = null;
	// ManualResetEvent sealed class
	myResetEvent = new ManualResetEvent(initialState: false);
	inventoryResetEvent = new ManualResetEvent(initialState: false);
	keepAliveResetEvent = new ManualResetEvent(initialState: false);
	// ISendPacker from Assemble with byte[]
	sendPacket = null;
	// BackGoundWorker class
	backGroundWorker = new BackgroundWorker();
	// bool
	RUNNING = false;
	// bool
	IsConnected = false;
	// RFSType(射頻類型) class (ETSI = 0(歐洲電信標準協會) FCC = 1(聯邦通信委員會))
	RFS_Type = RFSType.FCC;
	// List<int>
	AntennaCheckResult = new List<int>();
	// string
	LogHeader = "";
	// Dictionary<int, bool>
	antennaPortExists = new Dictionary<int, bool>();
	// ProductType enum E = 0 R_U = 1, L = 2, UNKNOW = 99
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
