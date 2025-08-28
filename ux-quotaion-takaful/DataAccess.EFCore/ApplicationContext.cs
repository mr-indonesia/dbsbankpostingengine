using DataAccess.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("dbo");

            //set base entity
            //ex : builder.Entity<NumberSequenceTable>().HasKey(e => new { e.SeqId, e.UniqueId });
            builder.Entity<User>().HasKey(e => new { e.UserCode });
            builder.Entity<CompanyBranch>().HasKey(e => new { e.BranchCode });
            builder.Entity<UserMapping>().HasKey(e => new { e.RoleApplicationID, e.UserID });
            builder.Entity<WFHeader>().HasKey(e => new { e.HeaderID});
            builder.Entity<WFDetail>().HasKey(e => new { e.HeaderID, e.State, e.RoleID, e.AmountID});
            builder.Entity<WFAmount>().HasKey(e => new { e.Code});
            builder.Entity<RoleAccess>().HasKey(e => new { e.RoleAccessID});

            //set scheme
            //ex : builder.Entity<NumberSequenceTable>().ToTable("NumberSequenceTable", "dbo");
            builder.Entity<User>().ToTable("MstUsers", "dbo");
            builder.Entity<CompanyBranch>().ToTable("MstBranch", "dbo");
            builder.Entity<UserMapping>().ToTable("RoleApplication", "dbo");
            builder.Entity<WFHeader>().ToTable("WFHeader", "dbo");
            builder.Entity<WFDetail>().ToTable("WFDetail", "dbo");
            builder.Entity<WFAmount>().ToTable("WFAmount", "dbo");
            builder.Entity<RoleAccess>().ToTable("RoleAccess", "dbo");
        }

        //set dbset
        //example : public virtual DbSet<NumberSequenceTable> NumberSequenceTable { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<CompanyBranch> CompanyBranches { get; set; }
        public virtual DbSet<UserMapping> UserMappings { get; set; }
        public virtual DbSet<WFHeader> WFHeaders { get; set; }
        public virtual DbSet<WFDetail> WFDetails { get; set; }
        public virtual DbSet<WFAmount> WFAmounts { get; set; }
        public virtual DbSet<RoleAccess> RoleAccesses { get; set; }
    }
}
