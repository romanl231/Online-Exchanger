interface PriceSectionProps {
  titleText: string;
}

export function PriceSection({titleText}: PriceSectionProps) {
  return (
    <section className="mt-16 max-md:mt-10">
      <div className="flex gap-5 justify-between items-end max-w-full text-gray-200 w-[792px]">
        <div className="flex flex-col self-start">
          <h2 className="text-3xl font-semibold">{titleText}</h2>
          <div className="self-center mt-6 ml-5 text-sm">0$</div>
        </div>
        <div className="flex flex-col mt-9 text-sm whitespace-nowrap">
          <input
            type="text"
            defaultValue="600000$"
            className="px-5 py-px rounded-3xl border border-solid bg-zinc-800 border-neutral-700 text-gray-200"
          />
          <div className="flex shrink-0 self-center mt-1 bg-purple-400 rounded-full h-[25px] w-[25px]" />
        </div>
        <div className="mt-16 text-sm max-md:mt-10">999999 $</div>
      </div>
    </section>
  );
}