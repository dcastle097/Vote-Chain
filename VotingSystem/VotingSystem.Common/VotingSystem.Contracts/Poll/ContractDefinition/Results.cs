using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace VotingSystem.Contracts.Poll.ContractDefinition
{
    public partial class Results : ResultsBase { }

    public class ResultsBase 
    {
        [Parameter("string", "winnerOption", 1)]
        public virtual string WinnerOption { get; set; }
        [Parameter("tuple[]", "resultsByOption", 2)]
        public virtual List<Option> ResultsByOption { get; set; }
        [Parameter("tuple[]", "resultsByDepartment", 3)]
        public virtual List<SectionResults> ResultsByDepartment { get; set; }
        [Parameter("tuple[]", "resultsByLocality", 4)]
        public virtual List<SectionResults> ResultsByLocality { get; set; }
        [Parameter("tuple[]", "resultsByCity", 5)]
        public virtual List<SectionResults> ResultsByCity { get; set; }
    }
}
