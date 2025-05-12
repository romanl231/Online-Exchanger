import React, { useState } from "react";
import UnactiveEyeIcon from "./UnactiveEyeIcon";
import ActiveEyeIcon from "./ActiveEyeIcon";

interface PasswordInputProps {
  label: string;
  name: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onBlur: (e: React.FocusEvent<HTMLInputElement>) => void;
  error?: string | false;
}

const PasswordInput: React.FC<PasswordInputProps> = ({ 
  label,
  name,
  value,
  onChange,
  onBlur,
  error,
}) => {
  const [showPassword, setShowPassword] = useState(false);
  const [isHovered, setIsHovered] = useState(false);

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  const getIcon = () => {
    if (isHovered) {
      return showPassword ? <UnactiveEyeIcon /> : <ActiveEyeIcon />;
    }
    return showPassword ? <ActiveEyeIcon /> : <UnactiveEyeIcon />;
  };

  return (
    <div className="flex relative flex-col gap-2.5 w-full">
      <div className="relative">
        <label htmlFor={name} className="text-sm text-gray-300 mb-1">
          {label}
        </label>
        <input
          id={name}
          name={name}
          type={showPassword ? "text" : "password"}
          className={`w-full rounded-3xl border h-[41px] px-4 pr-10 
          text-gray-200 outline-none bg-zinc-800 
          ${error ? "border-red-500" : "border-neutral-700"} 
          focus:border-purple-400`}
          value={value}
          onChange={onChange}
          onBlur={onBlur}
        />
        <button
          type="button"
          onClick={togglePasswordVisibility}
          className="absolute right-2 top-[65%] translate-y-[-50%] 
          p-1 bg-transparent focus:outline-none hover:ring-2 hover:ring-purple-400"
          aria-label={showPassword ? "Hide password" : "Show password"}
          onMouseEnter={() => setIsHovered(true)}
          onMouseLeave={() => setIsHovered(false)}
        >
          {getIcon()}
        </button>
      </div>
      {error && <p className="text-red-500 text-sm">{error}</p>}
    </div>
  );
};

export default PasswordInput;
