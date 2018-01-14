using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Model.Round;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Migrations;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.impl
{
    public class BracketService : IBracketService
    {
        private readonly IRepository<Bracket> _bracketRepository;
        private readonly IRoundService _roundService;
        

        public BracketService(IRepository<Bracket> bracketRepository, IRoundService roundService)
        {
            _bracketRepository = bracketRepository;
            _roundService = roundService;
        }
        
        public async Task<BracketModel> CreateBracketAsync(Guid weightDivisionId)
        {
            var participants = GetParticipants();
            var bracketModel = new BracketModel();
            bracketModel.BracketId = Guid.NewGuid();
            bracketModel.RoundModels = _roundService.GetRoundStructure(participants.ToArray(),bracketModel.BracketId).Select(r => new RoundModel()
            {
                RoundId = r.RoundId,
                NextRoundId = r.NextRoundId,
                SecondParticipantId = r.SecondParticipantId,
                FirstParticipantId = r.FirstParticipantId,
                FirstParticipantName = r.FirstParticipant == null ? "" : r.FirstParticipant.FirstName,
                SecondParticipantName = r.SecondParticipant == null ? "" : r.SecondParticipant.FirstName,
                Stage = r.Stage
            });
            return bracketModel;
        }

        public Task<BracketModel> GetBracket(Guid bracketId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBracket(BracketModel model)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Participant> GetParticipants()
        {
            var result = new List<Participant>();
            for (int i = 0; i < 8; i++)
            {
                result.Add(new Participant()
                {
                    ParticipantId = Guid.NewGuid(),
                    FirstName = $"Participant {i}"
                });    
            }

            return result;
        }
    }
}
