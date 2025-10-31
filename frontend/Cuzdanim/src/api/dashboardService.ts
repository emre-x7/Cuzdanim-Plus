import { api } from "@/lib/axios";
import type { ApiResult, DashboardData } from "@/types";

export const dashboardService = {
  //  Dashboard verilerini getir
  getDashboard: async (): Promise<ApiResult<DashboardData>> => {
    const response = await api.get<ApiResult<DashboardData>>("/dashboard");
    return response.data;
  },
};
