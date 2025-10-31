import { api } from "@/lib/axios";
import type { ApiResult, Category } from "@/types";

export const categoriesService = {
  //  Kullanıcının tüm kategorilerini getir
  getCategories: async (): Promise<ApiResult<Category[]>> => {
    const response = await api.get<ApiResult<Category[]>>("/categories");
    return response.data;
  },
};
