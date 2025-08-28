using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EFCore //Infrastructure.Data
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //set default schema
            builder.HasDefaultSchema("dbo");

			//set primary key
			/*builder.Entity<MstUser>().HasKey(e => e.UserId);
            builder.Entity<UserRole>().HasKey(e => e.Id);
            builder.Entity<Roles>().HasKey(e => e.RoleId);
            builder.Entity<RefreshToken>().HasKey(e => e.RefreshTokenId);*/
			builder.Entity<MGRAgent>().HasKey(e => e.CODE);
			builder.Entity<MstCompanyType>().HasKey(e => e.Code);
			builder.Entity<MstCompanyAgent>().HasKey(e => e.CategoryCode);

			//set table schema
			/*builder.Entity<MstUser>().ToTable("MstUser", "dbo");
            builder.Entity<Roles>().ToTable("Roles", "dbo");
            builder.Entity<UserRole>().ToTable("UserRole", "dbo");
            builder.Entity<RefreshToken>().ToTable("RefreshToken", "dbo");*/
			builder.Entity<MGRAgent>().ToTable("MGRAgent", "dbo");
			builder.Entity<MstCompanyType>().ToTable("MstCompanyType", "dbo");
			builder.Entity<MstCompanyAgent>().ToTable("MstCompanyAgent", "dbo");
		}
		/*public virtual DbSet<MstUser> MstUsers { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }*/
		public virtual DbSet<MGRAgent> MGRAgents { get; set; }
		public virtual DbSet<MstCompanyType> MstCompanyTypes { get; set; }
        public virtual DbSet<MstCompanyAgent> MstCompanyAgents { get; set; }
	}
}
