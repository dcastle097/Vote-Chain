using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace VotingSystem.Contracts.Poll.ContractDefinition
{


    public partial class PollDeployment : PollDeploymentBase
    {
        public PollDeployment() : base(BYTECODE) { }
        public PollDeployment(string byteCode) : base(byteCode) { }
    }

    public class PollDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60806040523480156200001157600080fd5b5060405162001f6638038062001f6683398101604081905262000034916200029f565b600080546001600160a01b03191633179055600382905560048190558451620000659060019060208801906200011b565b5083516200007b9060029060208701906200011b565b5060005b83518110156200010f5760066040518060400160405280868481518110620000ab57620000ab620003cd565b6020908102919091018101518252600091810182905283546001810185559382529081902082518051939460020290910192620000ec92849201906200011b565b5060208201518160010155505080806200010690620003e3565b9150506200007f565b50505050505062000447565b82805462000129906200040b565b90600052602060002090601f0160209004810192826200014d576000855562000198565b82601f106200016857805160ff191683800117855562000198565b8280016001018555821562000198579182015b82811115620001985782518255916020019190600101906200017b565b50620001a6929150620001aa565b5090565b5b80821115620001a65760008155600101620001ab565b634e487b7160e01b600052604160045260246000fd5b604051601f8201601f191681016001600160401b0381118282101715620002025762000202620001c1565b604052919050565b600082601f8301126200021c57600080fd5b81516001600160401b03811115620002385762000238620001c1565b60206200024e601f8301601f19168201620001d7565b82815285828487010111156200026357600080fd5b60005b838110156200028357858101830151828201840152820162000266565b83811115620002955760008385840101525b5095945050505050565b600080600080600060a08688031215620002b857600080fd5b85516001600160401b0380821115620002d057600080fd5b620002de89838a016200020a565b9650602091508188015181811115620002f657600080fd5b620003048a828b016200020a565b9650506040880151818111156200031a57600080fd5b8801601f81018a136200032c57600080fd5b805182811115620003415762000341620001c1565b8060051b62000352858201620001d7565b918252828101850191858101908d8411156200036d57600080fd5b86850192505b83831015620003ae578251868111156200038d5760008081fd5b6200039d8f89838901016200020a565b835250918601919086019062000373565b60608d01516080909d01519b9e9a9d509b9a9998505050505050505050565b634e487b7160e01b600052603260045260246000fd5b6000600182016200040457634e487b7160e01b600052601160045260246000fd5b5060010190565b600181811c908216806200042057607f821691505b6020821081036200044157634e487b7160e01b600052602260045260246000fd5b50919050565b611b0f80620004576000396000f3fe608060405234801561001057600080fd5b50600436106100885760003560e01c8063c24a0f8b1161005b578063c24a0f8b146100e8578063cc2ee196146100f1578063e42cb9f314610106578063ec61623b1461010e57600080fd5b80630b97bc861461008d5780634717f97c146100a95780634a79d50c146100be578063b3f98adc146100d3575b600080fd5b61009660035481565b6040519081526020015b60405180910390f35b6100b1610121565b6040516100a09190611727565b6100c6610c47565b6040516100a091906117a5565b6100e66100e13660046117bf565b610cd5565b005b61009660045481565b6100f96111c9565b6040516100a091906117e2565b6100c6611310565b6100e661011c3660046118e7565b61131d565b61014c6040518060800160405280606081526020016060815260200160608152602001606081525090565b6004546101576114df565b116101d1576040805162461bcd60e51b81526020600482015260248101919091527f54686520706f6c6c206973207374696c6c206f70656e2c20726573756c74732060448201527f77696c6c20626520617661696c61626c65207768656e20697420636c6f73657360648201526084015b60405180910390fd5b60008060005b60065481101561024a5782600682815481106101f5576101f561198e565b906000526020600020906002020160010154111561023857600681815481106102205761022061198e565b90600052602060002090600202016001015492508091505b80610242816119ba565b9150506101d7565b506006818154811061025e5761025e61198e565b9060005260206000209060020201600001805461027a906119d3565b80601f01602080910402602001604051908101604052809291908181526020018280546102a6906119d3565b80156102f35780601f106102c8576101008083540402835291602001916102f3565b820191906000526020600020905b8154815290600101906020018083116102d657829003601f168201915b5050509185525050600b5460009067ffffffffffffffff81111561031957610319611844565b60405190808252806020026020018201604052801561035e57816020015b60408051808201909152606080825260208201528152602001906001900390816103375790505b50905060005b600b5481101561060a5760065460009067ffffffffffffffff81111561038c5761038c611844565b6040519080825280602002602001820160405280156103d257816020015b6040805180820190915260608152600060208201528152602001906001900390816103aa5790505b50905060005b60065481101561051e576040518060400160405280600683815481106104005761040061198e565b9060005260206000209060020201600001805461041c906119d3565b80601f0160208091040260200160405190810160405280929190818152602001828054610448906119d3565b80156104955780601f1061046a57610100808354040283529160200191610495565b820191906000526020600020905b81548152906001019060200180831161047857829003601f168201915b505050505081526020016007600b86815481106104b4576104b461198e565b906000526020600020016040516104cb9190611a07565b90815260200160405180910390206000848152602001908152602001600020548152508282815181106105005761050061198e565b60200260200101819052508080610516906119ba565b9150506103d8565b506040518060400160405280600b848154811061053d5761053d61198e565b906000526020600020018054610552906119d3565b80601f016020809104026020016040519081016040528092919081815260200182805461057e906119d3565b80156105cb5780601f106105a0576101008083540402835291602001916105cb565b820191906000526020600020905b8154815290600101906020018083116105ae57829003601f168201915b50505050508152602001828152508383815181106105eb576105eb61198e565b6020026020010181905250508080610602906119ba565b915050610364565b5060208401819052600d5460009067ffffffffffffffff81111561063057610630611844565b60405190808252806020026020018201604052801561067557816020015b604080518082019091526060808252602082015281526020019060019003908161064e5790505b50905060005b600d548110156109215760065460009067ffffffffffffffff8111156106a3576106a3611844565b6040519080825280602002602001820160405280156106e957816020015b6040805180820190915260608152600060208201528152602001906001900390816106c15790505b50905060005b600654811015610835576040518060400160405280600683815481106107175761071761198e565b90600052602060002090600202016000018054610733906119d3565b80601f016020809104026020016040519081016040528092919081815260200182805461075f906119d3565b80156107ac5780601f10610781576101008083540402835291602001916107ac565b820191906000526020600020905b81548152906001019060200180831161078f57829003601f168201915b505050505081526020016008600d86815481106107cb576107cb61198e565b906000526020600020016040516107e29190611a07565b90815260200160405180910390206000848152602001908152602001600020548152508282815181106108175761081761198e565b6020026020010181905250808061082d906119ba565b9150506106ef565b506040518060400160405280600d84815481106108545761085461198e565b906000526020600020018054610869906119d3565b80601f0160208091040260200160405190810160405280929190818152602001828054610895906119d3565b80156108e25780601f106108b7576101008083540402835291602001916108e2565b820191906000526020600020905b8154815290600101906020018083116108c557829003601f168201915b50505050508152602001828152508383815181106109025761090261198e565b6020026020010181905250508080610919906119ba565b91505061067b565b5060608501819052600f5460009067ffffffffffffffff81111561094757610947611844565b60405190808252806020026020018201604052801561098c57816020015b60408051808201909152606080825260208201528152602001906001900390816109655790505b50905060005b600f54811015610c385760065460009067ffffffffffffffff8111156109ba576109ba611844565b604051908082528060200260200182016040528015610a0057816020015b6040805180820190915260608152600060208201528152602001906001900390816109d85790505b50905060005b600654811015610b4c57604051806040016040528060068381548110610a2e57610a2e61198e565b90600052602060002090600202016000018054610a4a906119d3565b80601f0160208091040260200160405190810160405280929190818152602001828054610a76906119d3565b8015610ac35780601f10610a9857610100808354040283529160200191610ac3565b820191906000526020600020905b815481529060010190602001808311610aa657829003601f168201915b505050505081526020016009600f8681548110610ae257610ae261198e565b90600052602060002001604051610af99190611a07565b9081526020016040518091039020600084815260200190815260200160002054815250828281518110610b2e57610b2e61198e565b60200260200101819052508080610b44906119ba565b915050610a06565b506040518060400160405280600f8481548110610b6b57610b6b61198e565b906000526020600020018054610b80906119d3565b80601f0160208091040260200160405190810160405280929190818152602001828054610bac906119d3565b8015610bf95780601f10610bce57610100808354040283529160200191610bf9565b820191906000526020600020905b815481529060010190602001808311610bdc57829003601f168201915b5050505050815260200182815250848381518110610c1957610c1961198e565b6020026020010181905250508080610c30906119ba565b915050610992565b50604086015250929392505050565b60018054610c54906119d3565b80601f0160208091040260200160405190810160405280929190818152602001828054610c80906119d3565b8015610ccd5780601f10610ca257610100808354040283529160200191610ccd565b820191906000526020600020905b815481529060010190602001808311610cb057829003601f168201915b505050505081565b600354610ce06114df565b11610d2d5760405162461bcd60e51b815260206004820152601960248201527f566f74696e67206861736e27742073746172746564207965740000000000000060448201526064016101c8565b600454610d386114df565b10610d785760405162461bcd60e51b815260206004820152601060248201526f159bdd1a5b99c81a185cc8195b99195960821b60448201526064016101c8565b60008160ff16118015610d8f575060065460ff8216105b610dd45760405162461bcd60e51b8152602060048201526016602482015275151a19481bdc1d1a5bdb881a5cdb89dd081d985b1a5960521b60448201526064016101c8565b336000908152600560205260409020600381015460ff16610e2f5760405162461bcd60e51b81526020600482015260156024820152742430b9903737903934b3b43a103a37903b37ba329760591b60448201526064016101c8565b6003810154610100900460ff1615610e7a5760405162461bcd60e51b815260206004820152600e60248201526d20b63932b0b23c903b37ba32b21760911b60448201526064016101c8565b60038101805460ff8416620100000262ffff001990911617610100179055604051600a90610ea9908390611a07565b9081526040519081900360200190205460ff16610f44576001600a82600001604051610ed59190611a07565b908152604051908190036020019020805491151560ff19909216919091179055600b805460018101825560009190915281547f0175b7a638427703f0dbe7bb9bbf987a2551717b34e79f33b5b1008d1fa01db9909101908290610f37906119d3565b610f429291906114f2565b505b600c81600101604051610f579190611a07565b9081526040519081900360200190205460ff16610ff4576001600c82600101604051610f839190611a07565b908152604051908190036020019020805491151560ff19909216919091179055600d80546001818101835560009290925290820180547fd7b6990105719101dabeb77144f2a3385c8033acd3af97e9423a695e81ad1eb590920191610fe7906119d3565b610ff29291906114f2565b505b600e816002016040516110079190611a07565b9081526040519081900360200190205460ff166110a4576001600e826002016040516110339190611a07565b908152604051908190036020019020805491151560ff19909216919091179055600f80546001810182556000919091526002820180547f8d1108e10bcb7c27dddfc02ed9d693a074039d026cf4ea4240b40f7d581ac80290920191611097906119d3565b6110a29291906114f2565b505b600160068360ff16815481106110bc576110bc61198e565b906000526020600020906002020160010160008282546110dc9190611aa2565b90915550506040516001906007906110f5908490611a07565b908152602001604051809103902060008460ff16815260200190815260200160002060008282546111269190611aa2565b9250508190555060016008826001016040516111429190611a07565b908152602001604051809103902060008460ff16815260200190815260200160002060008282546111739190611aa2565b92505081905550600160098260020160405161118f9190611a07565b908152602001604051809103902060008460ff16815260200190815260200160002060008282546111c09190611aa2565b90915550505050565b60065460609060009067ffffffffffffffff8111156111ea576111ea611844565b60405190808252806020026020018201604052801561121d57816020015b60608152602001906001900390816112085790505b50905060005b60065481101561130a57600681815481106112405761124061198e565b9060005260206000209060020201600001805461125c906119d3565b80601f0160208091040260200160405190810160405280929190818152602001828054611288906119d3565b80156112d55780601f106112aa576101008083540402835291602001916112d5565b820191906000526020600020905b8154815290600101906020018083116112b857829003601f168201915b50505050508282815181106112ec576112ec61198e565b60200260200101819052508080611302906119ba565b915050611223565b50919050565b60028054610c54906119d3565b6000546001600160a01b0316331461138f5760405162461bcd60e51b815260206004820152602f60248201527f4f6e6c792074686520636f6e7472616374206f776e65722063616e206769766560448201526e103934b3b43a103a37903b37ba329760891b60648201526084016101c8565b6001600160a01b038416600090815260056020526040902060030154610100900460ff16156114005760405162461bcd60e51b815260206004820152601860248201527f54686520766f74657220616c726561647920766f7465642e000000000000000060448201526064016101c8565b6001600160a01b03841660009081526005602052604090206003015460ff161561142957600080fd5b6001600160a01b038416600090815260056020908152604090912084516114529286019061157d565b506001600160a01b038416600090815260056020908152604090912083516114829260019092019185019061157d565b506001600160a01b038416600090815260056020908152604090912082516114b29260029092019184019061157d565b5050506001600160a01b039091166000908152600560205260409020600301805460ff1916600117905550565b60006114ed426103e8611aba565b905090565b8280546114fe906119d3565b90600052602060002090601f016020900481019282611520576000855561156d565b82601f10611531578054855561156d565b8280016001018555821561156d57600052602060002091601f016020900482015b8281111561156d578254825591600101919060010190611552565b506115799291506115f1565b5090565b828054611589906119d3565b90600052602060002090601f0160209004810192826115ab576000855561156d565b82601f106115c457805160ff191683800117855561156d565b8280016001018555821561156d579182015b8281111561156d5782518255916020019190600101906115d6565b5b8082111561157957600081556001016115f2565b6000815180845260005b8181101561162c57602081850181015186830182015201611610565b8181111561163e576000602083870101525b50601f01601f19169290920160200192915050565b600082825180855260208086019550808260051b8401018186016000805b8581101561171957601f1980888603018b5283516040815181885261169882890182611606565b92890151888403898b01528051808552908a0193915089820190600581901b83018b01885b828110156116fc5787858303018452865180518784526116df88850182611606565b918f0151938f0193909352968d0196938d019391506001016116bd565b509f8b019f99505050958801955050506001919091019050611671565b509198975050505050505050565b60208152600082516080602084015261174360a0840182611606565b90506020840151601f19808584030160408601526117618383611653565b9250604086015191508085840301606086015261177e8383611653565b925060608601519150808584030160808601525061179c8282611653565b95945050505050565b6020815260006117b86020830184611606565b9392505050565b6000602082840312156117d157600080fd5b813560ff811681146117b857600080fd5b6000602080830181845280855180835260408601915060408160051b870101925083870160005b8281101561183757603f19888603018452611825858351611606565b94509285019290850190600101611809565b5092979650505050505050565b634e487b7160e01b600052604160045260246000fd5b600082601f83011261186b57600080fd5b813567ffffffffffffffff8082111561188657611886611844565b604051601f8301601f19908116603f011681019082821181831017156118ae576118ae611844565b816040528381528660208588010111156118c757600080fd5b836020870160208301376000602085830101528094505050505092915050565b600080600080608085870312156118fd57600080fd5b84356001600160a01b038116811461191457600080fd5b9350602085013567ffffffffffffffff8082111561193157600080fd5b61193d8883890161185a565b9450604087013591508082111561195357600080fd5b61195f8883890161185a565b9350606087013591508082111561197557600080fd5b506119828782880161185a565b91505092959194509250565b634e487b7160e01b600052603260045260246000fd5b634e487b7160e01b600052601160045260246000fd5b6000600182016119cc576119cc6119a4565b5060010190565b600181811c908216806119e757607f821691505b60208210810361130a57634e487b7160e01b600052602260045260246000fd5b600080835481600182811c915080831680611a2357607f831692505b60208084108203611a4257634e487b7160e01b86526022600452602486fd5b818015611a565760018114611a6757611a94565b60ff19861689528489019650611a94565b60008a81526020902060005b86811015611a8c5781548b820152908501908301611a73565b505084890196505b509498975050505050505050565b60008219821115611ab557611ab56119a4565b500190565b6000816000190483118215151615611ad457611ad46119a4565b50029056fea2646970667358221220e75ba6f91c3674d00832e135ce773f53d856da359fd8c33eec5004777e018c1d64736f6c634300080d0033";
        public PollDeploymentBase() : base(BYTECODE) { }
        public PollDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("string", "_title", 1)]
        public virtual string Title { get; set; }
        [Parameter("string", "_statement", 2)]
        public virtual string Statement { get; set; }
        [Parameter("string[]", "_options", 3)]
        public virtual List<string> Options { get; set; }
        [Parameter("uint256", "_startDate", 4)]
        public virtual BigInteger StartDate { get; set; }
        [Parameter("uint256", "_endDate", 5)]
        public virtual BigInteger EndDate { get; set; }
    }

    public partial class EndDateFunction : EndDateFunctionBase { }

    [Function("endDate", "uint256")]
    public class EndDateFunctionBase : FunctionMessage
    {

    }

    public partial class GetOptionsFunction : GetOptionsFunctionBase { }

    [Function("getOptions", "string[]")]
    public class GetOptionsFunctionBase : FunctionMessage
    {

    }

    public partial class GetResultsFunction : GetResultsFunctionBase { }

    [Function("getResults", typeof(GetResultsOutputDTO))]
    public class GetResultsFunctionBase : FunctionMessage
    {

    }

    public partial class GiveRightToVoteFunction : GiveRightToVoteFunctionBase { }

    [Function("giveRightToVote")]
    public class GiveRightToVoteFunctionBase : FunctionMessage
    {
        [Parameter("address", "voter", 1)]
        public virtual string Voter { get; set; }
        [Parameter("string", "department", 2)]
        public virtual string Department { get; set; }
        [Parameter("string", "city", 3)]
        public virtual string City { get; set; }
        [Parameter("string", "locality", 4)]
        public virtual string Locality { get; set; }
    }

    public partial class StartDateFunction : StartDateFunctionBase { }

    [Function("startDate", "uint256")]
    public class StartDateFunctionBase : FunctionMessage
    {

    }

    public partial class StatementFunction : StatementFunctionBase { }

    [Function("statement", "string")]
    public class StatementFunctionBase : FunctionMessage
    {

    }

    public partial class TitleFunction : TitleFunctionBase { }

    [Function("title", "string")]
    public class TitleFunctionBase : FunctionMessage
    {

    }

    public partial class VoteFunction : VoteFunctionBase { }

    [Function("vote")]
    public class VoteFunctionBase : FunctionMessage
    {
        [Parameter("uint8", "option", 1)]
        public virtual byte Option { get; set; }
    }

    public partial class EndDateOutputDTO : EndDateOutputDTOBase { }

    [FunctionOutput]
    public class EndDateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetOptionsOutputDTO : GetOptionsOutputDTOBase { }

    [FunctionOutput]
    public class GetOptionsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string[]", "", 1)]
        public virtual List<string> ReturnValue1 { get; set; }
    }

    public partial class GetResultsOutputDTO : GetResultsOutputDTOBase { }

    [FunctionOutput]
    public class GetResultsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("tuple", "_results", 1)]
        public virtual Results Results { get; set; }
    }



    public partial class StartDateOutputDTO : StartDateOutputDTOBase { }

    [FunctionOutput]
    public class StartDateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class StatementOutputDTO : StatementOutputDTOBase { }

    [FunctionOutput]
    public class StatementOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TitleOutputDTO : TitleOutputDTOBase { }

    [FunctionOutput]
    public class TitleOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }


}