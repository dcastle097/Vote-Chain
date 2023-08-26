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
        [Parameter("uint256", "startDate", 3)]
        public virtual BigInteger StartDate { get; set; }
        [Parameter("uint256", "endDate", 4)]
        public virtual BigInteger EndDate { get; set; }
        [Parameter("bool", "voted", 5)]
        public virtual bool Voted { get; set; }
    }
}
