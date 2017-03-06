using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TRNMNT.Core.Data;
using TRNMNT.Core.Data.Entities;

namespace TRNMNT.Core.Services.impl
{
    public class FighterServiceFake: IFighterService
    {
        public FighterServiceFake(IAppDbContext context) 
        {
           
        }

       
       public IQueryable<Fighter> GetFightersByWeightDivision(Guid WeightDivisionID)
        {
            var result =  new List<Fighter>();
            result.Add(new Fighter(){FighterID = new Guid("d946322d-554c-437c-812a-da7906419215"),FirstName = "Marcelo", LastName = "Garcia", 
                    TeamID = new Guid("1a2e746c-aa9d-49c8-83d4-c2bf4cdd51ad"), Team = new Team {TeamID = new Guid("1a2e746c-aa9d-49c8-83d4-c2bf4cdd51ad"), Name = "Alliance"}});
            result.Add(new Fighter(){FighterID = new Guid("3c8665c0-be0b-440f-8144-5f9728d1ef61"),FirstName = "Xandi", LastName = "Ribeiro", 
                    TeamID = new Guid("f19487a9-0df2-457f-95eb-cb3dfcd7ae55"), Team = new Team {TeamID = new Guid("75031752-076f-4c6f-b1c4-a85b7c15c5b9"), Name = "Ribeiro JJ"}});                    
            result.Add(new Fighter(){FighterID = new Guid("f19487a9-0df2-457f-95eb-cb3dfcd7aef4"),FirstName = "Saulo", LastName = "Ribeiro", 
                    TeamID = new Guid("75031752-076f-4c6f-b1c4-a85b7c15c5b9"), Team = new Team {TeamID = new Guid("75031752-076f-4c6f-b1c4-a85b7c15c5b9"), Name = "Ribeiro JJ"}});
            result.Add(new Fighter(){FighterID = new Guid("f19487a9-0df2-457f-95eb-cb3dfcd7aef4"),FirstName = "Saulo", LastName = "Ribeiro", 
                    TeamID = new Guid("75031752-076f-4c6f-b1c4-a85b7c15c5b9"), Team = new Team {TeamID = new Guid("75031752-076f-4c6f-b1c4-a85b7c15c5b9"), Name = "Ribeiro JJ"}});
            result.Add(new Fighter(){FighterID = new Guid("43b9a18f-dbd6-43c3-bb5c-4a721ea94c6e"),FirstName = "Braulio", LastName = "Estima", 
                    TeamID = new Guid("70e22d01-6666-421f-ae09-06d37501e401"), Team = new Team {TeamID = new Guid("70e22d01-6666-421f-ae09-06d37501e401"), Name = "Gracie Barra"}});
            result.Add(new Fighter(){FighterID = new Guid("bb418fde-6d83-47ac-a22d-0201c58107da"),FirstName = "Felippe", LastName = "Pena", 
                    TeamID = new Guid("70e22d01-6666-421f-ae09-06d37501e401"), Team = new Team {TeamID = new Guid("70e22d01-6666-421f-ae09-06d37501e401"), Name = "Gracie Barra"}});
            result.Add(new Fighter(){FighterID = new Guid("98971bf3-eb3a-4ad9-926e-d4a4348f990f"),FirstName = "Andre", LastName = "Galvao", 
                    TeamID = new Guid("35f76f3e-8f3a-4773-9bed-95bf2b8467be"), Team = new Team {TeamID = new Guid("35f76f3e-8f3a-4773-9bed-95bf2b8467be"), Name = "Atos"}});
            
            
            return result.AsQueryable();
        }


        public bool ProcessFighterListFromFile(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
