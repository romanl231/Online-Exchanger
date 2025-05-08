import { useEffect } from "react";
import axios from "axios";
import { AuthService } from "../api/authApi";

export const useAxiosInterceptor = () => {
  useEffect(() => {
    const interceptor = axios.interceptors.response.use(
      (res) => res,
      async (err) => {
        const originalRequest = err.config;

        if (err.response?.status === 401 && !originalRequest._retry) {
          originalRequest._retry = true;

          try {
            await AuthService.refresh();
            return axios(originalRequest);
          } catch {
            window.location.href = "/login";
          }
        }

        return Promise.reject(err);
      }
    );

    return () => {
      axios.interceptors.response.eject(interceptor);
    };
  }, []);
};