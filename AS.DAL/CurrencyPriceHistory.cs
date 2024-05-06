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
    
    public partial class CurrencyPriceHistory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CurrencyPriceHistory()
        {
            this.DealRequests = new HashSet<DealRequest>();
        }
    
        public long CPH_Id { get; set; }
        public int Cur_Id { get; set; }
        public System.DateTime CPH_CreateDate { get; set; }
        public double CPH_SellPrice { get; set; }
        public double CPH_BuyPrice { get; set; }
        public Nullable<long> AdmUsr_Id { get; set; }
    
        public virtual AdminUser AdminUser { get; set; }
        public virtual Currency Currency { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DealRequest> DealRequests { get; set; }
    }
}