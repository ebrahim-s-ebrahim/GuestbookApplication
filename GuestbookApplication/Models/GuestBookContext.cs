using System.Data.SqlClient;

namespace GuestbookApplication.Models
{
    public class GuestBookContext : IDisposable
    {
        private readonly IConfiguration _config;
        private SqlConnection? _context;
        private static readonly Object _lock = new object();

        public GuestBookContext(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection DbContext
        {
            get
            {
                lock (_lock)
                {
                    if (_context is null)
                        _context = new SqlConnection(_config.GetConnectionString("Default"));
                    return _context;
                }
            }
        }

        public void Dispose()
        {
            _context.Close();
            _context.Dispose();
        }
    }
}
