// SPDX-License-Identifier: GPL-3.0
pragma solidity ^0.8.4;

/**
 * @title Poll
 * @dev Implements voting process
 */
contract Poll {
    struct Voter {
        string department;
        string city;
        string locality;
        bool canVote; // if true, the voter has been given voting options
        bool voted; // if true, the voter already voted
        uint8 vote; // Index of the voted proposal
    }

    struct Option {
        string option; // Option statement
        uint256 votesCount; // number of votes casted to the option
    }

    struct SectionResults {
        string section;
        Option[] options;
    }

    struct Results {
        string winnerOption;
        Option[] resultsByOption;
        SectionResults[] resultsByDepartment;
        SectionResults[] resultsByLocality;
        SectionResults[] resultsByCity;
    }

    address owner;
    string public title;
    string public statement;
    uint256 public startDate;
    uint256 public endDate;
    mapping(address => Voter) voters;
    Option[] options;
    mapping(string => mapping(uint256 => uint256)) votesByDepartment;
    mapping(string => mapping(uint256 => uint256)) votesByCity;
    mapping(string => mapping(uint256 => uint256)) votesByLocality;

    mapping(string => bool) existingDepartments;
    string[] departments;
    mapping(string => bool) existingCities;
    string[] cities;
    mapping(string => bool) existingLocalities;
    string[] localities;

    /**
     * @dev Create a new poll to vote for one of the options
     * @param _title title of the poll
     * @param _statement extended statement of the poll
     * @param _options options the voters will pick from
     * @param _startDate poll start date in unix timestamp milliseconds
     * @param _endDate poll end date in unix timestamp milliseconds
     */
    constructor(
        string memory _title,
        string memory _statement,
        string[] memory _options,
        uint256 _startDate,
        uint256 _endDate
    ) {
        owner = msg.sender;
        startDate = _startDate;
        endDate = _endDate;
        title = _title;
        statement = _statement;
        for (uint256 i = 0; i < _options.length; i++) {
            options.push(Option({option : _options[i], votesCount : 0}));
        }
    }

    /**
     * @dev Enable a voter to cast a vote
     * @param voter voter's account address
     * @param department where the voter's voting centre is located
     * @param city where the voter's voting centre is located
     * @param locality where the voter's voting centre is located
     */
    function giveRightToVote(
        address voter,
        string memory department,
        string memory city,
        string memory locality
    ) public {
        require(
            msg.sender == owner,
            "Only the contract owner can give right to vote."
        );
        require(!voters[voter].voted, "The voter already voted.");
        require(voters[voter].canVote == false);
        voters[voter].department = department;
        voters[voter].city = city;
        voters[voter].locality = locality;
        voters[voter].canVote = true;
    }

    /**
     * @dev Cast a vote in the name of the message sender for an option
     * @param option index of the option
     */
    function vote(uint8 option) public {
        uint256 timestamp = getTimestamp();
        require(timestamp > startDate, string(abi.encodePacked("Voting hasn't started yet. Timestamp: ", uintToStr(timestamp))));
        require(timestamp < endDate, string(abi.encodePacked("Voting has ended. Timestamp: ", uintToStr(timestamp))));
        require(
            option > 0 && option < options.length,
            "The option isn't valid"
        );
        Voter storage sender = voters[msg.sender];
        require(sender.canVote, "Has no right to vote.");
        require(!sender.voted, "Already voted.");

        sender.voted = true;
        sender.vote = option;

        if (!existingDepartments[sender.department]) {
            existingDepartments[sender.department] = true;
            departments.push(sender.department);
        }
        if (!existingCities[sender.city]) {
            existingCities[sender.city] = true;
            cities.push(sender.city);
        }
        if (!existingLocalities[sender.locality]) {
            existingLocalities[sender.locality] = true;
            localities.push(sender.locality);
        }

        options[option].votesCount += 1;
        votesByDepartment[sender.department][option] += 1;
        votesByCity[sender.city][option] += 1;
        votesByLocality[sender.locality][option] += 1;
    }

    /**
     * @dev Transform option array to string array
     * @return _options with matching index
     */
    function getOptions() public view returns (string[] memory) {
        string[] memory _options = new string[](options.length);
        for (uint256 i = 0; i < options.length; i++) {
            _options[i] = options[i].option;
        }
        return _options;
    }

    /**
     * @dev Computes the winning proposal and information grouped by sections
     * @return _results results of the voting process
     */
    function getResults() public view returns (Results memory _results) {
        require(
            getTimestamp() > endDate,
            "The poll is still open, results will be available when it closes"
        );
        uint256 winnerVotesCount = 0;
        uint256 winnerProposal = 0;
        for (uint256 p = 0; p < options.length; p++) {
            if (options[p].votesCount > winnerVotesCount) {
                winnerVotesCount = options[p].votesCount;
                winnerProposal = p;
            }
        }

        _results.winnerOption = options[winnerProposal].option;
        _results.resultsByOption = options;

        SectionResults[] memory _votesByDepartment =
        new SectionResults[](departments.length);
        for (uint256 i = 0; i < departments.length; i++) {
            Option[] memory _options = new Option[](options.length);
            for (uint256 j = 0; j < options.length; j++) {
                _options[j] = Option({
                option : options[j].option,
                votesCount : votesByDepartment[departments[i]][j]
                });
            }
            _votesByDepartment[i] = SectionResults({
            section : departments[i],
            options : _options
            });
        }
        _results.resultsByDepartment = _votesByDepartment;

        SectionResults[] memory _votesByCity =
        new SectionResults[](cities.length);
        for (uint256 i = 0; i < cities.length; i++) {
            Option[] memory _options = new Option[](options.length);
            for (uint256 j = 0; j < options.length; j++) {
                _options[j] = Option({
                option : options[j].option,
                votesCount : votesByCity[cities[i]][j]
                });
            }
            _votesByCity[i] = SectionResults({
            section : cities[i],
            options : _options
            });
        }
        _results.resultsByCity = _votesByCity;

        SectionResults[] memory _votesByLocality =
        new SectionResults[](localities.length);
        for (uint256 i = 0; i < localities.length; i++) {
            Option[] memory _options = new Option[](options.length);
            for (uint256 j = 0; j < options.length; j++) {
                _options[j] = Option({
                option : options[j].option,
                votesCount : votesByLocality[localities[i]][j]
                });
            }
            _votesByCity[i] = SectionResults({
            section : localities[i],
            options : _options
            });
        }
        _results.resultsByLocality = _votesByLocality;
    }

    /**
     * @dev compute the block unix timestamp in milliseconds
     */
    function getTimestamp() private view returns (uint256) {
        return block.timestamp * 1000;
    }

    function uintToStr(uint _i) internal pure returns (string memory _uintAsString) {
        uint number = _i;
        if (number == 0) {
            return "0";
        }
        uint j = number;
        uint len;
        while (j != 0) {
            len++;
            j /= 10;
        }
        bytes memory bstr = new bytes(len);
        uint k = len - 1;
        while (number != 0) {
            bstr[k--] = byte(uint8(48 + number % 10));
            number /= 10;
        }
        return string(bstr);
    }
}
