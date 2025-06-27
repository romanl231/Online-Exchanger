import type { Category } from "../../types/Category";

interface CategoryColumnProps {
  categories: Category[];
  selectedCategoryIds: string[];
  setSelectedCategoryIds: (selectedCategoryIds: string[]) => void;
}

export function CategoryColumn({ categories, selectedCategoryIds, setSelectedCategoryIds }: CategoryColumnProps) {
  const handleCheckboxChange = (categoryId: string) => {
    if (selectedCategoryIds.includes(categoryId)) {
      setSelectedCategoryIds(selectedCategoryIds.filter((c) => c !== categoryId));
    } else {
      setSelectedCategoryIds([...selectedCategoryIds, categoryId]);
    }
  };

  return (
    <div className="w-[33%] max-md:w-full">
      <div className="flex flex-col gap-6">
        {categories.map((category) => {
          const inputId = `checkbox-${category.id}`;
          return (
            <label
              htmlFor={inputId}
              key={category.id}
              className="flex items-center gap-3 text-lg font-medium text-gray-200 cursor-pointer"
            >
              <input
                id={inputId}
                type="checkbox"
                value={category.id}
                checked={selectedCategoryIds.includes(category.id)}
                onChange={() => handleCheckboxChange(category.id)}
                className="accent-purple-500 w-5 h-5"
              />
              <span>{category.name}</span>
            </label>
          );
        })}
      </div>
    </div>
  );
}