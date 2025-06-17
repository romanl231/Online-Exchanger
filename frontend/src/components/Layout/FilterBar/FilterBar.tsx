import React, { useState } from "react";
import { FromInputField } from "./FromInputField";
import { ToInputField } from "./ToInputField";
import { CategorySelect } from "./CategorySelect";

export const FilterBar: React.FC = () => {
  const [minPrice, setMinPrice] = useState("");
  const [maxPrice, setMaxPrice] = useState("");
  const [selectedCategories, setSelectedCategories] = useState<string[]>([]);

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
            selectedCategories={selectedCategories}
            onChange={setSelectedCategories}
          />

          <button
            type="submit"
            className="bg-purple-400 text-white px-3 py-1 rounded hover:bg-purple-400"
          >
            Apply Filters
          </button>
        </form>
    </div>
  );
};