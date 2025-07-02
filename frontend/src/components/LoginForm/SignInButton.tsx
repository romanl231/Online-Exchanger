import { AuthService } from "../../api/authApi";
import { useAuth } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { getApiErrorMessage } from "../../utils/getApiErrorMessage";
import { getClientIp, getDeviceFingerprint } from "../../utils/getClientInfo";
import type { User } from "../../types/User";

const SignInButton = ({ email, password, disabled } : 
  { email: string; password: string; disabled: boolean}) => {
  const { setUser } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async () => {
    try {
        const ipAddress = await getClientIp();
        const deviceType = getDeviceFingerprint();
        await AuthService.login({ email, password, ipAddress, deviceType});
        const me = await AuthService.me();
        setUser(me.data as User);
        navigate("/");
        toast.success("Login succeed", {
          position: "top-center",
          autoClose: 5000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          theme: "dark",
        });
      } catch (err : any) {
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
      className={`outline-none focus:outline-none
      ${disabled ? "opacity-50 cursor-not-allowed" : ""}
      ring-0 focus:ring-0 w-full bg-purple-600 text-white 
      py-2 rounded-xl hover:bg-purple-700 transition`}
    >
      Sign In
    </button>
  );
};

export default SignInButton;
