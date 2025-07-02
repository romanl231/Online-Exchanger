import { CategoryColumn } from "./CategoryColumn";
import { useCategories } from "../../hooks/useCategories";

interface CategoriesSectionProps {
  titleText: string;
  selectedCategoryIds: string[];
  setSelectedCategoryIds: (selectedCategoryIds: string[]) => void;
  error?: string;
}

export function CategoriesSection({ titleText, selectedCategoryIds, setSelectedCategoryIds, error }: CategoriesSectionProps) {
  const { categories, loading, categoryError } = useCategories();

  if (loading) return <p>Loading categories...</p>;
  if (categoryError) return <p className="text-red-500">{categoryError}</p>;

  const chunkSize = 4;
  const columns = [];
  for (let i = 0; i < categories.length; i += chunkSize) {
    columns.push(categories.slice(i, i + chunkSize));
  }

  return (
    <section className="mt-14 max-md:mt-10">
      <h2 className="text-3xl font-semibold text-gray-200 mb-4 max-md:ml-1">{titleText}</h2>
      <div className="flex justify-center">
        <div className={`w-full max-w-[600px] px-6 py-8 rounded-3xl border bg-zinc-800 
          ${error ? "border-red-500" : "border-neutral-700" }`}>
          <div className="flex gap-5 flex-wrap max-md:flex-col">
            {columns.map((chunk, index) => (
              <CategoryColumn
                key={index}
                categories={chunk}
                selectedCategoryIds={selectedCategoryIds}
                setSelectedCategoryIds={setSelectedCategoryIds}
              />
            ))}
          </div>
        </div>
      </div>
      {error && <p className="text-red-500 text-sm">{error}</p>}
    </section>
  );
}