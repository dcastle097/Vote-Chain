using VotingSystem.Domain.Enums;

namespace VotingSystem.Domain.DTOs.Responses
{
    public class GenericResponseDto<T>
    {
        public T Data { get; set; }
        public ResponseCode ResponseCode { get; set; }
        public string Message { get; set; }
    }
}