using System;
using System.Collections.Generic;
using System.Linq;
using TRNMNT.Core.Data;
using TRNMNT.Core.Data.Entities;

namespace TRNMNT.Core.Services.impl
{
    public class BracketsService: IBracketsService
    {
        private IAppDbContext _context;
        public BracketsService(IAppDbContext context) 
        {
            _context = context;
        }

public void Test()
{
    int participantsCount = 6;
    var fightList = GetFights(GetBracketSize(participantsCount));
}


#region private methods
        private int GetBracketSize(int participantsCount)
        {
            int initialSize = 2;
            for (int i = 1; i < 10; i++)
            {
                var size = Math.Pow(initialSize,i);
                if(size - participantsCount >= 0)
                {
                    return (Int32)size;
                } 
            }
            return initialSize;
        }

private IEnumerable<Fight> GetFights(int bracketSize)
{
    var resultFights = new List<Fight>(){ new Fight(1,null) };
    var levels = Math.Log(bracketSize,2);
        
    for (int i = 2; i <= levels; i++)
    {
        resultFights.AddRange(GetFightsForLevel(i, resultFights));
    }
    return resultFights;
}

private IEnumerable<Fight> GetFightsForLevel(int level, IEnumerable<Fight> parentLevelFights){
    var result = new List<Fight>();
    foreach (var f in parentLevelFights.Where(pf => pf.Level == level-1))
    {
        result.Add(new Fight(level,f.FightID));
        result.Add(new Fight(level,f.FightID));
    }
    return result;
}

// private IEnumerable<Fight> GetSubFights(int level, IEnumerable<Fight> parentLevelFights){

// }



#endregion


    }
}
