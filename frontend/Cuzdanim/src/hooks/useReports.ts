import { useQuery } from "@tanstack/react-query";
import { reportsService } from "@/api";
import type { GetReportQuery } from "@/types";

/**
 * Rapor verisi getir
 */
export function useReports(query: GetReportQuery = {}) {
  return useQuery({
    queryKey: ["reports", query],
    queryFn: async () => {
      const response = await reportsService.getReport(query);
      if (!response.isSuccess) {
        throw new Error(response.message);
      }
      return response.data;
    },
  });
}
