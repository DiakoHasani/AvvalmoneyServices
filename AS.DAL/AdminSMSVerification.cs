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
    
    public partial class AdminSMSVerification
    {
        public int ASV_Id { get; set; }
        public string ASV_Code { get; set; }
        public int ASV_Type { get; set; }
        public long AdmUsr_Id { get; set; }
        public bool ASV_Used { get; set; }
        public System.DateTime ASV_CreateDate { get; set; }
    
        public virtual AdminUser AdminUser { get; set; }
    }
}
