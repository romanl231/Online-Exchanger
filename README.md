# Online Exchanger

An online currency exchange platform built with a modern full-stack setup. The goal of this project is to provide a clean and efficient solution for simulating currency exchange between users — with admin management and real-time updates planned.

## 🛠 Tech Stack

### Backend  
- **ASP.NET Core** (Web API)  
- **SQL Server Express**  
- **Swagger / OpenAPI**  
- **xUnit** & **FluentAssertions** (unit testing)  

### Frontend  
- **React** (TypeScript + Vite)  
- **Material UI**  
- Designed in **Figma**

## 🚀 Features (Planned / In Progress)
- User authentication and profile management  
- Currency exchange request flow  
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

Navigate to `https://localhost:<port>/swagger` to test API endpoints.

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

The UI is designed in Figma. You can view the design [https://www.figma.com/design/rVCxCXNpINRi4OgkEIF3FK/Exchanger?node-id=0-1&p=f&t=UezbtG8bryRt3GPO-0](#)  
<!-- TODO: Replace # with a public Figma link if available -->

## 📦 Database

Make sure you have **SQL Server Express** installed.  
Connection string is located in `appsettings.Development.json` (feel free to change it as needed).

## 🤝 Contribution

This is a personal project in progress. Feedback, ideas, and suggestions are welcome!  
PRs might be considered in the future after the MVP is complete.

---

## 📌 Status

> ⚙️ MVP in development – current focus: core backend features and basic frontend flows.
```
