import { useCategories } from "../../../hooks/useCategories";

interface CategorySelectProps {
    selectedCategories: string[];
    onChange: (selected: string[]) => void;
}

export const CategorySelect: React.FC<CategorySelectProps> = ({
    selectedCategories,
    onChange,
}) => {

    const {categories, loading, error} = useCategories();

     if (loading) return <p>Loading categories...</p>;
     if (error) return <p className="text-red-500">{error}</p>;

     return (
    <div className="mb-3">
      <label className="block text-sm font-medium text-[#EAEAEA] mb-1">Categories</label>
       <select
        multiple
        value={selectedCategories}
        onChange={(e) =>
          onChange(Array.from(e.target.selectedOptions, (opt) => opt.value))
        }
        className="mt-1 block w-full border border-gray-300 rounded px-2 py-1 h-32"
      >
        {categories.map((category) => (
          <option key={category.id} value={category.name}>
            {category.name}
          </option>
        ))}
      </select>
    </div>
  );
}