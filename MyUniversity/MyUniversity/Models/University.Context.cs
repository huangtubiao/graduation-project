﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyUniversity.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UniversityEntities : DbContext
    {
        public UniversityEntities()
            : base("name=UniversityEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<City> City { get; set; }
        public DbSet<Depart> Depart { get; set; }
        public DbSet<FollowType> FollowType { get; set; }
        public DbSet<Love> Love { get; set; }
        public DbSet<Plancomment> Plancomment { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<RecentVisitor> RecentVisitor { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<RecoverAnswer> RecoverAnswer { get; set; }
        public DbSet<hotSuperior> hotSuperior { get; set; }
        public DbSet<QCategory> QCategory { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Supervice> Supervice { get; set; }
        public DbSet<Plan> Plan { get; set; }
        public DbSet<PlanComeOn> PlanComeOn { get; set; }
        public DbSet<Clocklog> Clocklog { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<LastChat> LastChat { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Follows> Follows { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<ArticleComment> ArticleComment { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<ProjectRecord> ProjectRecord { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<SuperiorLetter> SuperiorLetter { get; set; }
    }
}
