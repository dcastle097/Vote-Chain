using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace VotingSystem.Contracts.Poll.ContractDefinition
{
    public partial class SectionResults : SectionResultsBase { }

    public class SectionResultsBase 
    {
        [Parameter("string", "section", 1)]
        public virtual string Section { get; set; }
        [Parameter("tuple[]", "options", 2)]
        public virtual List<Option> Options { get; set; }
    }
}
