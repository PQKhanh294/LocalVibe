import axiosClient from './axiosClient';

export const authApi = {
  login: async (data: any) => {
    return await axiosClient.post('/auth/login', data);
  },
  register: async (data: any) => {
    return await axiosClient.post('/auth/register', data);
  }
};
