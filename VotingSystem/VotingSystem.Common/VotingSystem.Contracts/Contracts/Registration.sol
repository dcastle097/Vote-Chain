// SPDX-License-Identifier: GPL-3.0
pragma solidity ^0.8.4;

/**
 * @title Registration
 * @dev This acts as a registration center, users are created and polls are assigned to them
 */
contract Registration {
    struct Voter {
        string name; // full name
        string email;
        string department;
        string city;
        string locality;
        address[] polls; // list of assocciated poll addresses
    }

    struct Poll {
        string title;
        uint256 startDate;
        uint256 endDate;
    }

    // Object used to return a user's associated polls
    struct PollDto {
        address id;
        string name;
    }

    address owner;
    address[] pollAddresses;
    mapping(address => Poll) public polls;
    mapping(address => Voter) public voters;
    mapping(address => mapping(address => bool)) voterPolls;
    mapping(address => mapping(address => bool)) voterVoted;

    /**
     * @dev Create a new Registration entity.
     */
    constructor() {
        owner = msg.sender;
    }

    /**
     * @dev Add a new poll to the list
     * @param pollAddress poll's contract address
     * @param title poll's title for easy identification
     * @param _startDate poll start date in unix timestamp milliseconds
     * @param _endDate poll end date in unix timestamp milliseconds
     */
    function addPoll(
        address pollAddress,
        string memory title,
        uint256 _startDate,
        uint256 _endDate
    ) public {
        require(
            msg.sender == owner,
            "Only the contract owner can create new polls."
        );
        Poll storage poll = polls[pollAddress];
        poll.title = title;
        poll.startDate = _startDate;
        poll.endDate = _endDate;
        pollAddresses.push(pollAddress);
    }

    /**
     * @dev Add a voter to the list
     * @param name Voter's full name
     * @param email Voter's email address for communication
     * @param department Department where the voter's voting centre is located
     * @param city City where the voter's voting centre is located
     * @param locality Locality where the voter's voting centre is located
     */
    function addVoter(
        address voterAddress,
        string memory name,
        string memory email,
        string memory department,
        string memory city,
        string memory locality
    ) public {
        require(
            msg.sender == owner,
            "Only the contract owner can execute this function."
        );
        Voter storage voter = voters[voterAddress];
        voter.name = name;
        voter.email = email;
        voter.department = department;
        voter.city = city;
        voter.locality = locality;
    }

    /**
     * @dev Assign a poll to a voter
     * @param voterAddress Voter's address identification
     * @param pollAddress Poll's contract address to assign to the voter
     */
    function addPollToVoter(address voterAddress, address pollAddress) public {
        require(
            msg.sender == owner,
            "Only the contract owner can execute this function."
        );
        require(
            voterPolls[voterAddress][pollAddress] == false,
            "Voter already has permission"
        );
        voterPolls[voterAddress][pollAddress] = true;
        voters[voterAddress].polls.push(pollAddress);
    }

    /**
     * @dev Mark a poll as voted by a voter
     * @param voterAddress Voter's address identification
     * @param pollAddress Poll's contract address to mark as voted
     */
    function markPollAsVoted(address voterAddress, address pollAddress) public {
        require(
            msg.sender == owner,
            "Only the contract owner can execute this function."
        );
        require(
            voterVoted[voterAddress][pollAddress] == false,
            "Voter already voted"
        );
        voterVoted[voterAddress][pollAddress] = true;
    }

    /**
     * @dev Filters the polls assigned to a user
     * @return A list of PollDtos
     */
    function getVoterPolls(address voter)
        public
        view
        returns (PollDto[] memory)
    {
        require(
            msg.sender == owner,
            "Only the contract owner can execute this function."
        );
        PollDto[] memory _polls = new PollDto[](voters[voter].polls.length);
        uint256 j = 0;
        for (uint256 i = 0; i < voters[voter].polls.length; i++) {
            _polls[j] = PollDto({
                id: voters[voter].polls[i],
                name: polls[voters[voter].polls[i]].title
            });
            j += 1;
        }
        return _polls;
    }
}
