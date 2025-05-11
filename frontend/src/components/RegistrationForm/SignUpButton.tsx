import React from "react";
import { useAuth } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { AuthService } from "../../api/authApi";

interface SignUpButtonProps {
  email: string;
  password: string;
  confirmPassword: string;
}

const SignUpButton: React.FC<SignUpButtonProps> = ({ email, password, confirmPassword }) => {
  const { setUser } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
      if (password !== confirmPassword) {
        throw new Error("Passwords do not match");
      }

      await AuthService.register({ email, password });
      const me = await AuthService.me();
      setUser(me.data);
      navigate("/");
    } catch (err) {
      alert((err as Error).message || "Registration failed");
    }
  };

  return (
    <button
      type="button"
      onClick={handleSubmit}
      className="w-full bg-purple-600 text-white py-2 rounded-xl hover:bg-purple-700 transition"
    >
      Sign Up
    </button>
  );
};

export default SignUpButton;