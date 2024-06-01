using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.DealRequest
{
    public class DealRequestModel
    {
        public Guid Drq_Id { get; set; }
        public DateTime Drq_CreateDate { get; set; }
        public int Aff_Id { get; set; }
        public int Cur_Id { get; set; }
        public double Drq_Amount { get; set; }
        public double Drq_Cur_Latest_Price { get; set; }
        public long CPH_Id { get; set; }
        public double Drq_TotalPrice { get; set; }
        public long? Usr_Id { get; set; }
        public DealRequestType Drq_Type { get; set; }
        public DealRequestStatus Drq_Status { get; set; }
        public string Drq_RefrenceID { get; set; }
        public string Drq_Tag1 { get; set; }
        public string Drq_Tag2 { get; set; }
        public DealRequestVerificationType? Drq_VerificationType { get; set; }
        public DealRequestVerificationStatus Drq_VerificationStatus { get; set; }
        public long? AdmUsr_Id { get; set; }
        public string AdmUsr_Description { get; set; }
        public int? Wal_Id { get; set; }
        public string Drq_UsrWalletAddress { get; set; }
        public string Txid { get; set; }
    }
}
