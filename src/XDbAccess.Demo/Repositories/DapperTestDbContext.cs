using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;

namespace XDbAccess.Demo.Repositories
{
    public class DapperTestDbContext : DbContext
    {
        public DapperTestDbContext(DbContextOptions options) : base(options) { }
    }
}
