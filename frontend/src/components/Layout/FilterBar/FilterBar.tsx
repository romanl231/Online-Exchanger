import React from "react";
import { FromInputField } from "./FromInputField";
import { ToInputField } from "./ToInputField";
import { CategorySelect } from "./CategorySelect";

interface FilterBarProps {
  minPrice: string;
  setMinPrice: (value: string) => void;
  maxPrice: string;
  setMaxPrice: (value: string) => void;
  selectedCategoryIds: string[];
  setSelectedCategoryIds: (ids: string[]) => void;
}

export const FilterBar: React.FC<FilterBarProps> = ({
  minPrice,
  setMinPrice,
  maxPrice,
  setMaxPrice,
  selectedCategoryIds,
  setSelectedCategoryIds,
}) => {
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
  };

  return (
    <div className="relative">
        <form
          onSubmit={handleSubmit}
          className="absolute top-12 left-0 bg-[#1E1E1E] border border-gray-300 p-4 rounded shadow-md z-10 w-72"
        >

          <FromInputField 
            minPrice={minPrice} 
            onChange={setMinPrice}/>
          <ToInputField 
            maxPrice={maxPrice} 
            onChange={setMaxPrice}/>
          <CategorySelect 
            selectedCategoryIds={selectedCategoryIds}
            onChange={setSelectedCategoryIds}
          />
        </form>
    </div>
  );
};