import * as React from "react";
import { ExchangeHeader } from "./ExchangeHeader";
import { Outlet } from "react-router-dom";

export const ExchangeMe: React.FC = () => {
  return (
    <div className="min-h-screen w-full">
      <div className="fixed top-0 
      left-0 w-screen h-[12vh] 
      bg-[#1E1E1E] z-50 flex 
      items-center justify-center px-4
      ">
        <ExchangeHeader />
      </div>
      <main className="pt-[12vh] px-4 sm:px-8 ">
        <div className="w-full">
          <Outlet />
        </div>
      </main>
    </div>
  );
};

export default ExchangeMe;