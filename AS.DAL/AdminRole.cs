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
    
    public partial class AdminRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdminRole()
        {
            this.AdminRoleActions = new HashSet<AdminRoleAction>();
            this.AdminUsers = new HashSet<AdminUser>();
        }
    
        public System.Guid AdmRole_Id { get; set; }
        public string AdmRole_Title { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminRoleAction> AdminRoleActions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminUser> AdminUsers { get; set; }
    }
}