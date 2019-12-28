﻿// <auto-generated />
using System;
using LookDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LookDB.Migrations
{
    [DbContext(typeof(LookDBContext))]
    partial class LookDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LookDB.FakeTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("FakeTable","mem");
                });

            modelBuilder.Entity("LookDB.Model.Member.DtCertification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveBool")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("CertEnd")
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("CertName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("CertStart")
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("FileSupport")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("InsertBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("InsertDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2019, 12, 28, 10, 37, 36, 507, DateTimeKind.Local).AddTicks(6754));

                    b.Property<int>("MemberID")
                        .HasColumnType("int");

                    b.Property<string>("UpdateBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DtCertification","mem");
                });

            modelBuilder.Entity("LookDB.Model.Member.DtCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveBool")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("CompCity")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("CompField")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("CompName")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("CompPortal")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("CompProv")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("CompanyAddr")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("CompanyNoReg")
                        .IsRequired()
                        .HasColumnType("nvarchar(13)")
                        .HasMaxLength(13);

                    b.Property<bool>("ContactEmail1")
                        .HasColumnType("bit")
                        .HasMaxLength(60);

                    b.Property<string>("ContactEmail2")
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("ContactHP1")
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("ContactHP2")
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("ContactName1")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("ContactName2")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<bool>("EffectiveBool")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("InsertBy")
                        .HasColumnType("nvarchar(18)")
                        .HasMaxLength(18);

                    b.Property<DateTime>("InsertDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2198));

                    b.Property<string>("MemberType")
                        .HasColumnType("nvarchar(1)")
                        .HasMaxLength(1);

                    b.Property<string>("NPWP")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("ShortProfile")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("SubscribeEnd")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("SubscribeStart")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("UpdateBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CompanyNoReg")
                        .IsUnique();

                    b.ToTable("DtCompany","mem");
                });

            modelBuilder.Entity("LookDB.Model.Member.DtEducation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveBool")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("FileSupport")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("IPK")
                        .HasColumnType("nvarchar(4)")
                        .HasMaxLength(4);

                    b.Property<string>("InsertBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("InsertDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2342));

                    b.Property<string>("Institution")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("InstitutionLocation")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Major")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("MemberID")
                        .HasColumnType("int");

                    b.Property<bool>("OnStudy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("StudyEnd")
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("StudyStart")
                        .IsRequired()
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("UpdateBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FileSupport");

                    b.ToTable("DtEducation","mem");
                });

            modelBuilder.Entity("LookDB.Model.Member.DtExpertise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveBool")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<short>("ExpertLevel")
                        .HasColumnType("smallint")
                        .HasMaxLength(6);

                    b.Property<string>("ExpertName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("FileSupport")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("InsertBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("InsertDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2484));

                    b.Property<int>("MemberID")
                        .HasColumnType("int");

                    b.Property<string>("UpdateBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FileSupport");

                    b.ToTable("DtExpertise","mem");
                });

            modelBuilder.Entity("LookDB.Model.Member.DtLanguage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveBool")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("InsertBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("InsertDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2605));

                    b.Property<short>("LangLevel")
                        .HasColumnType("smallint")
                        .HasMaxLength(6);

                    b.Property<string>("LangName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("MemberID")
                        .HasColumnType("int");

                    b.Property<string>("UpdateBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DtLanguage","mem");
                });

            modelBuilder.Entity("LookDB.Model.Member.DtMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveBool")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("AddressCity")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("AddressProv")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("BackName")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("DateBirth")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("FrontName")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("HP")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("IDCardNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("InsertBy")
                        .HasColumnType("nvarchar(18)")
                        .HasMaxLength(18);

                    b.Property<DateTime>("InsertDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2756));

                    b.Property<bool>("Marital")
                        .HasColumnType("bit");

                    b.Property<string>("MemberNoReg")
                        .IsRequired()
                        .HasColumnType("nvarchar(13)")
                        .HasMaxLength(13);

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("PlaceBirth")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)")
                        .HasMaxLength(1);

                    b.Property<string>("UpdateBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MemberNoReg")
                        .IsUnique();

                    b.ToTable("DtMember","mem");
                });

            modelBuilder.Entity("LookDB.Model.Member.DtOrgExperience", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveBool")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("FileSupport")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("InsertBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("InsertDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2883));

                    b.Property<int>("MemberID")
                        .HasColumnType("int");

                    b.Property<bool>("OnOrg")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("OrgEnd")
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("OrgName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("OrgStart")
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("UpdateBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DtOrgExperience","mem");
                });

            modelBuilder.Entity("LookDB.Model.Member.DtWorkingExperience", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveBool")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("CompField")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("CompName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Dept")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("FileSupport")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("InsertBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("InsertDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(3020));

                    b.Property<int>("MemberID")
                        .HasColumnType("int");

                    b.Property<bool>("OnWork")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Specialization")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("UpdateBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("WorkEnd")
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("WorkStart")
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.HasKey("Id");

                    b.ToTable("DtWorkingExperience","mem");
                });

            modelBuilder.Entity("LookDB.Model.Member.DtWorkingInterest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveBool")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Dept")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("InsertBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("InsertDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(3152));

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("MemberID")
                        .HasColumnType("int");

                    b.Property<string>("Posisi")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<int>("Salary")
                        .HasColumnType("int")
                        .HasMaxLength(50);

                    b.Property<string>("UpdateBy")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DtWorkingInterest","mem");
                });
#pragma warning restore 612, 618
        }
    }
}
