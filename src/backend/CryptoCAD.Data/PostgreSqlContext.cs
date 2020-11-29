using Microsoft.EntityFrameworkCore;

namespace CryptoCAD.Data
{
    public class PostgreSqlContext : DbContext
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)
        {

        }
    }
}