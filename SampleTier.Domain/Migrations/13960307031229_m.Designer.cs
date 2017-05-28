using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SampleTier.API.Models;

namespace SampleTier.Domain.Migrations
{
    [DbContext(typeof(AngularContext))]
    [Migration("13960307031229_m")]
    partial class m
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SampleTier.API.Models.Staff", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("ID");

                    b.Property<int?>("Code");

                    b.Property<string>("Famil")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Staff");
                });
        }
    }
}
