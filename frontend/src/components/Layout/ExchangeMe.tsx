import * as React from "react";
import { ExchangeHeader } from "./ExchangeHeader";

export const ExchangeMe: React.FC = () => {
  return (
    <div>
      <div className="fixed top-0 left-0 w-screen h-[12vh] bg-color-1E1E1E z-50 flex items-center justify-center">
        <ExchangeHeader />
      </div>
    </div>
  );
};

export default ExchangeMe;