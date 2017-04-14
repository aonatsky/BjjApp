using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TRNMNT.Core.Data;

namespace TRNMNT.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170414083356_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDefaultSchema("trnmnt")
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TRNMNT.Core.Data.Entities.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("TRNMNT.Core.Data.Entities.Fighter", b =>
                {
                    b.Property<Guid>("FighterId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryID");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<Guid>("TeamId");

                    b.HasKey("FighterId");

                    b.HasIndex("CategoryID");

                    b.HasIndex("TeamId");

                    b.ToTable("Fighter");
                });

            modelBuilder.Entity("TRNMNT.Core.Data.Entities.Team", b =>
                {
                    b.Property<Guid>("TeamId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("TeamId");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("TRNMNT.Core.Data.Entities.WeightDivision", b =>
                {
                    b.Property<Guid>("WeightDivisionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descritpion");

                    b.Property<string>("Name");

                    b.Property<int>("Weight");

                    b.HasKey("WeightDivisionId");

                    b.ToTable("WeightDivision");
                });

            modelBuilder.Entity("TRNMNT.Core.Data.Entities.Fighter", b =>
                {
                    b.HasOne("TRNMNT.Core.Data.Entities.Category", "Category")
                        .WithMany("Fighter")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TRNMNT.Core.Data.Entities.Team", "Team")
                        .WithMany("Fighters")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
