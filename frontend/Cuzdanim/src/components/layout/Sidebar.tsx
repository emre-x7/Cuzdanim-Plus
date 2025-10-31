import { Link, useLocation } from "react-router-dom";
import { cn } from "@/lib/utils";
import {
  LayoutDashboard,
  Wallet,
  ArrowLeftRight,
  PiggyBank,
  Target,
  BarChart3,
  LogOut,
} from "lucide-react";
import { useAuthStore } from "@/store/authStore";

const menuItems = [
  {
    title: "Dashboard",
    href: "/dashboard",
    icon: LayoutDashboard,
  },
  {
    title: "Hesaplarım",
    href: "/accounts",
    icon: Wallet,
  },
  {
    title: "İşlemler",
    href: "/transactions",
    icon: ArrowLeftRight,
  },
  {
    title: "Bütçeler",
    href: "/budgets",
    icon: PiggyBank,
  },
  {
    title: "Hedefler",
    href: "/goals",
    icon: Target,
  },

  {
    title: "Raporlar",
    href: "/reports",
    icon: BarChart3,
  },
];

export default function Sidebar() {
  const location = useLocation();
  const { clearAuth, user } = useAuthStore();

  const handleLogout = () => {
    clearAuth();
    // Router otomatik olarak /login'e yönlendirecek (ProtectedRoute sayesinde)
  };

  return (
    <aside className="fixed top-0 left-0 z-40 w-64 h-screen transition-transform -translate-x-full lg:translate-x-0 bg-card border-r border-border">
      <div className="h-full flex flex-col">
        {/* Logo */}
        <div className="p-6 border-b border-border">
          <div className="flex items-center gap-2">
            <div className="w-10 h-10 bg-primary rounded-lg flex items-center justify-center text-white">
              <span className="text-xl font-bold">₺</span>
            </div>
            <div>
              <h1 className="text-lg font-bold text-foreground">Cüzdanım+</h1>
              <p className="text-xs text-muted-foreground">Finans Yönetimi</p>
            </div>
          </div>
        </div>

        {/* User Info */}
        <div className="px-6 py-4 border-b border-border">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 bg-primary/10 rounded-full flex items-center justify-center">
              <span className="text-sm font-semibold text-primary">
                {user?.firstName?.charAt(0)}
                {user?.lastName?.charAt(0)}
              </span>
            </div>
            <div className="flex-1 min-w-0">
              <p className="text-sm font-medium text-foreground truncate">
                {user?.firstName} {user?.lastName}
              </p>
              <p className="text-xs text-muted-foreground truncate">
                {user?.email}
              </p>
            </div>
          </div>
        </div>

        {/* Navigation */}
        <nav className="flex-1 p-4 space-y-1 overflow-y-auto">
          {menuItems.map((item) => {
            const isActive = location.pathname === item.href;
            const Icon = item.icon;

            return (
              <Link
                key={item.href}
                to={item.href}
                className={cn(
                  "flex items-center gap-3 px-4 py-3 rounded-lg text-sm font-medium transition-colors",
                  isActive
                    ? "bg-primary text-primary-foreground"
                    : "text-muted-foreground hover:bg-accent hover:text-accent-foreground"
                )}
              >
                <Icon className="w-5 h-5" />
                <span>{item.title}</span>
              </Link>
            );
          })}
        </nav>

        {/* Bottom Actions */}
        <div className="p-4 border-t border-border space-y-1">
          <button
            onClick={handleLogout}
            className="w-full flex items-center gap-3 px-4 py-3 rounded-lg text-sm font-medium text-muted-foreground hover:bg-destructive/10 hover:text-destructive transition-colors"
          >
            <LogOut className="w-5 h-5" />
            <span>Çıkış Yap</span>
          </button>
        </div>
      </div>
    </aside>
  );
}
