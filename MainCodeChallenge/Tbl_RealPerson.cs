//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MainCodeChallenge
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_RealPerson
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_RealPerson()
        {
            this.Tbl_ApprovalStatus = new HashSet<Tbl_ApprovalStatus>();
            this.Tbl_ApprovalStatus1 = new HashSet<Tbl_ApprovalStatus>();
            this.Tbl_Challenge = new HashSet<Tbl_Challenge>();
        }
    
        public int RP_id { get; set; }
        public Nullable<int> RP_Userid { get; set; }
        public string RP_PersonelId { get; set; }
        public string RP_FName { get; set; }
        public string RP_LName { get; set; }
        public string RP_PhoneNumber { get; set; }
        public string RP_EmailAddress { get; set; }
        public Nullable<byte> RP_Role { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ApprovalStatus> Tbl_ApprovalStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ApprovalStatus> Tbl_ApprovalStatus1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Challenge> Tbl_Challenge { get; set; }
        public virtual Tbl_Role Tbl_Role { get; set; }
        public virtual Tbl_User Tbl_User { get; set; }
        public string getRealPersonFullname()
        {
            return (string)(this.RP_FName + " " + this.RP_LName);
        }
    }
}
