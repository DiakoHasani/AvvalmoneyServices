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
    
    public partial class ReservationWallet
    {
        public int Rw_Id { get; set; }
        public int Wal_Id { get; set; }
        public System.DateTime RW_CreateDate { get; set; }
        public bool RW_Status { get; set; }
        public double RW_Amount { get; set; }
        public int CryptoType { get; set; }
        public int Aff_Id { get; set; }
    
        public virtual Wallet Wallet { get; set; }
    }
}