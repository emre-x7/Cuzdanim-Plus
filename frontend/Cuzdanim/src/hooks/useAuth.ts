import { useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";
import { authService } from "@/api";
import { useAuthStore } from "@/store/authStore";
import type { LoginRequest, RegisterRequest } from "@/types";

/**
 * Login hook
 */
export function useLogin() {
  const navigate = useNavigate();
  const { setAuth } = useAuthStore();

  return useMutation({
    mutationFn: (data: LoginRequest) => authService.login(data),
    onSuccess: (response) => {
      if (response.isSuccess && response.data) {
        const {
          userId,
          email,
          firstName,
          lastName,
          accessToken,
          refreshToken,
        } = response.data;

        // Auth store'u güncelle
        setAuth(
          { userId, email, firstName, lastName },
          accessToken,
          refreshToken
        );

        // Başarı bildirimi
        toast.success("Giriş başarılı!", {
          description: `Hoş geldin, ${firstName}!`,
        });

        // Dashboard'a yönlendir
        navigate("/dashboard");
      } else {
        // Backend'den hata mesajı
        toast.error("Giriş başarısız", {
          description: response.message || "Bir hata oluştu",
        });
      }
    },
    onError: (error: any) => {
      // Network veya diğer hatalar
      const errorMessage = error.response?.data?.message || "Sunucu hatası";
      toast.error("Giriş başarısız", {
        description: errorMessage,
      });
    },
  });
}

/**
 * Register hook
 */
export function useRegister() {
  const navigate = useNavigate();

  return useMutation({
    mutationFn: (data: RegisterRequest) => authService.register(data),
    onSuccess: (response) => {
      if (response.isSuccess) {
        toast.success("Kayıt başarılı!", {
          description: "Şimdi giriş yapabilirsiniz.",
        });

        // Login sayfasına yönlendir
        navigate("/login");
      } else {
        toast.error("Kayıt başarısız", {
          description: response.message || "Bir hata oluştu",
        });
      }
    },
    onError: (error: any) => {
      const errorMessage = error.response?.data?.message || "Sunucu hatası";

      // Validation hataları varsa
      const errors = error.response?.data?.errors;
      if (errors && errors.length > 0) {
        errors.forEach((err: string) => {
          toast.error("Kayıt başarısız", { description: err });
        });
      } else {
        toast.error("Kayıt başarısız", { description: errorMessage });
      }
    },
  });
}

/**
 * Logout hook
 */
export function useLogout() {
  const navigate = useNavigate();
  const { clearAuth } = useAuthStore();

  return () => {
    clearAuth();
    toast.info("Çıkış yapıldı");
    navigate("/login");
  };
}
