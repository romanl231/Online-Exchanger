import axios from "axios";
import { API_BASE_URL } from "../constants.ts";
import type { Category } from "../types/Category.ts";

const api = axios.create({
    baseURL: API_BASE_URL,
    withCredentials: true,
});

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