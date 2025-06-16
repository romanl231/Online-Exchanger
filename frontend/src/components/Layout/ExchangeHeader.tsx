import * as React from "react";
import { SearchBar } from "./Searchbar";
import { ProfileLink } from "./ProfileLink";
import { ListItemButton } from "./ListItemButtons";
import Logo from "../Logo.tsx";

export const ExchangeHeader: React.FC = () => {
  return (
    <header className="w-full px-8 py-4 flex justify-between items-center max-md:flex-col max-md:items-start max-md:gap-4">
      <Logo />
      <nav className="flex gap-6 items-center text-sm text-gray-200 max-md:w-full max-md:justify-between">
        <SearchBar />
        <ProfileLink />
        <ListItemButton />
      </nav>
    </header>
  );
};
