using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services
{
    public class ParticipantService: IParticipantService
    {
        private IRepository<Participant> repository;

        public ParticipantService(IRepository<Participant> repository)
        {
            this.repository = repository;
        }

        public async Task CreateParticipant(Participant participant)
        {
            
        }
    }
}
