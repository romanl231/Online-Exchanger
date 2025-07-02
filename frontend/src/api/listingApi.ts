import { api } from "./api";

export const ListingService = {
  create: async (data: {
    title: string;
    images: File[];
    description: string;
    price: number;
    categoryIds: string[];
  }) => {
    const formData = new FormData();

    formData.append("Title", data.title);
    formData.append("Description", data.description);
    formData.append("Price", data.price.toString());

    data.categoryIds.forEach((id) => {
      formData.append("CategoryIds", id);
    });

    data.images.forEach((image) => {
      formData.append("Images", image);
    });

    return api.post("/listing/create", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
  },
};