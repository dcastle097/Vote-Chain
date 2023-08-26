using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Microsoft.Extensions.Options;
using VotingSystem.Infrastructure.Models.Configuration;
using VotingSystem.Infrastructure.Repositories.Interfaces;
using VotingSystem.Infrastructure.Repositories.Models;

namespace VotingSystem.Infrastructure.Repositories
{
    public class PollsRepository : IPollsRepository
    {
        private readonly DynamoDBContext _context;

        public PollsRepository(IOptions<AmazonWebServicesConfiguration> configuration)
        {
            var basicAwsCredentials = new BasicAWSCredentials(configuration.Value.DynamoDb.AccessKey,
                configuration.Value.DynamoDb.SecretKey);
            var amazonDynamoDbConfig = new AmazonDynamoDBConfig { RegionEndpoint = RegionEndpoint.USEast2 };
            var amazonDynamoDbClient = new AmazonDynamoDBClient(basicAwsCredentials, amazonDynamoDbConfig);

            _context = new DynamoDBContext(amazonDynamoDbClient);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Poll>> GetAsync()
        {
            try
            {
                var table = _context.GetTargetTable<Poll>();
                var results = table.Scan(new ScanOperationConfig());
                var data = await results.GetNextSetAsync();
                return data.Select(d => new Poll
                {
                    Created = d["Created"].AsDateTime(),
                    Id = d["Id"].AsString() ?? string.Empty,
                    Title = d["Title"] ?? string.Empty,
                    StartDate = d["StartDate"].AsDateTime().ToUniversalTime(),
                    EndDate = d["EndDate"].AsDateTime().ToUniversalTime(),
                    Statement = d["Statement"].AsString() ?? string.Empty,
                    Options = d.Keys.Contains("Options") ? d["Options"]?.AsListOfString() : new List<string>()
                });
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<Poll> GetByIdAsync(string id)
        {
            var poll = await _context.LoadAsync<Poll>(id);
            return new Poll
            {
                Id = poll.Id,
                Title = poll.Title,
                Statement = poll.Statement,
                StartDate = poll.StartDate.ToUniversalTime(),
                EndDate = poll.EndDate.ToUniversalTime(),
                Options = poll.Options
            };
        }

        /// <inheritdoc />
        public async Task AddAsync(Poll poll)
        {
            await _context.SaveAsync(poll);
        }

        /// <inheritdoc />
        public async Task AddVoterAsync(string pollId, string voterId)
        {
            var poll = await GetByIdAsync(pollId);
            poll.Voters ??= new List<string>();
            poll.Voters.Add(voterId);
            await _context.SaveAsync(poll);
        }
    }
}