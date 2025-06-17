import * as React from "react";

interface ListItemButtonProps {
  text?: string;
  onClick?: () => void;
}

export const ListItemButton: React.FC<ListItemButtonProps> = ({
  text = "List an Item",
  onClick
}) => {
  return (
      <button
        onClick={onClick}
        className="px-6 py-3 bg-purple-400 rounded-full text-white font-semibold hover:bg-purple-500 transition-colors max-md:px-5"
      >
      {text}
    </button>
  );
};