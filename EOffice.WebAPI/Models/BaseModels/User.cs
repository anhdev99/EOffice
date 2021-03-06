using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using EOffice.WebAPI.ViewModels;

namespace EOffice.WebAPI.Models
{
    public class User : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }
        public string UserNameKySo { get; set; }
        public string PasswordKySo { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public Avatar Avatar { get; set; }
        public KySo KySo { get; set; }
        public DonVi DonVi { get; set; }
        public ChucVu ChucVu { get; set; }
        public List<Role> Roles { get; set; }

        public List<string> DonViIds { get; set; }

        public bool IsAppAuthentication { get; set; } = false;
        public bool IsVerified { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsSyncPasswordSuccess { get; set; } = true;
        public byte[] PrivateKey, PublicKey;
        public string SignPath, PrivateKey_string, PublicKey_string;
        public string PIN { get; set; }
        public string EOfficeId { get; set; }
        [BsonIgnore]   public bool IsRequireChangePassword { get; set; } = false;
        [BsonIgnore]     public bool IsRequireVerify { get; set; } = false;
        [BsonIgnore] public string Password { get; set; }
        [BsonIgnore] public List<string> Permissions { get; set; }
        [BsonIgnore] public List<NavMenuVM> Menu { get; set; }
        [BsonIgnore] public string NgayKy { get; set; }
        [BsonIgnore] public string FilePath { get; set; }
    }

    public class UserShort
    {
        public UserShort(){}

        public UserShort(User model)
        {
            this.Id = model.Id;
            this.FullName = model.FullName;
            this.DonVi = model.DonVi;
            this.ChucVu = model.ChucVu;
            this.Avatar = model.Avatar;
            this.Note = model.Note;
            this.PhoneNumber = model.PhoneNumber;
            this.Email = model.Email;
        }
        public string Id { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public Avatar Avatar { get; set; }
        public DonVi DonVi { get; set; }
        public ChucVu ChucVu { get; set; }
    }
}