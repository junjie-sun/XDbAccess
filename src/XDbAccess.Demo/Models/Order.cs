using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Common;

namespace XDbAccess.Demo.Models
{
    public class Order
    {
        [Field("Id", true, true)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal Total { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
