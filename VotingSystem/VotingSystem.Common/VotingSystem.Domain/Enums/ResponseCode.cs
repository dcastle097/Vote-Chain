namespace VotingSystem.Domain.Enums
{
    public enum ResponseCode
    {
        Ok,
        Created,
        NotFound,
        BadRequest,
        Unauthorized,
        Forbidden,
        InternalServerError,
        Conflict,
        UnprocessableEntity,
        TooManyRequests,
        ServiceUnavailable,
        GatewayTimeout
    }
}