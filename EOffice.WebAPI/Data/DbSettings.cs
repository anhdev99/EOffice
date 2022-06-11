namespace EOffice.WebAPI.Data
{
    public class DbSettings : IDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string LoggingCollectionName { get; set; }
        public string UserCollectionName { get; set; }
        public string RoleCollectionName { get; set; }
        public string ModuleCollectionName { get; set; }
        public string MenuCollectionName { get; set; }
        public string FileCollectionName { get; set; }
        public string RefreshTokenCollectionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DonViCollectionName { get; set; }

        public string LinhVucCollectionName { get; set; }
        public string ChucVuCollectionName { get; set; }
        public string XaCollectionName { get; set; }
        public string HuyenCollectionName { get; set; }
        public string QuestionCollectionName { get; set; }
        public string AnswerCollectionName { get; set; }
        public string HistoryQuestionCollectionName { get; set; }
        public string WarningCollectionName { get; set; }
        public string TrangThaiCollectionName { get; set; }
        public string ModuleTrangThaiCollectionName { get; set; }
    }

    public interface IDbSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string LoggingCollectionName { get; set; }
        string UserCollectionName { get; set; }
        string RoleCollectionName { get; set; }
        string ModuleCollectionName { get; set; }
        string MenuCollectionName { get; set; }
        string FileCollectionName { get; set; }
        string RefreshTokenCollectionName { get; set; }


        string DonViCollectionName { get; set; }
        string LinhVucCollectionName { get; set; }
        string ChucVuCollectionName { get; set; }
        string XaCollectionName { get; set; }
        string HuyenCollectionName { get; set; }
        string QuestionCollectionName { get; set; }
        string AnswerCollectionName { get; set; }
        string HistoryQuestionCollectionName { get; set; }
        string WarningCollectionName { get; set; }
        string TrangThaiCollectionName { get; set; }
        string ModuleTrangThaiCollectionName { get; set; }
    }
}