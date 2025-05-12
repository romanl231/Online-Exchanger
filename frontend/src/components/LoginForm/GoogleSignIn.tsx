import React from "react";
import { Link } from "react-router-dom";
import GoogleIcon from "./GoogleIcon";

const GoogleSignIn: React.FC = () => {
  return (
    <Link
      to="/"
      className="flex gap-2.5 items-center cursor-pointer hover:opacity-80 transition-opacity"
    >
      <GoogleIcon />
      <span className="text-sm text-purple-400">Continue with google</span>
    </Link>
  );
};

export default GoogleSignIn;
