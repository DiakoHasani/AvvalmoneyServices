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
    
    public partial class Affiliate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Affiliate()
        {
            this.DepositeWallets = new HashSet<DepositeWallet>();
            this.DealRequests = new HashSet<DealRequest>();
            this.Domains = new HashSet<Domain>();
            this.Settings = new HashSet<Setting>();
            this.Users = new HashSet<User>();
            this.UserBankCards = new HashSet<UserBankCard>();
            this.UserWithdraws = new HashSet<UserWithdraw>();
            this.Wallets = new HashSet<Wallet>();
        }
    
        public int Aff_Id { get; set; }
        public string Aff_Title { get; set; }
        public bool Aff_IsActive { get; set; }
        public int Aff_Type { get; set; }
        public string Aff_TitleEN { get; set; }
        public string Aff_TelegramID { get; set; }
        public string Aff_InstagramID { get; set; }
        public string Aff_TwitterID { get; set; }
        public string Aff_TitleText { get; set; }
        public string Aff_Logo { get; set; }
        public string Aff_favicon { get; set; }
        public string Aff_footer { get; set; }
        public string Aff_bg_Primary { get; set; }
        public string Aff_bg_Secondary { get; set; }
        public string Aff_bg_Third { get; set; }
        public string Aff_text_Primary { get; set; }
        public string Aff_text_Secondary { get; set; }
        public string Aff_text_Third { get; set; }
        public string Aff_body_bg { get; set; }
        public string Aff_body_text { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DepositeWallet> DepositeWallets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DealRequest> DealRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Domain> Domains { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Setting> Settings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserBankCard> UserBankCards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserWithdraw> UserWithdraws { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}