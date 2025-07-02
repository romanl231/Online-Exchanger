interface DescriptionSectionProps {
  titleText: string;
  description: string;
  setDescription: (description: string) => void;
  error?: string | false; 
}

export function DescriptionSection({titleText, description, setDescription, error}: DescriptionSectionProps) {
  return (
    <section className="mt-24 max-md:mt-10">
      <h2 className="self-start text-3xl font-semibold text-gray-200 max-md:ml-1">
        {titleText}
      </h2>
      <textarea
        placeholder="Tell more about what you want to sell..."
        className={`self-center px-5 py-3.5 mt-3.5 max-w-full text-sm 
        text-gray-200 rounded-3xl border border-solid bg-zinc-800 ${error ? "border-red-500" : "border-neutral-700"} 
        w-[600px] max-md:px-5 placeholder-gray-400 resize-none overflow-hidden`}
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        rows={3}
        onInput={(e) => {
          const target = e.target as HTMLTextAreaElement;
          target.style.height = "auto";
          target.style.height = target.scrollHeight + "px";
        }}
      />
      {error && <p className="text-red-500 text-sm">{error}</p>}
    </section>
  );
}