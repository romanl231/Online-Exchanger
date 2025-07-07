# Online Exchanger

An online currency exchange platform built with a modern full-stack setup. The goal of this project is to provide a clean and efficient solution for simulating currency exchange between users — with admin management and real-time updates planned.

## 🛠 Tech Stack

### Backend  
- **ASP.NET Core** (Web API, RESTful)  
- **SQL Server Express**  
- **JWT Authentication + Refresh Tokens**  
- **Swagger / OpenAPI**  
- **xUnit** & **FluentAssertions** (unit testing)  

### Frontend  
- **React** (TypeScript + Vite)  
- **Material UI**  
- Designed in **Figma**

## 🚀 Features (Planned / In Progress)
- User authentication with **JWT + Refresh Token flow**  
- Profile and account management  
- Currency exchange request system  
- Admin panel for managing rates and requests  
- Real-time exchange rate updates  
- Transaction history  
- Responsive frontend with modern UI/UX  

## 🔧 How to Run

### Backend  
```bash
cd OnlineExchanger.Api
dotnet build
dotnet run
```

Navigate to `https://localhost:7183/swagger` to test API endpoints.

### Frontend  
```bash
cd online-exchanger-frontend
npm install
npm run dev
```

Frontend will run on `http://localhost:5173` by default.

## 🧪 Testing

```bash
cd OnlineExchanger.Tests
dotnet test
```

Unit tests use **xUnit** + **FluentAssertions** to ensure business logic integrity.

## 🎨 UI/UX Design

The UI is designed in Figma. You can view the design [here](https://www.figma.com/design/rVCxCXNpINRi4OgkEIF3FK/Exchanger?node-id=0-1&p=f&t=UezbtG8bryRt3GPO-0)  

## 📦 Database

Make sure you have **SQL Server Express** installed.  
Connection string is located in `appsettings.Development.json` (feel free to change it as needed).

## 🔐 Authentication

This project uses **JWT access tokens** with a **refresh token mechanism** for secure and scalable user sessions.  
All endpoints are structured following **REST API** best practices.

---

## 📘 API Overview

### 🔑 Authorization
- `POST /api/auth/login`
- `POST /api/auth/logout`
- `GET /api/auth/me`
- `POST /api/auth/refresh-jwt`

### 👤 User
- `POST /api/user/register`
- `GET /api/user/me`
- `PATCH /api/user/update`
- `PATCH /api/user/change-avatar`
- `POST /api/user/send-verification-email`
- `POST /api/user/verify-email/{token}`

### 📄 Listings
- `POST /api/listing/create`
- `POST /api/listing/search/by-params`
- `POST /api/listing/search/by-title`
- `POST /api/listing/user-{userId}/listings`
- `DELETE /api/listing/{listingId}/delete`
- `PATCH /api/listing/{listingId}/activate`
- `PATCH /api/listing/{listingId}/deactivate`

### 🗂️ Listing Categories
- `POST /api/listing/category/listing-{listingId}/add`
- `DELETE /api/listing/category/listing-{listingId}/{categoryId}`
- `GET /api/listing/category/all`

### 🖼️ Listing Images
- `POST /api/listing/image/{listingId}/image/add`
- `DELETE /api/listing/image/{listingId}/image/delete/{avatarUrl}`

## 📌 Status

> 🧪 **MVP in development**  
> 🎯 Current focus:  
> - Profile management  
> - Listing management (CRUD)  
> - Filtered search  
> - JWT auth with refresh  
> - RESTful API consistency
