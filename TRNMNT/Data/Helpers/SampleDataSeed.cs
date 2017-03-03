using System;
using System.Linq;
using TRNMNT.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace TRNMNT.Data.Helpers
{
    public static class SampleDataSeed
    {
        public static void EnsureSeedData(this IApplicationBuilder app)
        {
            try
            {


                var context = app.ApplicationServices.GetService<IAppDbContext>();

                if (!context.Team.Any())
                {
                    var teams = new Team[]{
                    new Team {TeamID = new Guid("75031752-076f-4c6f-b1c4-a85b7c15c5b9"), Name = "Ribeiro JJ"},
                    new Team {TeamID = new Guid("70e22d01-6666-421f-ae09-06d37501e401"), Name = "Gracie Barra"},
                    new Team {TeamID = new Guid("1a2e746c-aa9d-49c8-83d4-c2bf4cdd51ad"), Name = "Alliance"},
                    new Team {TeamID = new Guid("35f76f3e-8f3a-4773-9bed-95bf2b8467be"), Name = "Atos"},
                    new Team {TeamID = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538"), Name = "ZR Team"}
                };
                    context.Team.AddRange(teams);
                }
                context.Save();
                if (!context.Fighter.Any())
                {
                    var fighters = new Fighter[]{
                    new Fighter(){FighterID = new Guid("d946322d-554c-437c-812a-da7906419215"),FirstName = "Marcelo", LastName = "Garcia",
                    TeamID = new Guid("1a2e746c-aa9d-49c8-83d4-c2bf4cdd51ad")},
                    new Fighter(){FighterID = new Guid("3c8665c0-be0b-440f-8144-5f9728d1ef61"),FirstName = "Xandi", LastName = "Ribeiro",
                    TeamID = new Guid("75031752-076f-4c6f-b1c4-a85b7c15c5b9")},
                    new Fighter(){FighterID = new Guid("f19487a9-0df2-457f-95eb-cb3dfcd7aef4"),FirstName = "Saulo", LastName = "Ribeiro",
                    TeamID = new Guid("75031752-076f-4c6f-b1c4-a85b7c15c5b9")},
                    new Fighter(){FighterID = new Guid("43b9a18f-dbd6-43c3-bb5c-4a721ea94c6e"),FirstName = "Braulio", LastName = "Estima",
                    TeamID = new Guid("70e22d01-6666-421f-ae09-06d37501e401")},
                    new Fighter(){FighterID = new Guid("bb418fde-6d83-47ac-a22d-0201c58107da"),FirstName = "Felippe", LastName = "Pena",
                    TeamID = new Guid("70e22d01-6666-421f-ae09-06d37501e401")},
                    new Fighter(){FighterID = new Guid("98971bf3-eb3a-4ad9-926e-d4a4348f990f"),FirstName = "Andre", LastName = "Galvao",
                    TeamID = new Guid("35f76f3e-8f3a-4773-9bed-95bf2b8467be")},
                    new Fighter(){FighterID = new Guid("98971bf3-eb3a-4ad9-926e-d4a4348f880f"),FirstName = "Lucas", LastName = "Rocha",
                    TeamID = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0538")},

                };
                    context.Fighter.AddRange(fighters);
                    context.Save(false);
                }


                if (!context.WeightClass.Any())
                {
                    var classes = new WeightClass[]{
                    new WeightClass(){WeightClassID = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0531"), Name = "Feather", Weight = 60},
                    new WeightClass(){WeightClassID = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0532"), Name = "Light", Weight = 70},
                    new WeightClass(){WeightClassID = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0533"), Name = "Medium", Weight = 80},
                    new WeightClass(){WeightClassID = new Guid("c56a4180-65aa-42ec-a945-5fd21dec0534"), Name = "Heavy", Weight = 90}
                };
                    context.WeightClass.AddRange(classes);
                    context.Save(false);
                }

             
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }
    }
}
