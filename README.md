# Cüzdanım+ | Kişisel Finans Yönetimi

Modern, güvenli ve kullanıcı dostu kişisel finans yönetim uygulaması.

---

## 🚀 Özellikler

- **Hesap Yönetimi:** Banka hesapları, kredi kartları, nakit
- **İşlem Takibi:** Gelir ve gider kayıtları
- **Bütçe Planlama:** Kategori bazlı bütçe kontrolü
- **Hedef Belirleme:** Finansal hedefler ve katkı takibi
- **Raporlama:** Grafikler ve detaylı analizler
- **Dashboard:** Finansal durum özeti ve analizler

---

## 🛠️ Teknolojiler

### **Backend:**
- .NET 9
- Entity Framework Core 9
- PostgreSQL 17
- JWT Authentication
- Clean Architecture
- CQRS Pattern
- FluentValidation
- AutoMapper

### **Frontend:**
- React 18
- TypeScript 5
- Vite 5
- TanStack Query (React Query)
- React Hook Form + Zod
- shadcn/ui + Tailwind CSS
- Recharts
- Axios
- Lucide Icons

---

## 📋 Gereksinimler

### **Backend:**
- .NET 9 SDK
- PostgreSQL 17+
- Visual Studio 2022 veya VS Code

### **Frontend:**
- Node.js 18+ (LTS)
- npm, yarn veya pnpm

---

## 🔧 Backend Kurulumu

### **1. Repository'yi Clone'la:**
```bash
git clone https://github.com/emre-x7/Cuzdanim-Plus.git
cd cuzdanim-plus/backend
```

### **2. PostgreSQL Veritabanı Oluştur:**
```sql
CREATE DATABASE CuzdanimDB;
```

### **3. Connection String'i Güncelle:**
`Cuzdanim.API/appsettings.json` dosyasında:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=CuzdanimDB;Username=postgres;Password=yourpassword"
}
```

### **4. Migration Çalıştır:**
```bash
cd Cuzdanim.API
dotnet ef database update
```

### **5. Uygulamayı Çalıştır:**
```bash
dotnet run
```

### **6. Swagger'ı Aç:**
```
https://localhost:7168/swagger
```

---

## 🔧 Frontend Kurulumu

### **1. Frontend Dizinine Git:**
```bash
cd cuzdanim-plus/frontend
```

### **2. Dependencies'leri Yükle:**
```bash
npm install
# veya
yarn install
# veya
pnpm install
```

### **3. Environment Dosyası Oluştur:**
`.env` dosyası oluştur:
```env
VITE_API_URL=https://localhost:7168/api/v1
```

### **4. Development Server'ı Başlat:**
```bash
npm run dev
# veya
yarn dev
# veya
pnpm dev
```

### **5. Tarayıcıda Aç:**
```
http://localhost:5173
```

```
## 📚 API Endpoint'leri

### **Authentication**
- `POST /api/v1/auth/register` - Kayıt ol
- `POST /api/v1/auth/login` - Giriş yap
- `POST /api/v1/auth/refresh-token` - Token yenile

### **Accounts**
- `GET /api/v1/accounts` - Hesapları listele
- `POST /api/v1/accounts` - Hesap oluştur
- `PUT /api/v1/accounts/{id}` - Hesap güncelle
- `DELETE /api/v1/accounts/{id}` - Hesap sil

### **Transactions**
- `GET /api/v1/transactions` - İşlemleri listele
- `GET /api/v1/transactions/{id}` - İşlem detayı
- `POST /api/v1/transactions` - İşlem oluştur
- `PUT /api/v1/transactions/{id}` - İşlem güncelle
- `DELETE /api/v1/transactions/{id}` - İşlem sil

### **Budgets**
- `GET /api/v1/budgets` - Bütçeleri listele
- `GET /api/v1/budgets/{id}` - Bütçe detayı
- `POST /api/v1/budgets` - Bütçe oluştur
- `PUT /api/v1/budgets/{id}` - Bütçe güncelle
- `DELETE /api/v1/budgets/{id}` - Bütçe sil

