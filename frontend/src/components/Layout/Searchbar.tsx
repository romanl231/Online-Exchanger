import * as React from "react";
import { FilterButton } from "./FilterButton";
import { SearchButton } from "./SearchButton";
import { useState } from "react";
import { FilterBar } from "./FilterBar/FilterBar";

interface SearchBarProps {
  placeholder?: string;
}

export const SearchBar: React.FC<SearchBarProps> = ({
  placeholder = "What are you looking for?",
}) => {
  const [isFilterOpen, setIsFilterOpen] = useState(false);

  const handleFilterClick = () => {
    setIsFilterOpen((prev) => !prev);
  };

  return (
    <div className="w-full flex items-center gap-4 px-4 py-2.5 rounded-3xl border border-neutral-700 bg-zinc-800">
      <SearchButton />
      <input
        type="text"
        placeholder={placeholder}
        className="w-full bg-transparent outline-none text-gray-200 placeholder-gray-400"
      />
      <FilterButton handleFilterClick={handleFilterClick} />
      {isFilterOpen && <FilterBar />}
    </div>
  );
};