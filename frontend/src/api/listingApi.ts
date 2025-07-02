import { api } from "./api";

export const ListingService = {
    create: (data: {
        title:string;
        images: File[];
        description:string;
        price:string;
        categoryIds:string[];
    }) => { api.post("listing/create", {
        Title: data.title,
        Images: data.images,
        Description: data.description,
        Price: data.price,
        CategoryIds: data.categoryIds,
    }) }
}