//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AS.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class DepositeWallet
    {
        public System.Guid Dpw_Id { get; set; }
        public double Dpw_Amount { get; set; }
        public long Usr_Id { get; set; }
        public int Dpw_Status { get; set; }
        public int Aff_Id { get; set; }
        public System.DateTime Dpw_CreateDate { get; set; }
        public string Dpw_RefrenceID { get; set; }
    
        public virtual Affiliate Affiliate { get; set; }
        public virtual User User { get; set; }
    }
}
