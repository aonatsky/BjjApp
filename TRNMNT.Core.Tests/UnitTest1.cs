using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRNMNT.Core.Services.impl;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories.Impl;

namespace TRNMNT.Core.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
          
        }

        private IEnumerable<Participant> getParticipants()
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
