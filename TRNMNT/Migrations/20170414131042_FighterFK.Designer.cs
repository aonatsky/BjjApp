using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170414131042_FighterFK")]
    partial class FighterFK
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDefaultSchema("trnmnt")
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TRNMNT.Data.Entities.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Fighter", b =>
                {
                    b.Property<Guid>("FighterId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryId");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<Guid>("TeamId");

                    b.Property<Guid>("WeightDivisionId");

                    b.HasKey("FighterId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("TeamId");

                    b.HasIndex("WeightDivisionId");

                    b.ToTable("Fighter");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Team", b =>
                {
                    b.Property<Guid>("TeamId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("TeamId");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.WeightDivision", b =>
                {
                    b.Property<Guid>("WeightDivisionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descritpion");

                    b.Property<string>("Name");

                    b.Property<int>("Weight");

                    b.HasKey("WeightDivisionId");

                    b.ToTable("WeightDivision");
                });

            modelBuilder.Entity("TRNMNT.Data.Entities.Fighter", b =>
                {
                    b.HasOne("TRNMNT.Data.Entities.Category", "Category")
                        .WithMany("Fighters")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TRNMNT.Data.Entities.Team", "Team")
                        .WithMany("Fighters")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TRNMNT.Data.Entities.WeightDivision", "WeightDivision")
                        .WithMany("Fighters")
                        .HasForeignKey("WeightDivisionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
