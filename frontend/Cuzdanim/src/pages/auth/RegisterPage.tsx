import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Link } from "react-router-dom";
import { Loader2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { useRegister } from "@/hooks/useAuth";
import { registerSchema, type RegisterFormData } from "@/lib/validations";

export default function RegisterPage() {
  const registerMutation = useRegister();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
    defaultValues: {
      email: "",
      password: "",
      firstName: "",
      lastName: "",
      phoneNumber: "",
      dateOfBirth: "",
    },
  });

  const onSubmit = (data: RegisterFormData) => {
    // Boş optional field'ları undefined'a çevir
    const payload = {
      ...data,
      phoneNumber: data.phoneNumber || undefined,
      dateOfBirth: data.dateOfBirth || undefined,
    };

    registerMutation.mutate(payload);
  };

  return (
    <Card className="w-full">
      <CardHeader>
        <CardTitle className="text-2xl">Kayıt Ol</CardTitle>
        <CardDescription>
          Yeni bir hesap oluşturmak için bilgilerinizi girin
        </CardDescription>
      </CardHeader>

      <form onSubmit={handleSubmit(onSubmit)}>
        <CardContent className="space-y-4">
          {/* Ad Soyad - Grid */}
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="firstName">Ad</Label>
              <Input
                id="firstName"
                placeholder="Ahmet"
                {...register("firstName")}
                disabled={registerMutation.isPending}
              />
              {errors.firstName && (
                <p className="text-sm text-destructive">
                  {errors.firstName.message}
                </p>
              )}
            </div>

            <div className="space-y-2">
              <Label htmlFor="lastName">Soyad</Label>
              <Input
                id="lastName"
                placeholder="Yılmaz"
                {...register("lastName")}
                disabled={registerMutation.isPending}
              />
              {errors.lastName && (
                <p className="text-sm text-destructive">
                  {errors.lastName.message}
                </p>
              )}
            </div>
          </div>

          {/* Email */}
          <div className="space-y-2">
            <Label htmlFor="email">Email</Label>
            <Input
              id="email"
              type="email"
              placeholder="ornek@email.com"
              {...register("email")}
              disabled={registerMutation.isPending}
            />
            {errors.email && (
              <p className="text-sm text-destructive">{errors.email.message}</p>
            )}
          </div>

          {/* Şifre */}
          <div className="space-y-2">
            <Label htmlFor="password">Şifre</Label>
            <Input
              id="password"
              type="password"
              placeholder="••••••••"
              {...register("password")}
              disabled={registerMutation.isPending}
            />
            {errors.password && (
              <p className="text-sm text-destructive">
                {errors.password.message}
              </p>
            )}
            <p className="text-xs text-muted-foreground">
              En az 8 karakter, büyük/küçük harf, rakam ve özel karakter
              içermelidir
            </p>
          </div>

          {/* Telefon (Opsiyonel) */}
          <div className="space-y-2">
            <Label htmlFor="phoneNumber">
              Telefon <span className="text-muted-foreground">(Opsiyonel)</span>
            </Label>
            <Input
              id="phoneNumber"
              type="tel"
              placeholder="5551234567"
              {...register("phoneNumber")}
              disabled={registerMutation.isPending}
            />
            {errors.phoneNumber && (
              <p className="text-sm text-destructive">
                {errors.phoneNumber.message}
              </p>
            )}
          </div>

          {/* Doğum Tarihi (Opsiyonel) */}
          <div className="space-y-2">
            <Label htmlFor="dateOfBirth">
              Doğum Tarihi{" "}
              <span className="text-muted-foreground">(Opsiyonel)</span>
            </Label>
            <Input
              id="dateOfBirth"
              type="date"
              {...register("dateOfBirth")}
              disabled={registerMutation.isPending}
            />
            {errors.dateOfBirth && (
              <p className="text-sm text-destructive">
                {errors.dateOfBirth.message}
              </p>
            )}
          </div>
        </CardContent>

        <CardFooter className="flex flex-col gap-4">
          <Button
            type="submit"
            className="w-full"
            disabled={registerMutation.isPending}
          >
            {registerMutation.isPending ? (
              <>
                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                Kayıt yapılıyor...
              </>
            ) : (
              "Kayıt Ol"
            )}
          </Button>

          <p className="text-sm text-center text-muted-foreground">
            Zaten hesabınız var mı?{" "}
            <Link
              to="/login"
              className="font-medium text-primary hover:underline"
            >
              Giriş Yap
            </Link>
          </p>
        </CardFooter>
      </form>
    </Card>
  );
}
