import axios from "axios";
import { API_BASE_URL } from "../constants.ts";

const api = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true,
});

api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response) {
      return Promise.reject(error);
    }
    throw new Error("Network error or no response from server");
  }
);

export const AuthService = {
  login: (data: { email: string; password: string, ipAdress:string, deviceType:string }) => api.post("/auth/login", {
      AuthDTO: {
        Email: data.email,
        Password: data.password
      },
      SessionInfo: {
        IpAdress: data.ipAdress,
        DeviceType: data.deviceType
      }
    }),
  register: (data: {email: string; password: string }) => api.post("/user/register", data),
  logout: () => api.post("/auth/logout"),
  refresh: () => api.post("/auth/refresh"),
  me: () => api.get("/auth/me"),
};