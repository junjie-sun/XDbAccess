using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Common;

namespace XDbAccess.Test
{
    [Table("User")]
    public class User
    {
        [Field("Id", true, true)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public string Description { get; set; }

        public int OrgId { get; set; }

        [Ignore]
        public int Age
        {
            get
            {
                return (DateTime.Now - Birthday).Days / 365;
            }
        }
    }
}
