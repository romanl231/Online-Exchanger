import React from "react";
import { AuthService } from "../../api/authApi";
import { useAuth } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";

const SignInButton = ({ email, password }: { email: string; password: string }) => {
  const { setUser } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
      await AuthService.login({ email, password, ipAdress:"ipadd", deviceType:"itsme"});
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
      className="outline-none focus:outline-none 
      ring-0 focus:ring-0 w-full bg-purple-600 text-white 
      py-2 rounded-xl hover:bg-purple-700 transition"
    >
      Sign In
    </button>
  );
};

export default SignInButton;
