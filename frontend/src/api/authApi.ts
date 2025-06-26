import axios from "axios";
import { API_BASE_URL } from "../constants.ts";

export const api = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true,
});

export const AuthService = {
  login: (data: { email: string; password: string, ipAddress:string, deviceType:string }) => api.post("/auth/login", {
      AuthDTO: {
        Email: data.email,
        Password: data.password
      },
      SessionInfo: {
        DeviceType: data.deviceType,
        IpAddress: data.ipAddress
      }
    }),
  register: (data: {email: string; password: string }) => api.post("/user/register", data),
  logout: () => api.post("/auth/logout"),
  refresh: ( data: { ipAddress:string; deviceType:string }) => api.post("/auth/refresh-jwt", {
      DeviceType: data.deviceType,
      IpAddress: data.ipAddress
  }),
  me: () => api.get("/auth/me"),
};