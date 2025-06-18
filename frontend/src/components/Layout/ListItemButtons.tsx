import * as React from "react";
import { useNavigate } from "react-router-dom";

interface ListItemButtonProps {
  text?: string;
}

export const ListItemButton: React.FC<ListItemButtonProps> = ({
  text = "List an Item",
}) => {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(`/listing/create`);
  }
  return (
      <button
        onClick={handleClick}
        className="px-6 py-3 bg-purple-400 rounded-full text-white font-semibold hover:bg-purple-500 transition-colors max-md:px-5"
      >
      {text}
    </button>
  );
};