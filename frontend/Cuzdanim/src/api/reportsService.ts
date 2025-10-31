import { api } from "@/lib/axios";
import type { ApiResult, ReportData, GetReportQuery } from "@/types";

export const reportsService = {
  //  Tarih aralığına göre rapor verisi getir
  getReport: async (
    query: GetReportQuery = {}
  ): Promise<ApiResult<ReportData>> => {
    const params = new URLSearchParams();

    if (query.startDate) {
      params.append("startDate", query.startDate);
    }

    if (query.endDate) {
      params.append("endDate", query.endDate);
    }

    const response = await api.get<ApiResult<ReportData>>(
      `/reports?${params.toString()}`
    );
    return response.data;
  },
};
