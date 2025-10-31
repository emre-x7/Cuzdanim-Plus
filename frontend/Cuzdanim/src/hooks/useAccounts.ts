import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { accountsService } from "@/api";
import type { CreateAccountRequest, UpdateAccountRequest } from "@/types";

// Query keys (cache yönetimi için)
const accountKeys = {
  all: ["accounts"] as const,
  detail: (id: string) => ["accounts", id] as const,
};

/**
 * Tüm hesapları getir
 */
export function useAccounts() {
  return useQuery({
    queryKey: accountKeys.all,
    queryFn: async () => {
      const response = await accountsService.getAccounts();
      if (!response.isSuccess) {
        throw new Error(response.message);
      }
      return response.data || [];
    },
  });
}

/**
 * Yeni hesap oluştur
 */
export function useCreateAccount() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateAccountRequest) =>
      accountsService.createAccount(data),
    onSuccess: (response) => {
      if (response.isSuccess) {
        // Cache'i invalidate et (yeniden fetch et)
        queryClient.invalidateQueries({ queryKey: accountKeys.all });

        toast.success("Hesap oluşturuldu!", {
          description: "Yeni hesap başarıyla eklendi.",
        });
      } else {
        toast.error("Hesap oluşturulamadı", {
          description: response.message,
        });
      }
    },
    onError: (error: any) => {
      const errorMessage = error.response?.data?.message || "Bir hata oluştu";
      toast.error("Hesap oluşturulamadı", { description: errorMessage });
    },
  });
}

/**
 * Hesap güncelle
 */
export function useUpdateAccount() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateAccountRequest }) =>
      accountsService.updateAccount(id, data),
    onSuccess: (response) => {
      if (response.isSuccess) {
        // Cache'i invalidate et
        queryClient.invalidateQueries({ queryKey: accountKeys.all });

        toast.success("Hesap güncellendi!");
      } else {
        toast.error("Hesap güncellenemedi", {
          description: response.message,
        });
      }
    },
    onError: (error: any) => {
      const errorMessage = error.response?.data?.message || "Bir hata oluştu";
      toast.error("Hesap güncellenemedi", { description: errorMessage });
    },
  });
}

/**
 * Hesap sil
 */
export function useDeleteAccount() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => accountsService.deleteAccount(id),
    onSuccess: (response) => {
      if (response.isSuccess) {
        // Cache'i invalidate et
        queryClient.invalidateQueries({ queryKey: accountKeys.all });

        toast.success("Hesap silindi!");
      } else {
        toast.error("Hesap silinemedi", {
          description: response.message,
        });
      }
    },
    onError: (error: any) => {
      const errorMessage = error.response?.data?.message || "Bir hata oluştu";
      toast.error("Hesap silinemedi", { description: errorMessage });
    },
  });
}
