﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CodeChallengeEntitie : DbContext
    {
        public CodeChallengeEntitie()
            : base("name=CodeChallengeEntitie")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Tbl_ApprovalStatus> Tbl_ApprovalStatus { get; set; }
        public virtual DbSet<Tbl_Category> Tbl_Category { get; set; }
        public virtual DbSet<Tbl_Challenge> Tbl_Challenge { get; set; }
        public virtual DbSet<Tbl_Example> Tbl_Example { get; set; }
        public virtual DbSet<Tbl_Language> Tbl_Language { get; set; }
        public virtual DbSet<Tbl_Level> Tbl_Level { get; set; }
        public virtual DbSet<Tbl_PointIncDec> Tbl_PointIncDec { get; set; }
        public virtual DbSet<Tbl_RealPerson> Tbl_RealPerson { get; set; }
        public virtual DbSet<Tbl_RealPesronPoint> Tbl_RealPesronPoint { get; set; }
        public virtual DbSet<Tbl_Role> Tbl_Role { get; set; }
        public virtual DbSet<Tbl_User> Tbl_User { get; set; }
        public virtual DbSet<View_ApprovalStatus> View_ApprovalStatus { get; set; }
        public virtual DbSet<View_Challenge> View_Challenge { get; set; }
        public virtual DbSet<View_ChallengeApprovalStatus> View_ChallengeApprovalStatus { get; set; }
        public virtual DbSet<View_ChallengeApprovalStatusCount> View_ChallengeApprovalStatusCount { get; set; }
    }
}
