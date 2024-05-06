using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.DealRequest
{
    public class RequestDealModel
    {
        public int Aff_Id { get; set; }
        public long CPH_Id { get; set; }
        public int Cur_Id { get; set; }
        public double Drq_Cur_Latest_Price { get; set; }
        public double Drq_Amount { get; set; }
        public double Drq_TotalPrice { get; set; }
        public long Usr_Id { get; set; }
        public int Wal_Id { get; set; }
        public DealRequestStatus DealRequestStatus { get; set; }
        public DealRequestType DealRequestType { get; set; }
        public DealRequestVerificationStatus DealRequestVerificationStatus { get; set; }
        public DealRequestVerificationType DealRequestVerificationType { get; set; }
        public string Txid { get; set; }
    }
}
