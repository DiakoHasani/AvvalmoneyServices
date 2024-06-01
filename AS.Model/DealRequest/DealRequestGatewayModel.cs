using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.DealRequest
{
    public class DealRequestGatewayModel
    {
        public Guid Drq_Id { get; set; }
        public string Txid { get; set; }
        public DealRequestStatus Drq_Status { get; set; }
        public double Drq_Amount { get; set; }
        public double Drq_TotalPrice { get; set; }
        public long? Usr_Id { get; set; }
    }
}
