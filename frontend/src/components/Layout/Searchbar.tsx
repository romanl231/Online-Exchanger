import * as React from "react";

interface SearchBarProps {
  placeholder?: string;
}

export const SearchBar: React.FC<SearchBarProps> = ({
  placeholder = "What are you looking for?"
}) => {
  return (
    <div className="flex items-center gap-4 self-stretch px-4 py-2.5 rounded-3xl border border-neutral-700 bg-zinc-800 max-md:pr-5">
      
      <button className="bg-transparent hover:bg-transparent focus:bg-transparent">
        <img
        src="https://cdn.builder.io/api/v1/image/assets/TEMP/4272772a03f465985b36fe6e67db6fb3e49023f2?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec"
        className="w-5 h-5 object-contain shrink-0"
        alt="Search icon"
        />
      </button>

      <input
        type="text"
        placeholder={placeholder}
        className="flex-grow bg-transparent outline-none text-gray-200 placeholder-gray-400"
      />

      <button className="bg-transparent hover:bg-transparent focus:bg-transparent">
      <img
        src="https://cdn.builder.io/api/v1/image/assets/TEMP/00051a37d1738bd649c2480085f98310266cb64b?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec"
        className="w-5 h-5 object-contain shrink-0"
        alt="Filter icon"
      />
      </button>
    </div>
  );
};