using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace VotingSystem.Contracts.Poll.ContractDefinition
{
    public partial class Option : OptionBase { }

    public class OptionBase 
    {
        [Parameter("string", "option", 1)]
        public virtual string Option { get; set; }
        [Parameter("uint256", "votesCount", 2)]
        public virtual BigInteger VotesCount { get; set; }
    }
}
