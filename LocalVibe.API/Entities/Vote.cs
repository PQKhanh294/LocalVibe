using LocalVibe.API.Entities.Enums;

namespace LocalVibe.API.Entities;

public class Vote
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public VoteType VoteType { get; set; }
    public string VoterToken { get; set; } = string.Empty;

    // Navigation property
    public Post Post { get; set; } = null!;
}
