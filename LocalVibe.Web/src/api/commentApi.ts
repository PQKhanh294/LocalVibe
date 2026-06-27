import axiosClient from './axiosClient';
import type { CommentResponse, CreateCommentRequest } from '../types/comment.types';

export const commentApi = {
  addComment: async (postId: number, data: CreateCommentRequest) => {
    return await axiosClient.post<any, CommentResponse>(`/posts/${postId}/comments`, data);
  },
};
