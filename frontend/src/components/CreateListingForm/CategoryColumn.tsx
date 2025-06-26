import type { Category } from "../../types/Category";

interface CategoryColumnProps {
  categories: Category[];
}

export function CategoryColumn({ categories }: CategoryColumnProps) {
  return (
    <div className="w-[33%] max-md:w-full">
      <div className="flex flex-col w-full gap-6">
        {categories.map((category) => (
          <label
            key={category.name}
            className="flex items-center gap-3 text-lg font-medium text-gray-200 cursor-pointer"
          >
            <input
              type="checkbox"
              className="accent-purple-500 w-5 h-5"
            />
            <img
              src={category.iconUrl}
              alt={category.name}
              className="w-6 h-6 object-contain"
            />
            <span>{category.name}</span>
          </label>
        ))}
      </div>
    </div>
  );
}