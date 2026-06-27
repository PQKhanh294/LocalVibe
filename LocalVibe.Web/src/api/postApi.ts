import axiosClient from './axiosClient';
import type { PagedResponse, PostDetailResponse, PostSummaryResponse } from '../types/post.types';

export const postApi = {
  getPosts: async (params?: { tag?: string; sort?: string; page?: number; pageSize?: number }) => {
    // AxiosClient interceptor đã map response.data ra ngoài
    return await axiosClient.get<any, PagedResponse<PostSummaryResponse>>('/posts', { params });
  },
  getPostById: async (id: number) => {
    return await axiosClient.get<any, PostDetailResponse>(`/posts/${id}`);
  },
};
