using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using EOffice.WebAPI.Extensions;

namespace EOffice.WebAPI.Models
{
    public class Question : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string Code { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Content { get; set; }
        public StatusQuestion LastedStatus { get; set; } = new StatusQuestion();

        public bool IsPrivate { get; set; } = false;
        public string UserId { get; set; }
        public string UserName { get; set; }

        [BsonIgnore]
        public string UserNameShow
        {
            get { return UserName;  }
            // get { return UserName.HideString(); }
        }
        //for guess information
        [BsonIgnore] public bool IsGuess { get; set; }
        [BsonIgnore] public string PhoneNumber { get; set; }
        
        public List<AnswerModel> Answers { get; set; }
        
        // public string HuyenId { get; set; }
        // public string HuyenName { get; set; }
        public HuyenShort Huyen { get; set;  }
        
        public string IdOwner { get; set;  }
        
        [BsonIgnore]
        public bool isShow { get; set; } = false;
        
        public  XaShort Xa { get; set;  }
        // public string XaId { get; set; }
        // public string XaName { get; set; }
        
        public  LinhVucShort LinhVuc { get; set;  }
        // public string LinhVucId { get; set; }  
        // public string LinhVucName { get; set; }
        public string Address { get; set; }
        public List<FileShort> FileManagers { get; set; } = new List<FileShort>();
        

        public List<FileShort> FileImage { get; set; } = new List<FileShort>();
        
        public List<FileShort> FileOffice { get; set; } = new List<FileShort>();

        [BsonIgnore]
        public List<FileShort> UploadFiles { get; set; } = new List<FileShort>();
        
        public DonViShort DonVi { get; set;  } 
        
        public  string Note { get; set;  }

        // [BsonIgnore] 
        // public BrowsingStatus  browsingStatus { get; set; }  
        // public List<FileShort> UploadFilesGiaiNgan { get; set; }
    }

    // public static class DefineStatus
    // {
    //     // public const int DaTraLoiNguoiDan = 100;
    //     // public const int VuaTiepNhan = 5;
    //     // public const int DangXuLy = 1;
    //     // public const int ChoDuyet = 0;
    //     // public const int DaXuLyXong = 99;
    //     // public const int DaHuy = -1;
    //     
    //     public const int KhongTiepNhan = -1; // Khi qu???n tr??? h??? th???ng v???n ????? kh??ng n???m trong ph???m v??? x??? l?? n??n kh??ng ti???p nh???n
    //     public const int ChoDuyet = 0; // Khi ng?????i d??n v???a t???o ph???n ??nh ki???n ngh??? th?? v??o tr???ng th??i ch??? duy???t
    //     public const int VuaTiepNhan = 1; // Sau khi qu???n tr??? vi??n h??? th???ng xem ph???n ??nh v?? ch???p nh???n ph???n ??nh th?? v??o tr???ng th??i v???a ti???p nh???n. 
    //     public const int DangXuLy = 2; // Sau khi qu???n tr??? vi??n h??? th???ng chuy???n ?????n c?? quan m?? c?? quan ti???p nh???n th?? chuy???n sang tr???ng th??i ??ang x??? l?? . 
    //     // N???u c?? quan x??? l?? c???m th???y v???n ????? kh??ng ph?? h???p t??? ch???i ti???p nh???n th?? tr???ng th??i v???n ??? tr???ng th??i V???a ti???p nh???n.
    //     public const int DaXuLyXong = 3; // Sau khi c?? quan x??? l?? xong th??  s??? qua tr???ng th??i ???? x??? l?? xong . 
    //     public const int DaTraLoiNguoiDan = 4; // Sau khi c?? quan nh???n ph???n h???i ng?????i d??n th?? s??? qua tr???ng th??i ???? tr??? l???i ng?????i d??n
    //     
    //     public static string GetStatusString(int status)
    //     {
    //         string result = "";
    //         switch (status)
    //         {
    //             case KhongTiepNhan:
    //                     result = "Kh??ng ti???p nh???n";
    //                 break;
    //             case ChoDuyet:
    //                 result = "Ch??? duy???t";
    //                 break;
    //             case VuaTiepNhan:
    //                 result = "V???a ti???p nh???n";
    //                 break;
    //             case DangXuLy:
    //                 result = "??ang x??? l??";
    //                 break;
    //             case DaXuLyXong:
    //                 result = "???? x??? l?? xong";
    //                 break;
    //             case DaTraLoiNguoiDan:
    //                 result = "???? tr??? l???i ng?????i d??n";
    //                 break;
    //             default:
    //                 break;
    //         }
    //
    //         return result;
    //     }
    //
    //     public static IEnumerable<int> GetAvailableStatuses()
    //     {
    //         return new List<int>
    //         {
    //             DangXuLy, DaTraLoiNguoiDan, VuaTiepNhan, DaXuLyXong
    //         };
    //     }
    // }

    public class AnswerModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Comment { get; set; }
        public string PathFile { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        [BsonIgnore]
        public string UserNameShow
        {
            get { return UserName.HideString(); }
        }

        public string Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public FileShort FileManager { get; set; }
    }


    public enum EQAStatus
    {
        REQUESTED,
        RESPONCED,
        ANSWERED,
        ENDED,
        NONE
    }

    public class BrowsingStatus
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        
        public  string idTo { get; set;  }
        public string tenTo { get; set;  }
    }
    
    
}