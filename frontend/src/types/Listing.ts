import type { Category } from "./Category";
import type { User } from "./User";

export interface Listing {
  id: string;
  title: string;
  description: string;
  name: string;
  price: string;
  created: string;
  updated: string;
  categories: Category[];
  user: User;
}