using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Data
{
    public static class ClaimTypes
    {
        public static List<string> AllClaimTypeList { get; set; } = new List<string>
        {
            "Edit Role",
            "Edit User",
            "Edit Albums",
            "Email"
        };
    }
}
