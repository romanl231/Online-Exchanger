import * as React from "react";
import { SearchBar } from "./Searchbar";
import { ProfileLink } from "./ProfileLink";
import { ListItemButton } from "./ListItemButtons";
import Logo from "../Logo.tsx";

export const ExchangeHeader: React.FC = () => {
  

  return (
    <header className="w-full max-w-7xl flex justify-between items-center gap-6">
      <Logo />
      <nav className="flex items-center gap-6 flex-grow">
        <div className="flex-grow max-w-[600px]">
          <SearchBar />
        </div>
        <ProfileLink />
        <ListItemButton />
      </nav>
    </header>
  );
};
