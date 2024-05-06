using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Ex4Ir
{
    public class Ex4TokenModel
    {
        public IEnumerable<Cookie> Cookies { get; set; }
        public string CsrfToken { get; set; }
    }
}
