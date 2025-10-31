import { useQuery } from "@tanstack/react-query";
import { dashboardService } from "@/api";

/**
 * Dashboard verilerini getir
 */
export function useDashboard() {
  return useQuery({
    queryKey: ["dashboard"],
    queryFn: async () => {
      const response = await dashboardService.getDashboard();
      if (!response.isSuccess) {
        throw new Error(response.message);
      }
      return response.data;
    },
    staleTime: 1 * 60 * 1000, // 1 dakika boyunca fresh
  });
}
