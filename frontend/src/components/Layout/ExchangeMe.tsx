import * as React from "react";
import { ExchangeHeader } from "./ExchangeHeader";

export const ExchangeMe: React.FC = () => {
  return (
    <div>
      <div className="fixed top-0 left-0 w-screen h-[12vh] bg-[#1E1E1E] z-50 flex items-center justify-center px-4">
        <ExchangeHeader />
      </div>
      <main className="pt-[12vh]">
      </main>
    </div>
  );
};

export default ExchangeMe;