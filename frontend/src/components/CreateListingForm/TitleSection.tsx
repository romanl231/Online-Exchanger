interface TitleSectionProps {
  titleText: string;
  title: string;
  setTitle: (title: string) => void;
  error?: string | false;
}

export function TitleSection({ titleText, title, setTitle, error }: TitleSectionProps) {
  return (
    <section className="mt-24 max-md:mt-10">
      <h2 className="text-3xl font-semibold text-gray-200 mb-2 max-md:ml-1">
        {titleText}
      </h2>
      <div className="flex justify-center">
        <input
          type="text"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          placeholder="What are you selling?"
          className={`w-full max-w-[600px] px-4 
            py-3.5 text-sm text-gray-200 rounded-3xl 
            border bg-zinc-800 ${error ? "border-red-500" : "border-neutral-700"} placeholder-gray-400`}
        />
      </div>
      {error && <p className="text-red-500 text-sm">{error}</p>}
    </section>
  );
}