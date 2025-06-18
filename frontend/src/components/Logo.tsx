import React from "react";
import { useNavigate } from "react-router-dom";
const Logo: React.FC = () => {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(`/`);
  }

  return (
        <h1 className="font-joti text-4xl text-purple-400" onClick={handleClick}>ExchangeMe</h1>
        
  );
};

export default Logo;