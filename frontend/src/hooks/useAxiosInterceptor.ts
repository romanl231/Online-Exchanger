import { useEffect } from "react";
import { getClientIp, getDeviceFingerprint } from "../utils/getClientInfo";
import { api, AuthService } from "../api/authApi";

export const useAxiosInterceptor = () => {
  useEffect(() => {
    const interceptor = api.interceptors.response.use(
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
            const ipAddress = await getClientIp();
            const deviceType = getDeviceFingerprint();
            await AuthService.refresh({ ipAddress, deviceType });

            return api(originalRequest);
          } catch (e) {
            console.error("Refresh failed", e);
            window.location.href = "/login";
          }
        }

        return Promise.reject(err);
      }
    );

    return () => {
      api.interceptors.response.eject(interceptor);
    };
  }, []);
};