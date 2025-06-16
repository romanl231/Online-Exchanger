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
      className="self-stretch px-12 py-4 bg-purple-400 rounded-[100px] max-md:px-5"
      onClick={onClick}
    >
      {text}
    </button>
  );
};