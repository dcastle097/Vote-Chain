using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace VotingSystem.Contracts.Registration.ContractDefinition
{
    public partial class PollDto : PollDtoBase { }

    public class PollDtoBase 
    {
        [Parameter("address", "id", 1)]
        public virtual string Id { get; set; }
        [Parameter("string", "name", 2)]
        public virtual string Name { get; set; }
    }
}
