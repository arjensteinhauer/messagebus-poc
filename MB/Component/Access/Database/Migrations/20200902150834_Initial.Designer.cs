﻿// <auto-generated />
using System;
using MB.Access.Tenant.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MB.Access.Tenant.Database.Migrations
{
    [DbContext(typeof(TenantContext))]
    [Migration("20200902150834_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("tenant")
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MB.Access.Tenant.Database.Models.Message", b =>
                {
                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("PublishedOnUTC")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("TenantId", "MessageId")
                        .HasName("PK_Message");

                    b.ToTable("Message","tenant");
                });
#pragma warning restore 612, 618
        }
    }
}
