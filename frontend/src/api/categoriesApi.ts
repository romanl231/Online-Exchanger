import type { Category } from "../types/Category.ts";
import { api } from "./api.ts";

api.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        if(error.response){
            return Promise.reject(error);
        }
        throw new Error("Network error or no response from server");
    }
);

export const CategoryService = {
  getAll: (): Promise<Category[]> => 
    api.get<Category[]>("listing/category/all").then((res) => res.data),
};