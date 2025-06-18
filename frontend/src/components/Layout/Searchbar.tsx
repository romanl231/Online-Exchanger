import * as React from "react";
import { FilterButton } from "./FilterButton";
import { SearchInputField } from "./SearchInputField";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FilterBar } from "./FilterBar/FilterBar";
import { SearchSubmitButton } from "./SearchSubmitButton";

interface SearchBarProps {
}

export const SearchBar: React.FC<SearchBarProps> = ({
  
}) => {
  const [isFilterOpen, setIsFilterOpen] = useState(false);
  const [query, setQuery] = useState("");
  const [minPrice, setMinPrice] = useState("");
  const [maxPrice, setMaxPrice] = useState("");
  const [selectedCategoryIds, setSelectedCategoryIds] = useState<string[]>([]);
  const navigate = useNavigate();

  const handleSearch = (e?: React.MouseEvent | React.FormEvent) => {
    e?.preventDefault();
    const params = new URLSearchParams();

    if(query) params.set("q", query);
    if (minPrice) params.set("min", minPrice);
    if (maxPrice) params.set("max", maxPrice);
    if (selectedCategoryIds.length > 0)
      params.set("categories", selectedCategoryIds.join(","));

    navigate(`/listings?${params.toString()}`);
  }

  const handleFilterClick = () => {
    setIsFilterOpen((prev) => !prev);
  };

  return (
      <form 
            onSubmit={handleSearch}
            className="w-full flex items-center gap-4 px-4 py-2.5 rounded-3xl border border-neutral-700 bg-zinc-800">
            
            <SearchSubmitButton />
            <SearchInputField setQuery={setQuery}/>
            <FilterButton 
                    handleFilterClick={handleFilterClick} />
                  {isFilterOpen && <FilterBar
                    minPrice={minPrice}
                    setMinPrice={setMinPrice}
                    maxPrice={maxPrice} 
                    setMaxPrice={setMaxPrice}
                    selectedCategoryIds={selectedCategoryIds}
                    setSelectedCategoryIds={setSelectedCategoryIds}/>}
        </form>
  );
};