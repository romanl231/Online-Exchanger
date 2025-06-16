import * as React from "react";
import { Link } from "react-router-dom";

interface ProfileLinkProps {
  text?: string;
  myProfileUrl: string;
}

export const ProfileLink: React.FC<ProfileLinkProps> = ({
  text = "Your profile",
  myProfileUrl = "/me"
}) => {
  return (
    <Link
        className="text-[#EAEAEA] hover:text-purple-400 bg-transparent flex gap-3 self-stretch my-auto transition-colors duration-300"
        to={myProfileUrl}
    >
      <img
        src="https://cdn.builder.io/api/v1/image/assets/TEMP/f9b75ae5baf3509fa7f51c083bf351a29866c9a0?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec"
        className="object-contain shrink-0 aspect-[0.81] w-[22px]"
        alt="Profile icon"
      />
      <span className="my-auto">
        {text}
      </span>
    </Link>
  );
};