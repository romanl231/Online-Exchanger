import { useEffect, useState } from "react";
import type { Category } from "../types/Category";
import { CategoryService } from "../api/categoriesApi";

export const useCategories = () => {
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(true);
    const [categoryError, setError] = useState<string | null>(null);

    useEffect(() => {
        CategoryService.getAll()
        .then(setCategories)
        .catch(() => setError("Failed to load categories"))
        .finally(() => setLoading(false));
    }, []);

    return {categories, loading, categoryError}
}