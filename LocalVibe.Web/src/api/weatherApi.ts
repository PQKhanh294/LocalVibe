import axiosClient from './axiosClient';
import type { WeatherResponse } from '../types/weather.types';

export const weatherApi = {
  getByPostId: async (postId: number) => {
    return await axiosClient.get<any, WeatherResponse>(`/posts/${postId}/weather`);
  },
};
