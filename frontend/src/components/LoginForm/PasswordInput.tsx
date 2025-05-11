"use client";

import React, { useState } from "react";
import EyeIcon from "./EyeIcon";

interface PasswordInputProps {
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const PasswordInput: React.FC<PasswordInputProps> = ({ 
    value,
    onChange,
 }) => {
  const [showPassword, setShowPassword] = useState(false);

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  return (
    <div className="flex relative flex-col gap-2.5 w-full">
      <div className="relative">
        <label htmlFor="password-input" className="text-sm text-gray-300 mb-1">Password</label>
        <input
          type={showPassword ? "text" : "password"}
          className="w-full rounded-3xl border bg-zinc-800 border-neutral-700 h-[41px] px-4 text-gray-200 outline-none focus:border-purple-400"
          value={value}
          onChange={onChange}
        />
        <button
          type="button"
          onClick={togglePasswordVisibility}
          className="absolute right-4 top-1/2 transform -translate-y-1/2"
          aria-label={showPassword ? "Hide password" : "Show password"}
        >
          <EyeIcon />
        </button>
      </div>
    </div>
  );
};

export default PasswordInput;
