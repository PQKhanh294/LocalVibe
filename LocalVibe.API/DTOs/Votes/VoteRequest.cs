using LocalVibe.API.Entities.Enums;

namespace LocalVibe.API.DTOs.Votes;

public class VoteRequest
{
    public VoteType VoteType { get; set; }
    public string VoterToken { get; set; } = string.Empty;
}
