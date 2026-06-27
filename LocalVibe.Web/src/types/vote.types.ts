export interface VoteRequest {
  voteType: 'Up' | 'Down';
  voterToken: string;
}

export interface VoteResultResponse {
  upVotes: number;
  downVotes: number;
  netScore: number;
}
