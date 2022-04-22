using System;
using System.Collections.Generic;
using JW.UHF;

namespace RebuildCubic.JWReaderFolder {
  // JW.UHF.ProductInfo

  public class ProductInfo {
    public int ANTENNA_NUMBER;

    public List<AntennaPort> ANTENNA_PORT_EXISTS_LIST;

    public ChipType CHIP_TYPE;

    public string COMPANY_NO;

    public string MODEL_SEQUENCE_NUMBER;

    public string MODEL_TYPE;

    public int MODEL_VERSION;

    public string PRODUCT_DATE;

    public ProductType PRODUCT_TYPE;

    internal ProductInfo(byte[] packetData) {
      string text = Util.ToHexString(packetData);
      COMPANY_NO = text.Substring(4, 4);
      PRODUCT_DATE = text.Substring(8, 8);
      MODEL_TYPE = text.Substring(16, 4);
      string value = text.Substring(16, 3);
      if ("101".Equals(value)) {
        CHIP_TYPE = ChipType.M100;
      } else if ("105".Equals(value)) {
        CHIP_TYPE = ChipType.R500;
      } else if ("110".Equals(value)) {
        CHIP_TYPE = ChipType.R1000;
      } else if ("120".Equals(value)) {
        CHIP_TYPE = ChipType.R2000;
      }
      MODEL_VERSION = Convert.ToInt16(text.Substring(20, 2), 16);
      MODEL_SEQUENCE_NUMBER = Convert.ToInt64(text.Substring(22, 6), 16).ToString();
      ANTENNA_NUMBER = int.Parse(MODEL_TYPE.Substring(3));
      ANTENNA_PORT_EXISTS_LIST = new List<AntennaPort>();
      for (int i = 0; i < 4; i++) {
        AntennaPort item = new AntennaPort {
          AntennaIndex = i,
          Exist = (ANTENNA_NUMBER > i)
        };
        ANTENNA_PORT_EXISTS_LIST.Add(item);
      }
      if ("12121212".Equals(text.Substring(28))) {
        PRODUCT_TYPE = ProductType.E;
      } else if ("13131313".Equals(text.Substring(28))) {
        PRODUCT_TYPE = ProductType.L;
      } else if ("11111111".Equals(text.Substring(28))) {
        PRODUCT_TYPE = ProductType.R_U;
      } else {
        PRODUCT_TYPE = ProductType.UNKNOWN;
      }
    }
  }

}
