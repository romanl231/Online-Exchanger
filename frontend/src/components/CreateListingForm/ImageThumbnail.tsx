import { Xsvg } from "../Shared/Xsvg";

interface ImageThumbnailProps {
  file: File;
  onRemove: (filename: string) => void;
}

export function ImageThumbnail({ file, onRemove }: ImageThumbnailProps) {
  return (
    <div className="mb-3.5">
      <div className="flex relative flex-col items-end px-6 pb-24 aspect-square w-[120px] max-md:pl-5">
        <img
          src={URL.createObjectURL(file)}
          className="object-cover absolute inset-0 size-full"
          alt={file.name}
        />
        <button 
          className="absolute top-0 right-0 z-10 bg-transparent p-0 m-0 border-none inline-flex items-center justify-center"
          onClick={() => onRemove(file.name)}>
          <Xsvg />
        </button>
      </div>
      <p className="self-center text-sm text-gray-200 text-center">
        {file.name.length > 15 ? `${file.name.slice(0, 12)}...` : file.name}
      </p>
    </div>
  );
}