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
    
    public partial class Tbl_ApprovalStatus
    {
        public int SId { get; set; }
        public Nullable<int> SQId { get; set; }
        public Nullable<int> SQPid { get; set; }
        public Nullable<int> SQStatus { get; set; }
        public Nullable<System.DateTime> SQDate { get; set; }
        public Nullable<int> ApprovalStatus { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public Nullable<int> ApprovalPID { get; set; }
        public Nullable<int> SAnswerLanguage { get; set; }
        public string SFileAddress { get; set; }
        public string SAnswer { get; set; }
        public Nullable<int> StatusRow { get; set; }
    
        public virtual Tbl_Challenge Tbl_Challenge { get; set; }
        public virtual Tbl_Language Tbl_Language { get; set; }
        public virtual Tbl_RealPerson Tbl_RealPerson { get; set; }
        public virtual Tbl_RealPerson Tbl_RealPerson1 { get; set; }
    }
}
