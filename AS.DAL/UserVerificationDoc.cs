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
    
    public partial class UserVerificationDoc
    {
        public long UVD_Id { get; set; }
        public System.DateTime UVD_CreateDate { get; set; }
        public long Usr_Id { get; set; }
        public int UVD_DocType { get; set; }
        public int UVD_Status { get; set; }
        public Nullable<System.DateTime> UVD_UpdateDate { get; set; }
        public string UVD_Tag1 { get; set; }
        public string UVD_Tag2 { get; set; }
        public string UVD_Tag3 { get; set; }
        public Nullable<long> AdmUsr_Id { get; set; }
        public string UVD_ImageExtention { get; set; }
        public string UVD_Description { get; set; }
    
        public virtual User User { get; set; }
        public virtual AdminUser AdminUser { get; set; }
    }
}
