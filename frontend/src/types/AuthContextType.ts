import type { User } from "./User";

export interface AuthContextType {
  user: User | null;
  setUser: (user: User | null) => void;
  authChecked: boolean;
}