import React from "react";
import GoogleIcon from "./GoogleIcon";

const GoogleSignIn: React.FC = () => {
  return (
    <button
      type="button"
      className="flex gap-2.5 items-center cursor-pointer hover:opacity-80 transition-opacity"
    >
      <GoogleIcon />
      <span className="text-sm text-purple-400">Continue with google</span>
    </button>
  );
};

export default GoogleSignIn;
