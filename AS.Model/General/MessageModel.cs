using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.General
{
    public class MessageModel
    {
        public int Code { get; set; } = 0;
        public string Message { get; set; } = "";
        public bool IsValid { get; set; } = false;
    }
}
