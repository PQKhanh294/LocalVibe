import axios from 'axios';

const axiosClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'https://localhost:7011/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Automatically attach JWT Token
axiosClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor
axiosClient.interceptors.response.use(
  (response) => response.data,
  (error) => {
    // Xử lý lỗi cơ bản
    console.error('Lỗi gọi API:', error);
    return Promise.reject(error);
  }
);

export default axiosClient;
