import { ImageUploadSection } from "./ImageUploadSection";
import { TitleSection } from "./TitleSection";
import { CategoriesSection } from "./CategoriesSection";
import { PriceSection } from "./PriceSection";
import { DescriptionSection } from "./DescriptionSection";
import { PublishButton } from "./PublishButton";
import React, { useState } from "react";
import { ListingService } from "../../api/listingApi";
import { toast } from "react-toastify";
import { getApiErrorMessage } from "../../utils/getApiErrorMessage";

export default function ListingForm() {
  const [images, setImages] = useState<File[]>([]);
  const [title, setTitle] = useState<string>("");
  const [selectedCategoryIds, setSelectedCategoryIds] = useState<string[]>([]);
  const [price, setPrice] = useState<number>(1);
  const [description, setDescription] = useState<string>("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const listingData = {
        title: title,
        price: price,
        description: description,
        categoryIds: selectedCategoryIds,
        images: images,
      }
      await ListingService.create(listingData);
      toast.success("Login succeed", {
        position: "top-center",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        theme: "dark",
        });
    } catch (err) {
      toast.error(getApiErrorMessage(err), {
        position: "top-center",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        theme: "dark",
      });
    }
  }

  return (
    <div className="flex justify-center items-start w-full px-4 py-10">
      <form 
        className="w-full bg-neutral-800 rounded-2xl p-8 shadow-lg"
        onSubmit={handleSubmit}>
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
            selectedCategoryIds={selectedCategoryIds}
            setSelectedCategoryIds={setSelectedCategoryIds} 
            titleText="Choose categories"/>
          <PriceSection
            price={price}
            setPrice={setPrice} 
            titleText="Select price"/>
          <DescriptionSection
            description={description}
            setDescription={setDescription} 
            titleText="Description"/>
          <div className="py-10 flex justify-center">
            <PublishButton />
          </div>
        </div>
      </form>
    </div>
  );
}