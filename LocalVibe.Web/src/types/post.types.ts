import type { CommentResponse } from './comment.types';

export interface PostSummaryResponse {
  id: number;
  title: string;
  tag: string;
  imageUrl: string | null;
  createdAt: string;
  upVotes: number;
  downVotes: number;
  netScore: number;
  commentCount: number;
}

export interface PagedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface PostDetailResponse {
  id: number;
  title: string;
  description: string;
  tag: string;
  imageUrl: string | null;
  latitude: number;
  longitude: number;
  address?: string;
  additionalImages?: string[];
  createdAt: string;
  upVotes: number;
  downVotes: number;
  netScore: number;
  commentCount: number;
  comments: CommentResponse[];
}
