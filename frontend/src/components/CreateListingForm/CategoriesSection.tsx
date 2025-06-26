import { CategoryColumn } from "./CategoryColumn";
import type { Category } from "../../types/Category";

interface CategoriesSectionPorps {
  titleText: string;
}

export function CategoriesSection({titleText}: CategoriesSectionPorps) {
  const categories: Category[] = [
    { id: "1", name: "Category1", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "2", name: "Category2", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "3", name: "Category3", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "4", name: "Category4", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "5", name: "Category5", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "6", name: "Category6", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "7", name: "Category7", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "8", name: "Category8", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "9", name: "Category9", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "10", name: "Category10", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "11", name: "Category11", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "12", name: "Category12", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "13", name: "Category13", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "14", name: "Category14", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
    { id: "15", name: "Category15", iconUrl: "https://cdn.builder.io/api/v1/image/assets/TEMP/1078de6f83b4803a96de08ff9def8876db5ce41c?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec" },
  ];

  const chunkSize = 4;
  const columns = [];
  for (let i = 0; i < categories.length; i += chunkSize) {
    columns.push(categories.slice(i, i + chunkSize));
  }

  return (
    <section className="mt-14 max-md:mt-10">
      <h2 className="text-3xl font-semibold text-gray-200">{titleText}</h2>
      <div className="px-12 py-11 mt-2 max-w-full rounded-3xl border bg-zinc-800 border-neutral-700 w-[600px] max-md:px-5">
        <div className="flex gap-5 flex-wrap max-md:flex-col">
          {columns.map((chunk, index) => (
            <CategoryColumn key={index} categories={chunk} />
          ))}
        </div>
      </div>
    </section>
  );
}