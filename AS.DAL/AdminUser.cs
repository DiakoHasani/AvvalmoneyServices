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
    
    public partial class AdminUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdminUser()
        {
            this.AdminSMSVerifications = new HashSet<AdminSMSVerification>();
            this.AdminUserHistories = new HashSet<AdminUserHistory>();
            this.CurrencyPriceHistories = new HashSet<CurrencyPriceHistory>();
            this.DealRequests = new HashSet<DealRequest>();
            this.HelpPages = new HashSet<HelpPage>();
            this.TicketItems = new HashSet<TicketItem>();
            this.Transactions = new HashSet<Transaction>();
            this.UserBankCards = new HashSet<UserBankCard>();
            this.UserVerificationDocs = new HashSet<UserVerificationDoc>();
            this.UserWithdraws = new HashSet<UserWithdraw>();
        }
    
        public long AdmUsr_Id { get; set; }
        public System.DateTime AdmUsr_CreateDate { get; set; }
        public System.Guid AdmRole_Id { get; set; }
        public string AdmUsr_Username { get; set; }
        public string AdmUsr_Password { get; set; }
        public string AdmUsr_Fullname { get; set; }
        public string AdmUsr_Email { get; set; }
        public string AdmUsr_Phone { get; set; }
        public bool AdmUsr_IsActive { get; set; }
        public string AdmUsr_SMSCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminSMSVerification> AdminSMSVerifications { get; set; }
        public virtual AdminRole AdminRole { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUserHistory> AdminUserHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CurrencyPriceHistory> CurrencyPriceHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DealRequest> DealRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HelpPage> HelpPages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TicketItem> TicketItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transactions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserBankCard> UserBankCards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserVerificationDoc> UserVerificationDocs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserWithdraw> UserWithdraws { get; set; }
    }
}
