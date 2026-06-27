namespace LocalVibe.API.DTOs.Votes;

public class VoteResultResponse
{
    public int UpVotes { get; set; }
    public int DownVotes { get; set; }
    public int NetScore => UpVotes - DownVotes;
}
