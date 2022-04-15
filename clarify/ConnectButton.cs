// check connect button.text = connect
private void button_connect_Click(object sender, EventArgs e) {
  if (button_connect.Text == "Connect") {
    // check jwReader object is null or not
    if (jwReader == null) {
      // if radioButton is check 
      if (radioButton_serial.Checked) {
        jwReader = new JWReader(comboBox_serial.Text);
      } else {
        jwReader = new JWReader(textBox_ip.Text, 9759);
      }
    }
    //////////////////////////////////////////////////////////////////////////
    //// here is the entry point of the program 
    //////////////////////////////////////////////////////////////////////////
    /// use Result been return value (Result is enumer)
    /// if value = 0
    /// 

    ///UHFException 
    if (jwReader.RFID_Open() == Result.OK) {
      Console.WriteLine("Open Module Success");
      jwReader.TagsReported += TagsReport;
      jwReader.gpiEventReported += GPIEventReport;
      button_connect.Text = "Disconnect";
      button_set.Enabled = true;
      button_inventory.Enabled = true;
      enter_update_thread = true;
      Thread thread = new Thread(Update_Inventory);
      thread.IsBackground = true;
      thread.Start();
    } else {
      Console.WriteLine("Open Module Failure");
    }
  } else {
    jwReader.TagsReported -= TagsReport;
    jwReader.gpiEventReported -= GPIEventReport;
    if (jwReader.RFID_Close() == Result.OK) {
      Console.WriteLine("Close Module Success");
    } else {
      Console.WriteLine("Close Module Failure");
    }
    jwReader = null;
    inventory_update_start = false;
    enter_update_thread = false;
    label_speed.Text = "";
    label_inventory_time.Text = "";
    global_data_table.Clear();
    button_connect.Text = "Connect";
    button_set.Enabled = false;
    button_inventory.Enabled = false;
  }
}
