interface DescriptionSectionProps {
  titleText: string;
}

export function DescriptionSection({titleText}: DescriptionSectionProps) {
  return (
    <section className="mt-24 max-md:mt-10">
      <h2 className="self-start text-3xl font-semibold text-gray-200 max-md:ml-1">
        {titleText}
      </h2>
      <textarea
        placeholder="Tell more about what you want to sell..."
        className="self-center px-5 py-3.5 mt-3.5 max-w-full text-sm text-gray-200 rounded-3xl border border-solid bg-zinc-800 border-neutral-700 w-[600px] max-md:px-5 placeholder-gray-200 resize-none h-20"
      />
    </section>
  );
}