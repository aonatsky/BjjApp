using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TRNMNT.Core.Data;

namespace TRNMNT.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170403144328_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDefaultSchema("trnmnt")
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TRNMNT.Core.Data.Entities.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("CategoryID");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("TRNMNT.Core.Data.Entities.Fighter", b =>
                {
                    b.Property<Guid>("FighterID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CategoryID");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<Guid>("TeamID");

                    b.HasKey("FighterID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("TeamID");

                    b.ToTable("Fighter");
                });

            modelBuilder.Entity("TRNMNT.Core.Data.Entities.Team", b =>
                {
                    b.Property<Guid>("TeamID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("TeamID");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("TRNMNT.Core.Data.Entities.WeightDivision", b =>
                {
                    b.Property<Guid>("WeightDivisionID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descritpion");

                    b.Property<string>("Name");

                    b.Property<int>("Weight");

                    b.HasKey("WeightDivisionID");

                    b.ToTable("WeightDivision");
                });

            modelBuilder.Entity("TRNMNT.Core.Data.Entities.Fighter", b =>
                {
                    b.HasOne("TRNMNT.Core.Data.Entities.Category", "Category")
                        .WithMany("Fighter")
                        .HasForeignKey("CategoryID");

                    b.HasOne("TRNMNT.Core.Data.Entities.Team", "Team")
                        .WithMany("Fighters")
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
