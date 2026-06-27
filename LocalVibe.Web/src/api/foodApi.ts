import axiosClient from './axiosClient';
import type { FoodInfoResponse } from '../types/food.types';

export const foodApi = {
  getByPostId: async (postId: number) => {
    return await axiosClient.get<any, FoodInfoResponse>(`/posts/${postId}/food-info`);
  },
};
