using System;
using System.Collections.Generic;
using System.Reflection;
using LookDB.Model.Member;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace LookDB
{
    public partial class LookDBContext : DbContext
    {
        public DbSet<FakeTable> FakeTable { get; set; }
        public DbSet<DtMember> DtMember { get; set; }
        public DbSet<DtCertification> DtCertification { get; set; }
        public DbSet<DtCompany> DtCompany { get; set; }
        public DbSet<DtEducation> DtEducation { get; set; }
        public DbSet<DtExpertise> DtExpertise { get; set; }
        public DbSet<DtLanguage> DtLanguage { get; set; }
        public DbSet<DtOrgExperience> DtOrgExperience { get; set; }
        public DbSet<DtWorkingExperience> DtWorkingExperience { get; set; }
        public DbSet<DtWorkingInterest> DtWorkingInterest { get; set; }

        public LookDBContext()
        {
        }

        public LookDBContext(DbContextOptions<LookDBContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = OptionMsSql(optionsBuilder);
            }
        }
        private string ConnectionString()
        {
            string connstring = string.Format(@"Server=localhost\sqlexpress;Database=LookDB;Trusted_Connection=True;");
            return connstring;
        }
        private DbContextOptionsBuilder OptionMsSql(DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseSqlServer(ConnectionString());
        }
        private DbContextOptionsBuilder OptionMySql(DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseMySql(ConnectionString());
        }
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
        public class IndexAttribute : Attribute
        {

            public bool IsUnique { get; set; }
            public bool IsClustered { get; set; }
        }
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
        public class DefaultAttribute : Attribute
        {
            public object DefaultValue { get; set; }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.SetTableDefaultAttributes(modelBuilder);
            this.SetTableDefaultValues(modelBuilder);
            this.SetTableIndexAttributes(modelBuilder);
        }
        private void SetTableIndexAttributes(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var prop in entity.GetProperties())
                {
                    var attr = prop.PropertyInfo.GetCustomAttribute<IndexAttribute>();
                    if (attr != null)
                    {
                        var index = entity.AddIndex(prop);
                        index.IsUnique = attr.IsUnique;
                        //index.SqlServer.IsClustered = attr.IsClustered;how to make isclustered
                    }

                }
            }
        }
        private void SetTableDefaultAttributes(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    var attr = property.PropertyInfo.GetCustomAttribute<DefaultAttribute>();
                    if (attr != null)
                    {                     
                         modelBuilder.Entity(entity.Name).Property(property.Name).HasDefaultValue(attr.DefaultValue);
                      
                    }

                }
            }
        }
        private void SetTableDefaultValues(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    string myfield = property.Name.Trim().ToLower().Replace("_", "").Replace("<", string.Empty).Replace(">k__BackingField", string.Empty);
                    if (myfield == "activebool")
                        modelBuilder.Entity(entity.Name).Property(property.Name).HasDefaultValue(true);
                    if (myfield == "insertdate")
                        modelBuilder.Entity(entity.Name).Property(property.Name).HasDefaultValue(DateTime.Now);

                }

            }

        }
    }

}

/*How to test
 * 1.cd D:\PROJECTS\EFDBCore\LookDB
 * 2. dotnet ef migrations add InitialCreate //perubahan
 * 3. dotnet ef database update // update dbnya
 * 4.dotnet ef migrations remove // remove initial
 */
