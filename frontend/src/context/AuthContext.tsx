import { createContext, useContext, useEffect, useState } from "react";
import { AuthService } from "../api/authApi";
import type { User } from "../types/User";
import type { AuthContextType } from "../types/AuthContextType";

const AuthContext = createContext<AuthContextType>({
  user: null,
  setUser: () => { },
  authChecked: false,
});

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [authChecked, setAuthChecked] = useState(false);

  useEffect(() => {
    const checkAuth = () => {
      Promise.resolve(AuthService.me())
        .then((res) => setUser(res.data as User))
        .finally(() => setAuthChecked(true));
    };

  checkAuth();
}, []);

  return (
    <AuthContext.Provider value={{ user, setUser, authChecked }}>
      {children}
    </AuthContext.Provider>
  );
};