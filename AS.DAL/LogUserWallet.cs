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
    
    public partial class LogUserWallet
    {
        public int LUW_Id { get; set; }
        public System.DateTime LUW_FromDate { get; set; }
        public Nullable<System.DateTime> LUW_ToDate { get; set; }
        public long Usr_Id { get; set; }
        public int Wal_Id { get; set; }
    
        public virtual User User { get; set; }
        public virtual Wallet Wallet { get; set; }
    }
}
