interface PriceSectionProps {
  titleText: string;
  price: number;
  setPrice: (price: number) => void;
}

export function PriceSection({ titleText, price, setPrice }: PriceSectionProps) {
  const min = 1;
  const max = 100000;

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = Number(e.target.value);
    if (newValue >= min && newValue <= max) {
      setPrice(newValue);
    }
  };

  return (
    <section className="mt-16 max-md:mt-10">
      <h2 className="text-3xl font-semibold text-gray-200 mb-6">{titleText}</h2>

      <div className="relative w-full max-w-[600px] mx-auto">
        <input
          type="number"
          value={price}
          onChange={handleInputChange}
          min={min}
          max={max}
          className="absolute left-1/2 -translate-x-1/2 -top-9 w-[100px] text-center bg-neutral-800 text-gray-200 text-sm px-3 py-1 rounded-xl border border-neutral-600"
        />

        <input
          type="range"
          min={min}
          max={max}
          value={price}
          onChange={(e) => setPrice(Number(e.target.value))}
          className="w-full h-2 bg-gradient-to-r from-purple-500 to-white rounded-full appearance-none cursor-pointer
            [&::-webkit-slider-thumb]:appearance-none
            [&::-webkit-slider-thumb]:w-5
            [&::-webkit-slider-thumb]:h-5
            [&::-webkit-slider-thumb]:rounded-full
            [&::-webkit-slider-thumb]:bg-purple-500
            [&::-webkit-slider-thumb]:border-none"
        />

        <div className="flex justify-between mt-2 text-sm text-gray-200">
          <span>{min}$</span>
          <span>{max.toLocaleString()} $</span>
        </div>
      </div>
    </section>
  );
}