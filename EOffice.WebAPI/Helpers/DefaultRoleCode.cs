﻿using System.Collections.Generic;

namespace EOffice.WebAPI.Helpers
{
    public class DefaultRoleCode
    {
        public const string HIEU_TRUONG = "9999";
        public const string THU_KY_HIEU_TRUONG = "9998";
        public const string VAN_THU_TRUONG = "9997";
        
        public const string TRINH_LANH_DAO_TRUONG = "TLDT";
        public const string TRINH_THU_KY_HIEU_TRUONG = "TKHTXN";
        public const string KY_SO_PHAP_LY = "HTKS";
        public const string TU_CHOI = "TC";
        public const string BAN_HANH = "BH";
        public const string DA_DUYET = "DD";
        public const string HIEU_TRUONG_DA_DUYET = "HTD";
        public const string HIEU_TRUONG_DA_KY = "HTK";
        public const string HIEU_TRUONG_TU_CHOI_DUYET = "HTTCD";
        public const string HIEU_TRUONG_TU_CHOI_KY = "HTTTK";
        public const string DUYET_VAN_BAN_PHAP_LY = "DVBPL";
        public const string VAN_THU_TRUONG_TU_CHOI_DUYET = "VTTTC";
        public const string THU_KY_HIEU_TRUONG_TU_CHOI_DUYET = "TKHTTC";
        public const string KHOI_TAO_VAN_BAN = "KTVB";

        public static List<string> TrangThaiCapTruong = new List<string>() { "KPL",  "DVBPL" };
        public static List<string> TrangThaiGhiNhanThongTin = new List<string>() {THU_KY_HIEU_TRUONG_TU_CHOI_DUYET,VAN_THU_TRUONG_TU_CHOI_DUYET,DUYET_VAN_BAN_PHAP_LY, TRINH_THU_KY_HIEU_TRUONG,  KY_SO_PHAP_LY , TU_CHOI, DA_DUYET, HIEU_TRUONG_DA_DUYET, HIEU_TRUONG_DA_KY, HIEU_TRUONG_TU_CHOI_DUYET, HIEU_TRUONG_TU_CHOI_KY};
    }
}