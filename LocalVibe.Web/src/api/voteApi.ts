import axiosClient from './axiosClient';
import type { VoteRequest, VoteResultResponse } from '../types/vote.types';

export const voteApi = {
  vote: async (postId: number, data: VoteRequest) => {
    return await axiosClient.post<any, VoteResultResponse>(`/posts/${postId}/vote`, data);
  },
};
