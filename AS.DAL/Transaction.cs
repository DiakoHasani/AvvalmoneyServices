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
    
    public partial class Transaction
    {
        public System.Guid Tns_Id { get; set; }
        public System.DateTime Tns_CreateDate { get; set; }
        public long Usr_Id { get; set; }
        public double Tns_Befor { get; set; }
        public double Tns_Amount { get; set; }
        public double Tns_After { get; set; }
        public int Tns_Type { get; set; }
        public string Tns_Tag { get; set; }
        public Nullable<long> AdmUsr_Id { get; set; }
    
        public virtual AdminUser AdminUser { get; set; }
        public virtual User User { get; set; }
    }
}