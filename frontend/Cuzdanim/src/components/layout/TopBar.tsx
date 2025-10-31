import { Bell, Menu } from "lucide-react";
import { Button } from "@/components/ui/button";

export default function TopBar() {
  return (
    <header className="sticky top-0 z-30 bg-background border-b border-border">
      <div className="flex items-center justify-between h-16 px-6">
        {/* Sol - Mobil Menu (gelecekte eklenecek) */}
        <Button variant="ghost" size="icon" className="lg:hidden">
          <Menu className="w-5 h-5" />
        </Button>

        {/* Orta - Boş (ileride breadcrumb eklenebilir) */}
        <div className="flex-1" />

        {/* Sağ - Bildirimler */}
        <div className="flex items-center gap-2">
          <Button variant="ghost" size="icon" className="relative">
            <Bell className="w-5 h-5" />
            {/* Bildirim badge (örnek) */}
            <span className="absolute top-1 right-1 w-2 h-2 bg-destructive rounded-full" />
          </Button>
        </div>
      </div>
    </header>
  );
}
