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

```
🧪 Test Kullanıcısı (Development)
Email: test.user@test.com
Şifre: Test1234!
```

## 📱 Uygulama Akışlarının Görsel Sunumu

Web uygulamamızın temel akışlarını ve kullanıcı arayüzü tasarımını aşağıda inceleyebilirsiniz. Görseller, projenin kapsamını ve kullanıcı deneyimi odaklı yaklaşımını göstermektedir.

### 1. Giriş ve Hesap Yönetimi Akışları

Giriş, kayıt ve temel hesap yönetimi formları.

<p align="center">
  <img src="https://github.com/user-attachments/assets/326a8ad7-844e-4eb1-a82e-09d0864c1951" width="700" alt="Kayıt Olma Sayfası">
</p>
<p align="center">
  <img src="https://github.com/user-attachments/assets/e638ab80-abfc-48bc-9218-000ea29d6405" width="700" alt="Giriş Yapma Sayfası">
</p>
<p align="center">
  <img src="https://github.com/user-attachments/assets/785189c1-93ad-4b5b-9a67-dc09d8f73b11" width="850" alt="Tüm Finansal Hesapların Yönetimi Sayfası">
</p>
<p align="center">
  <img src="https://github.com/user-attachments/assets/7ddc7774-b54e-4e2c-aece-8cb4e0a4a5a4" width="700" alt="Yeni Hesap Ekleme Formu">
</p>

---

### 2. Ana Kontrol Paneli ve İşlem Akışı

Uygulamanın ana ekranı, varlıkların özeti ve işlem takibi.

<p align="center">
  <img src="https://github.com/user-attachments/assets/654a60f1-8308-4b1b-9a48-4896031b54dd" width="900" alt="Ana Kontrol Paneli ve Özet Göstergeler">
</p>
<p align="center">
  <img src="https://github.com/user-attachments/assets/f08830a9-4288-46c6-a264-34882574ae50" width="900" alt="Gelir ve Gider İşlemleri Geçmişi Sayfası">
</p>
<p align="center">
  <img src="https://github.com/user-attachments/assets/8857f1f4-864a-4af1-be4a-6734e2104aaa" width="700" alt="Yeni İşlem Ekleme Formu">
</p>

---

### 3. Bütçeler ve Finansal Hedefler

Kullanıcıların uzun vadeli finansal planlama ve takibi.

<p align="center">
  <img src="https://github.com/user-attachments/assets/5890ed09-ef74-4c79-b3f8-2114c762c1f7" width="900" alt="Bütçe Yönetimi ve Takip Sayfası">
</p>
<p align="center">
  <img src="https://github.com/user-attachments/assets/0f2a1ca4-68f7-4c37-a925-1ae1e9a4d08d" width="700" alt="Yeni Bütçe Oluşturma Formu">
</p>
<p align="center">
  <img src="https://github.com/user-attachments/assets/4a30c9e5-713c-4302-bf26-6fefed231f43" width="900" alt="Finansal Hedef Belirleme ve Takip Sayfası">
</p>
<p align="center">
  <img src="https://github.com/user-attachments/assets/8f829c6f-4c64-4c73-b360-7cb986169b0e" width="700" alt="Yeni Finansal Hedef Oluşturma Formu">
</p>

---

### 4. Raporlar ve Analitik Görünümler

Kullanıcının bilinçli finansal davranışlar geliştirmesini destekleyen analizler.

<p align="center">
  <img src="https://github.com/user-attachments/assets/4154c320-848a-410c-8a88-de91739f7841" width="900" alt="Detaylı Harcama Dağılım Raporu">
</p>
<p align="center">
  <img src="https://github.com/user-attachments/assets/b2decf72-af1a-4824-b9e6-5ece257fb562" width="900" alt="Trend ve Karşılaştırma Analiz Raporu">
</p>
<p align="center">
  <img src="https://github.com/user-attachments/assets/1a1d81a3-96a8-4aca-bca3-a169c33cfd71" width="900" alt="Gelir-Gider Akış Raporu">
</p>