### **Goals**
- `GET /api/v1/goals` - Hedefleri listele
- `GET /api/v1/goals/{id}` - Hedef detayı
- `POST /api/v1/goals` - Hedef oluştur
- `PUT /api/v1/goals/{id}` - Hedef güncelle
- `POST /api/v1/goals/{id}/contribute` - Hedefe katkı ekle
- `DELETE /api/v1/goals/{id}` - Hedef sil

### **Reports**
- `GET /api/v1/reports` - Finansal raporlar ve analizler

### **Dashboard**
- `GET /api/v1/dashboard` - Finansal özet

```

```

### 🏗️ Proje Yapısı

### **Backend:**

Cuzdanim/
├── Cuzdanim.Domain/          # Domain katmanı (Entities, Value Objects, Enums)
├── Cuzdanim.Application/     # Application katmanı (CQRS, Validation, DTOs)
├── Cuzdanim.Infrastructure/  # Infrastructure katmanı (EF Core, Repositories)
└── Cuzdanim.API/            # API katmanı (Controllers, Middlewares)


### **Frontend:**

frontend/
├── src/
│   ├── api/                 # API services (axios)
│   ├── components/          # React components
│   │   ├── layout/         # Layout components (Sidebar, Header)
│   │   ├── accounts/       # Account components
│   │   ├── transactions/   # Transaction components
│   │   ├── budgets/        # Budget components
│   │   ├── goals/          # Goal components
│   │   ├── reports/        # Report components (charts)
│   │   └── ui/             # shadcn/ui components
│   ├── contexts/           # React Context (Auth)
│   ├── hooks/              # Custom hooks (useAuth, useAccounts, etc.)
│   ├── lib/                # Utilities (axios, validations, utils)
│   ├── pages/              # Page components
│   ├── types/              # TypeScript types
│   ├── App.tsx             # Main app component
│   └── main.tsx            # Entry point
├── public/                 # Static files
├── index.html              # HTML template
├── tailwind.config.js      # Tailwind configuration
├── tsconfig.json           # TypeScript configuration
└── vite.config.ts          # Vite configuration
```



## 🎨 Frontend Sayfalar

### **Public Pages:**
- `/login` - Giriş sayfası
- `/register` - Kayıt sayfası

### **Protected Pages:**
- `/dashboard` - Ana sayfa (özet kartlar, grafikler, widget'lar)
- `/accounts` - Hesap yönetimi
- `/transactions` - İşlem yönetimi
- `/budgets` - Bütçe yönetimi ve takibi
- `/goals` - Hedef belirleme ve katkı ekleme
- `/reports` - Grafikler ve detaylı analizler


## 📊 Frontend Özellikleri

### **UI/UX:**
- ✅ Responsive tasarım (mobile-first)
- ✅ Loading states
- ✅ Empty states
- ✅ Error handling
- ✅ Toast notifications
- ✅ Modal forms
- ✅ Confirmation dialogs

### **Data Visualization:**
- 📊 Bar charts (Gelir vs Gider)
- 🥧 Pie charts (Kategori dağılımı)
- 📈 Line charts (Aylık trend)
- 📉 Progress bars (Bütçe/Hedef tracking)

### **State Management:**
- TanStack Query (server state)
- React Context (auth state)
- React Hook Form (form state)

### **Validation:**
- Client-side: Zod schemas
- Server-side: FluentValidation


## 🔐 Güvenlik

### **Backend:**
- JWT Token tabanlı authentication
- Şifreleme: BCrypt
- CORS politikaları
- SQL Injection koruması (EF Core)

### **Frontend:**
- JWT token localStorage'da saklanır
- Protected routes (token kontrolü)
- Otomatik token refresh
- XSS koruması (React default)
- CSRF token (planlanan)

```
🧪 Test Kullanıcısı (Development)
Email: test.user@test.com
Şifre: Test1234!
