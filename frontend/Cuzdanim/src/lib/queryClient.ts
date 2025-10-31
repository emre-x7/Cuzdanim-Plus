import { QueryClient } from "@tanstack/react-query";

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      // Varsayılan ayarlar
      retry: 1, // Hata durumunda 1 kez daha dene
      refetchOnWindowFocus: false, // Pencere focus olduğunda refetch yapma
      staleTime: 5 * 60 * 1000, // 5 dakika boyunca data "fresh" kabul edilir
      gcTime: 10 * 60 * 1000, // 10 dakika sonra cache'den sil
    },
    mutations: {
      retry: 0, // Mutation'larda retry yapma (create, update, delete)
    },
  },
});
