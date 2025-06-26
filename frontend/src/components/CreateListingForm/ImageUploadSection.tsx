import { AddImageButton } from "./AddImageButton";
import { ImageThumbnail } from "./ImageThumbnail";
import { useRef } from "react";

interface ImageUploadSectionProps {
  titleText: string;
  images: File[];
  setImages: React.Dispatch<React.SetStateAction<File[]>>; 
}

export function ImageUploadSection({ titleText, images, setImages}: ImageUploadSectionProps) {
  const isActive = images.length > 20;
  const fileInputRef = useRef<HTMLInputElement>(null);
  
  const fullRows = Math.floor(images.length / 4);
  const fullRowsChunks = [];
  for (let i = 0; i < fullRows * 4; i += 4) {
    fullRowsChunks.push(images.slice(i, i + 4));
  }

  const lastRow = images.slice(fullRows * 4);

  const handleRemoveImage = (filenameToRemove: string) => {
    setImages(prev => prev.filter(file => file.name !== filenameToRemove));
  };

  const handleAddImages = () => {
    fileInputRef.current?.click();
  };

  const handleFilesSelected = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (files) {
      const newFiles = Array.from(files);
      setImages(prev => [...prev, ...newFiles]);
      e.target.value = "";
    }
  };

  return (
    <section>
      <h2 className="text-3xl font-semibold text-gray-200 mb-4">{titleText}</h2>

      <input
        ref={fileInputRef}
        type="file"
        accept="image/*"
        multiple
        className="hidden"
        onChange={handleFilesSelected}
      />

      <div className="rounded-3xl border border-neutral-700 bg-zinc-800 p-6 max-md:px-4">
        {fullRowsChunks.map((chunk, rowIdx) => (
          <div key={rowIdx} className="grid grid-cols-4 gap-4 max-md:grid-cols-2 mb-4">
            {chunk.map((image, i) => (
              <ImageThumbnail
                key={i}
                file={image}
                onRemove={handleRemoveImage}
              />
            ))}
          </div>
        ))}

        {lastRow.length > 0 && (
        <div className="flex justify-center gap-16 mb-4">
          {lastRow.map((image, i) => (
            <ImageThumbnail
              key={i}
              file={image}
              onRemove={handleRemoveImage}
            />
          ))}
        </div>
      )}

        <div className="mt-6 flex justify-center">
          <AddImageButton 
            isActive={isActive}
            handleClick={handleAddImages}/>
        </div>
      </div>
    </section>
  );
}
