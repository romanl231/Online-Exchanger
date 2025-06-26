interface TitleSectionProps {
  titleText: string;
  title: string;
  setTitle: (title: string) => void;
}

export function TitleSection({titleText, title, setTitle}: TitleSectionProps) {

  const handleTitleChange = () => {

  }

  return (
    <section className="mt-24 max-md:mt-10">
      <h2 className="self-start text-3xl font-semibold text-gray-200 max-md:ml-1">
        {titleText}
      </h2>
      <input
        type="text"
        value={title}
        onChange={handleTitleChange}
        placeholder="What are you selling?"
        className="self-center px-4 py-3.5 mt-2.5 max-w-full text-sm text-gray-200 rounded-3xl border border-solid bg-zinc-800 border-neutral-700 w-[600px] max-md:pr-5 placeholder-gray-200"
      />
    </section>
  );
}