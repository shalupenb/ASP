using ASP.Services.Kdf;

namespace ASP.Data.DAL
{
	public class DataAccessor
	{
		private readonly DataContext _dataCcontext;
		private readonly IKdfService _kdfService;
		public UserDao UserDao { get; private set; }
        public DataAccessor(DataContext dataCcontext, IKdfService kdfService)
        {
            _dataCcontext = dataCcontext;
            UserDao = new UserDao(dataCcontext, kdfService);
            _kdfService = kdfService;
        }
    }
}
