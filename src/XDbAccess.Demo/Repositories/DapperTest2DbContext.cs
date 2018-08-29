using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;

namespace XDbAccess.Demo.Repositories
{
    public class DapperTest2DbContext : DbContext
    {
        public DapperTest2DbContext(DbContextOptions options) : base(options) { }
    }
}
