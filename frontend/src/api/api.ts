import axios from "axios";
import { API_BASE_URL } from "../constants.ts";
import { AuthService} from "./authApi.ts";
import { ClientInfoService } from "../services/GetClientInfoService.ts";

export const api = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true,
});

api.interceptors.response.use(
  (res) => res,
  async (err) => {
    const originalRequest = err.config;

    if (originalRequest.url?.includes("/auth/refresh-jwt")) {
      console.error("Refresh itself failed. Redirecting to /login");
      window.location.href = "/login";
      return Promise.reject(err);
    }

    if (err.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        const { 
            ipAddress, 
            deviceType } = await ClientInfoService.getClientData();

        await AuthService.refresh({ ipAddress, deviceType });

        return api(originalRequest);
      } catch (e) {
        window.location.href = "/login";
      }
    }

    return Promise.reject(err);
  }
);