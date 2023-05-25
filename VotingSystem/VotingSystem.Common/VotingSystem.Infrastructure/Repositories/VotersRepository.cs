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
    public class VotersRepository : IVotersRepository
    {
        private readonly IDynamoDBContext _context;

        public VotersRepository(IOptions<AmazonWebServicesConfiguration> configuration)
        {
            var basicAwsCredentials = new BasicAWSCredentials(configuration.Value.DynamoDb.AccessKey, configuration.Value.DynamoDb.SecretKey);
            var amazonDynamoDbConfig = new AmazonDynamoDBConfig {RegionEndpoint = RegionEndpoint.USEast2};
            var amazonDynamoDbClient = new AmazonDynamoDBClient(basicAwsCredentials, amazonDynamoDbConfig);

            _context = new DynamoDBContext(amazonDynamoDbClient);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Voter>> GetAsync(bool includeInactive = true)
        {
            try
            {
                var table = _context.GetTargetTable<Voter>();
                var config = new ScanOperationConfig {Filter = new ScanFilter()};
                if (!includeInactive)
                {
                    config.Filter.AddCondition("IsActive", ScanOperator.Equal, 1);
                }
                var results = table.Scan(config);
                var data = await results.GetNextSetAsync();
                
                return data.Select(d => new Voter
                {
                    IsActive = d["IsActive"].AsInt() == 1,
                    City = d["City"].AsString() ?? string.Empty,
                    Created = d["Created"].AsDateTime(),
                    Department = d["Department"].AsString() ?? string.Empty,
                    Email = d["Email"].AsString() ?? string.Empty,
                    Id = d["Id"].AsString(),
                    Locality = d["Locality"].AsString() ?? string.Empty,
                    Name = d["Name"].AsString() ?? string.Empty,
                    Table = d["Table"].AsString() ?? string.Empty
                });
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<Voter> GetByIdAsync(string id)
        {
            return await _context.LoadAsync<Voter>(id);
        }

        /// <inheritdoc />
        public async Task SaveAsync(Voter voter)
        {
            await _context.SaveAsync(voter);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(string id, long created)
        {
            try
            {
                await _context.DeleteAsync<Voter>(id, created);
                return true;
            }
            catch (Exception)
            {
                // ignored
            }

            return false;
        }
    }
}