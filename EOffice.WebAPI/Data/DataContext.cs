using System.Security.Authentication;
using System.Threading.Tasks;
using MongoDB.Driver;
using EOffice.WebAPI.Models;

namespace EOffice.WebAPI.Data
{
    public class DataContext
    {
        private readonly IMongoClient _mongoClient = null;
        private readonly IMongoDatabase _context = null;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Role> _roles;
        private readonly IMongoCollection<Menu> _menu;
        private readonly IMongoCollection<Module> _module;
        private readonly IMongoCollection<File> _file;
        private readonly IMongoCollection<Logging> _logging;
        private readonly IMongoCollection<RefreshToken> _refreshToken;

        // Nghiệp vụ
        private readonly IMongoCollection<DonVi> _donVi;
        private readonly IMongoCollection<LinhVuc> _linhVuc;
        private readonly IMongoCollection<ChucVu> _chucVu;
        private readonly IMongoCollection<Xa> _xa;
        private readonly IMongoCollection<Huyen> _huyen;

        private readonly IMongoCollection<Question> _question;
        private readonly IMongoCollection<Answer> _answer;
        private readonly IMongoCollection<HistoryQuestion> _historyQuestion;
        private readonly IMongoCollection<Warning> _warning;
        private readonly IMongoCollection<TrangThai> _trangThai;
        private readonly IMongoCollection<ModuleTrangThai> _moduleTrangThai;
        private readonly IMongoCollection<Notify> _notify;
        private readonly IMongoCollection<VanBanDen> _vanBanDen;
        private readonly IMongoCollection<VanBanDi> _vanBanDi;
        private readonly IMongoCollection<LichCongTac> _lichCongTac;
        private readonly IMongoCollection<LoaiVanBan> _loaiVanBan;
        
        private readonly IDbSettings _settings;
        public DataContext(IDbSettings settings)
        {
            _settings = settings;

            MongoClientSettings settings1 = new MongoClientSettings();
            settings1.Server = new MongoServerAddress("anhdev99.com", 27017);
            settings1.UseTls = false;
            settings1.SslSettings = new SslSettings();
            settings1.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity("EOffice", "admin");
            MongoIdentityEvidence evidence = new PasswordEvidence("DongThap@123");

            settings1.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);
             var client = new MongoClient(settings1);
            if (client != null)
            {
                _context = client.GetDatabase(_settings.DatabaseName);
            }
            
            _users = _context.GetCollection<User>(_settings.UserCollectionName);
            _roles = _context.GetCollection<Role>(_settings.RoleCollectionName);
            _menu = _context.GetCollection<Menu>(_settings.MenuCollectionName);
            _module = _context.GetCollection<Module>(_settings.ModuleCollectionName);
            _file = _context.GetCollection<File>(_settings.FileCollectionName);
            _logging = _context.GetCollection<Logging>(_settings.LoggingCollectionName);
            _refreshToken = _context.GetCollection<RefreshToken>(_settings.RefreshTokenCollectionName);
            
            // Nghiệp vụ
            _donVi = _context.GetCollection<DonVi>(_settings.DonViCollectionName);
            _linhVuc = _context.GetCollection<LinhVuc>(_settings.LinhVucCollectionName);
            _chucVu = _context.GetCollection<ChucVu>(_settings.ChucVuCollectionName);
            _xa = _context.GetCollection<Xa>(_settings.XaCollectionName);
            _huyen = _context.GetCollection<Huyen>(_settings.HuyenCollectionName);
            
            _question = _context.GetCollection<Question>(_settings.QuestionCollectionName);
            _answer = _context.GetCollection<Answer>(_settings.AnswerCollectionName);
            _historyQuestion = _context.GetCollection<HistoryQuestion>(_settings.HistoryQuestionCollectionName);
            _warning = _context.GetCollection<Warning>(_settings.WarningCollectionName);
            _trangThai = _context.GetCollection<TrangThai>(_settings.TrangThaiCollectionName);
            _moduleTrangThai = _context.GetCollection<ModuleTrangThai>(_settings.ModuleTrangThaiCollectionName);
            _notify = _context.GetCollection<Notify>(_settings.NotifyCollectionName);
            _vanBanDen = _context.GetCollection<VanBanDen>(_settings.VanBanDenCollectionName);
            _vanBanDi = _context.GetCollection<VanBanDi>(_settings.VanBanDiCollectionName);
            _lichCongTac = _context.GetCollection<LichCongTac>(_settings.VanBanDiCollectionName);
            _loaiVanBan = _context.GetCollection<LoaiVanBan>(_settings.LoaiVanBanCollectionName);
        }
        
        public IMongoDatabase Database
        {
            get { return _context; }
        }
        public IMongoClient Client
        {
            get { return _mongoClient; }
        }
        public IMongoCollection<User> Users { get => _users; }
        public IMongoCollection<Role> Roles { get => _roles; }
        public IMongoCollection<Menu> Menu { get => _menu; }
        public IMongoCollection<File> Files { get => _file; }
        public IMongoCollection<Module> Modules { get => _module; }
        public IMongoCollection<Logging> Loggings { get => _logging; }
        public IMongoCollection<RefreshToken> RefreshToken { get => _refreshToken; }
        
        // Nghiệp vụ
        public IMongoCollection<DonVi> DonVis { get => _donVi; }
        public IMongoCollection<LinhVuc> LinhVuc { get => _linhVuc; }
        public IMongoCollection<ChucVu> ChucVu { get => _chucVu; }
        public IMongoCollection<Xa> Xa { get => _xa; }
        public IMongoCollection<Huyen> Huyen { get => _huyen; }
        public IMongoCollection<Question> Questions { get => _question; }
        public IMongoCollection<Answer> Answers { get => _answer; }
        public IMongoCollection<HistoryQuestion> HistoryQuestions { get => _historyQuestion; }
        public IMongoCollection<Warning> Warning { get => _warning; }
        public IMongoCollection<TrangThai> TrangThai { get => _trangThai; }
        public IMongoCollection<ModuleTrangThai> ModuleTrangThai { get => _moduleTrangThai; }
        public IMongoCollection<Notify> Notify { get => _notify; }
        public IMongoCollection<VanBanDen> VanBanDen { get => _vanBanDen; }
        public IMongoCollection<VanBanDi> VanBanDi { get => _vanBanDi; }
        public IMongoCollection<LichCongTac> LichCongTac { get => _lichCongTac; }
        public IMongoCollection<LoaiVanBan> LoaiVanBan { get => _loaiVanBan; }
    }
}