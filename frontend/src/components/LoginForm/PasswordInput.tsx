"use client";

import React, { useState } from "react";
import UnactiveEyeIcon from "./UnactiveEyeIcon";
import ActiveEyeIcon from "./ActiveEyeIcon";

interface PasswordInputProps {
  label: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const PasswordInput: React.FC<PasswordInputProps> = ({ 
    label,
    value,
    onChange,
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
        <label htmlFor="password-input" className="text-sm text-gray-300 mb-1">{label}</label>
        <input
          type={showPassword ? "text" : "password"}
          className="w-full rounded-3xl border bg-zinc-800 border-neutral-700 
          h-[41px] px-4 text-gray-200 outline-none focus:border-purple-400"
          value={value}
          onChange={onChange}
        />
        <button
          type="button"
          onClick={togglePasswordVisibility}
          className="outline-none focus:outline-none ring-0 
             hover:ring-2 hover:ring-purple-400 
             absolute right-1 transform bg-transparent"
          aria-label={showPassword ? "Hide password" : "Show password"}
          onMouseEnter={() => setIsHovered(true)}
          onMouseLeave={() => setIsHovered(false)}
        >
          {getIcon()}
        </button>
      </div>
    </div>
  );
};

export default PasswordInput;
