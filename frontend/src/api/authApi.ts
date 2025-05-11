import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5000/api",
  withCredentials: true,
});

api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response) {
      const status = error.response.status;
      const message = error.response.data?.message || "Unknown error";
      throw new Error(`HTTP ${status}: ${message}`);
    }
    throw new Error("Network error or no response from server");
  }
);

export const AuthService = {
  login: (data: { email: string; password: string }) => api.post("/auth/login", data),
  register: (data: {email: string; password: string }) => api.post("/auth/register", data),
  logout: () => api.post("/auth/logout"),
  refresh: () => api.post("/auth/refresh"),
  me: () => api.get("/auth/me"),
};