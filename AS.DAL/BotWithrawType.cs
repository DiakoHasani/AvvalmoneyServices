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
    
    public partial class BotWithrawType
    {
        public long Bwt_Id { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int BotType { get; set; }
        public Nullable<long> Wit_Id { get; set; }
        public bool Repeat { get; set; }
    
        public virtual UserWithdraw UserWithdraw { get; set; }
    }
}
