import React from "react";
import { AuthService } from "../../api/authApi";
import { useAuth } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";

const SignInButton = ({ email, password }: { email: string; password: string }) => {
  const { setUser } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
      await AuthService.login({ email, password });
      const me = await AuthService.me();
      setUser(me.data);
      navigate("/");
    } catch (err) {
      alert("Login failed");
    }
  };

  return (
    <button
      type="button"
      onClick={handleSubmit}
      className="absolute right-4 top-1/2 transform -translate-y-1/2"
    >
      Sign In
    </button>
  );
};

export default SignInButton;
