﻿using System;

namespace TRNMNT.Core.Model.Bracket
{
    public class RefreshBracketModel
    {
        public Guid WeightDivisionId { get; set; }
        public BracketModel Bracket { get; set; }
    }
}