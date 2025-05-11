import { useState } from "react";
import { AuthService } from "../api/authApi";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import LoginForm from "../components/LoginForm/LoginForm";

const Login = () => {
  const { setUser } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
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
    <LoginForm />
  );
};

export default Login;