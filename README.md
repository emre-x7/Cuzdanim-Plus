# Cüzdanım+ | Kişisel Finans Yönetimi

Modern, güvenli ve kullanıcı dostu kişisel finans yönetim uygulaması.

## 🚀 Özellikler

- **Hesap Yönetimi:** Banka hesapları, kredi kartları, nakit
- **İşlem Takibi:** Gelir ve gider kayıtları
- **Bütçe Planlama:** Kategori bazlı bütçe kontrolü
- **Hedef Belirleme:** Finansal hedefler ve katkı takibi
- **Dashboard:** Finansal durum özeti ve analizler

## 🛠️ Teknolojiler

**Backend:**
- .NET 9
- Entity Framework Core 9
- PostgreSQL 17
- JWT Authentication
- Clean Architecture
- CQRS Pattern
- FluentValidation
- AutoMapper

## 📋 Gereksinimler

- .NET 9 SDK
- PostgreSQL 17+
- Visual Studio 2022 veya VS Code

## 🔧 Kurulum

1. **Repository'yi Clone'la:**
```bash
git clone https://github.com/emre-x7/Cuzdanim-Plus.git
cd cuzdanim-plus/backend
```

2. **PostgreSQL Veritabanı Oluştur:**
```sql
CREATE DATABASE CuzdanimDB;
```

3. **Connection String'i Güncelle:**
`Cuzdanim.API/appsettings.json` dosyasında:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=CuzdanimDB;Username=postgres;Password=yourpassword"
}
```

4. **Migration Çalıştır:**
```bash
cd Cuzdanim.API
dotnet ef database update
```

5. **Uygulamayı Çalıştır:**
```bash
dotnet run
```

6. **Swagger'ı Aç:**
```
https://localhost:5001/swagger
```

## 📚 API Endpoint'leri

### Authentication
- `POST /api/v1/auth/register` - Kayıt ol
- `POST /api/v1/auth/login` - Giriş yap
- `POST /api/v1/auth/refresh-token` - Token yenile

### Accounts
- `GET /api/v1/accounts` - Hesapları listele
- `POST /api/v1/accounts` - Hesap oluştur
- `PUT /api/v1/accounts/{id}` - Hesap güncelle
- `DELETE /api/v1/accounts/{id}` - Hesap sil

### Transactions
- `GET /api/v1/transactions` - İşlemleri listele
- `GET /api/v1/transactions/{id}` - İşlem detayı
- `POST /api/v1/transactions` - İşlem oluştur
- `PUT /api/v1/transactions/{id}` - İşlem güncelle
- `DELETE /api/v1/transactions/{id}` - İşlem sil

### Budgets
- `GET /api/v1/budgets` - Bütçeleri listele
- `GET /api/v1/budgets/{id}` - Bütçe detayı
- `POST /api/v1/budgets` - Bütçe oluştur
- `PUT /api/v1/budgets/{id}` - Bütçe güncelle
- `DELETE /api/v1/budgets/{id}` - Bütçe sil

### Goals
- `GET /api/v1/goals` - Hedefleri listele
- `GET /api/v1/goals/{id}` - Hedef detayı
- `POST /api/v1/goals` - Hedef oluştur
- `PUT /api/v1/goals/{id}` - Hedef güncelle
- `POST /api/v1/goals/{id}/contribute` - Hedefe katkı ekle
- `DELETE /api/v1/goals/{id}` - Hedef sil

### Dashboard
- `GET /api/v1/dashboard` - Finansal özet

## 🏗️ Proje Yapısı
```
Cuzdanim/
├── Cuzdanim.Domain/          # Domain katmanı (Entities, Value Objects, Enums)
├── Cuzdanim.Application/     # Application katmanı (CQRS, Validation, DTOs)
├── Cuzdanim.Infrastructure/  # Infrastructure katmanı (EF Core, Repositories)
└── Cuzdanim.API/            # API katmanı (Controllers, Middlewares)
```

## 🔐 Güvenlik

- JWT Token tabanlı authentication
- Şifreleme: BCrypt
- HTTPS zorunlu (Production)
- CORS politikaları
- SQL Injection koruması (EF Core)
