using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;
using XDbAccess.Dapper;

namespace XDbAccess.Demo.Repositories
{
    public class BaseRepository<DbContextImpl> where DbContextImpl : IDbContext
    {
        public BaseRepository(IDbHelper<DbContextImpl> dbHelper)
        {
            DbHelper = dbHelper;
        }

        public IDbHelper<DbContextImpl> DbHelper { get; set; }
    }
}
