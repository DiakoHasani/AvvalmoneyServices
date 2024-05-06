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
    
    public partial class Wallet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Wallet()
        {
            this.DealRequests = new HashSet<DealRequest>();
            this.ReservationWallets = new HashSet<ReservationWallet>();
            this.TransactionIds = new HashSet<TransactionId>();
            this.LogUserWallets = new HashSet<LogUserWallet>();
            this.UserWalletReservations = new HashSet<UserWalletReservation>();
        }
    
        public int Wal_Id { get; set; }
        public string Address { get; set; }
        public bool Enabled { get; set; }
        public int CryptoType { get; set; }
        public int Aff_Id { get; set; }
        public Nullable<System.DateTime> LastTransaction { get; set; }
        public string MemoTag { get; set; }
        public bool Private { get; set; }
        public Nullable<long> Usr_Id { get; set; }
    
        public virtual Affiliate Affiliate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DealRequest> DealRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservationWallet> ReservationWallets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionId> TransactionIds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogUserWallet> LogUserWallets { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserWalletReservation> UserWalletReservations { get; set; }
    }
}
