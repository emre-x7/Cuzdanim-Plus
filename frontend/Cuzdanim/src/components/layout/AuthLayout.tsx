import { type ReactNode } from "react";

interface AuthLayoutProps {
  children: ReactNode;
}

export default function AuthLayout({ children }: AuthLayoutProps) {
  return (
    <div className="min-h-screen flex">
      {/* Sol Taraf - Branding/İllüstrasyon */}
      <div className="hidden lg:flex lg:w-1/2 bg-gradient-to-br from-primary to-primary/80 p-12 flex-col justify-between">
        <div>
          {/* Logo */}
          <div className="flex items-center gap-2 text-white">
            <div className="w-10 h-10 bg-white/20 rounded-lg flex items-center justify-center">
              <span className="text-2xl font-bold">₺</span>
            </div>
            <span className="text-2xl font-bold">Cüzdanım+</span>
          </div>
        </div>

        {/* Orta - Açıklama */}
        <div className="text-white">
          <h1 className="text-4xl font-bold mb-4">
            Finansal Hayatınızı <br />
            Kontrol Altına Alın
          </h1>
          <p className="text-lg text-white/80">
            Hesaplarınızı, harcamalarınızı ve hedeflerinizi tek bir platformda
            yönetin. Türkiye'ye özel finans yönetimi çözümü.
          </p>
        </div>

        {/* Alt - Features */}
        <div className="space-y-3 text-white/90">
          <div className="flex items-center gap-2">
            <div className="w-6 h-6 bg-white/20 rounded-full flex items-center justify-center">
              ✓
            </div>
            <span>Akıllı bütçeleme ve harcama takibi</span>
          </div>
          <div className="flex items-center gap-2">
            <div className="w-6 h-6 bg-white/20 rounded-full flex items-center justify-center">
              ✓
            </div>
            <span>Hedef odaklı tasarruf planları</span>
          </div>
          <div className="flex items-center gap-2">
            <div className="w-6 h-6 bg-white/20 rounded-full flex items-center justify-center">
              ✓
            </div>
            <span>Detaylı raporlar ve analizler</span>
          </div>
        </div>
      </div>

      {/* Sağ Taraf - Form */}
      <div className="flex-1 flex items-center justify-center p-8 bg-background">
        <div className="w-full max-w-md">{children}</div>
      </div>
    </div>
  );
}
