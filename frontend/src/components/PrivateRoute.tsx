import { useEffect, useState } from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { getClientIp, getDeviceFingerprint } from "../utils/getClientInfo";
import { AuthService } from "../api/authApi";
import type { User } from "../types/User";

interface PrivateRouteProps {
children: React.ReactNode;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ children }) => {
const { user, setUser, authChecked } = useAuth();
const location = useLocation();
const [refreshing, setRefreshing] = useState(false);

useEffect(() => {
  const tryRefresh = async () => {
    if (!authChecked || user !== null) { 
      console.log("PrivateRoute:", { authChecked, user });
      return;
    }

    setRefreshing(true);
    try {
      const ipAddress = await getClientIp();
      const deviceType = getDeviceFingerprint();
      await AuthService.refresh({ ipAddress, deviceType });
      console.log("Entered try catch");
      const meRes = await AuthService.me();
      setUser(meRes.data as User);
    } catch (err) {
    } finally {
      setRefreshing(false);
    }
  };

  tryRefresh();
}, [authChecked, user, setUser]);

if (!authChecked || refreshing) {
  return <div className="text-white text-center py-10">Checking access...</div>;
}

if (user === null) {
  return <Navigate to="/login" state={{ from: location }} replace />;
}

return <>{children}</>;
};

export default PrivateRoute;
