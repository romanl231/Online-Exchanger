import { ImageUploadSection } from "./ImageUploadSection";
import { TitleSection } from "./TitleSection";
import { CategoriesSection } from "./CategoriesSection";
import { PriceSection } from "./PriceSection";
import { DescriptionSection } from "./DescriptionSection";
import { PublishButton } from "./PublishButton";
import { useState } from "react";

export default function ListingForm() {
  const [images, setImages] = useState<File[]>([]);
  const [title, setTitle] = useState<string>("");
  const isActive = images.length > 1;

  return (
    <div className="flex justify-center items-start w-full px-4 py-10">
      <main className="w-full bg-neutral-800 rounded-2xl p-8 shadow-lg">
        <div className="flex-col gap-6">
          <ImageUploadSection
            images={images}
            setImages={setImages} 
            titleText="Upload images"/>
          <TitleSection
            title={title}
            setTitle={setTitle} 
            titleText="Create title"/>
          <CategoriesSection 
            titleText="Choose categories"/>
          <PriceSection 
            titleText="Select price"/>
          <DescriptionSection 
            titleText="Description"/>
          <div className="flex justify-end">
            <PublishButton 
            isActive={isActive}/>
          </div>
        </div>
      </main>
    </div>
  );
}