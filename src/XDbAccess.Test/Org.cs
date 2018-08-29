using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Dapper;

namespace XDbAccess.Test
{
    public class Org
    {
        [Field("Id", true, true)]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
