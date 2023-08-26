using System;

namespace VotingSystem.Web.Admin.Models.Configuration;

public class AdminUserConfiguration
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}