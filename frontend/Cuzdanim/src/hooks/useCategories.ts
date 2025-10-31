import { useQuery } from "@tanstack/react-query";
import { categoriesService } from "@/api";

/**
 * Kullanıcının tüm kategorilerini getir
 */
export function useCategories() {
  return useQuery({
    queryKey: ["categories"],
    queryFn: async () => {
      const response = await categoriesService.getCategories();
      if (!response.isSuccess) {
        throw new Error(response.message);
      }
      return response.data || [];
    },
    staleTime: 5 * 60 * 1000, // 5 dakika boyunca fresh
  });
}
