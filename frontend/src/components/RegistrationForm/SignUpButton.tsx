import React from "react";
import { useAuth } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { AuthService } from "../../api/authApi";
import { toast } from 'react-toastify';
import { getApiErrorMessage } from "../../utils/getApiErrorMessage.ts";

interface SignUpButtonProps {
  email: string;
  password: string;
  confirmPassword: string;
  disabled: boolean;
}

const SignUpButton: React.FC<SignUpButtonProps> = ({ 
  email, 
  password, 
  confirmPassword,
  disabled, 
}) => {
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
      toast.success("Registration successed", {
        position: "top-center",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        theme: "dark",
      });
    } catch (err: any) {
        toast.error(getApiErrorMessage(err), {
        position: "top-center",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        theme: "dark",
      });
    }
  };

  return (
    <button
      type="button"
      disabled={disabled}
      onClick={handleSubmit}
      className={`outline-none focus:outline-none ring-0
      ${disabled ? "opacity-50 cursor-not-allowed" : ""}
      focus:ring-0 w-full bg-purple-600 text-white 
      py-2 rounded-xl hover:bg-purple-700 transition`}
    >
      Sign Up
    </button>
  );
};

export default SignUpButton;