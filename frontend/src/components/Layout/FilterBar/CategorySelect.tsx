import { useCategories } from "../../../hooks/useCategories";

interface CategorySelectProps {
  selectedCategoryIds: string[];
  onChange: (selected: string[]) => void;
}

export const CategorySelect: React.FC<CategorySelectProps> = ({
  selectedCategoryIds,
  onChange,
}) => {
  const { categories, loading, categoryError } = useCategories();

  if (loading) return <p>Loading categories...</p>;
  if (categoryError) return <p className="text-red-500">{categoryError}</p>;

  const handleCheckboxChange = (categoryId: string) => {
    if (selectedCategoryIds.includes(categoryId)) {
      onChange(selectedCategoryIds.filter((c) => c !== categoryId));
    } else {
      onChange([...selectedCategoryIds, categoryId]);
    }
  };

  return (
    <div className="mb-3">
      <label className="block text-sm font-medium text-[#EAEAEA] mb-1">
        Categories
      </label>
      <div className="flex flex-col gap-2">
        {categories.map((category) => (
          <label key={category.id} className="flex items-center gap-2 text-[#EAEAEA]">
            <input
              type="checkbox"
              value={category.id}
              checked={selectedCategoryIds.includes(category.id)}
              onChange={() => handleCheckboxChange(category.id)}
              className="accent-purple-500"
            />
            {category.name}
          </label>
        ))}
      </div>
    </div>
  );
};