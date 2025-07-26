
# SampleGameApiWithDotNetAzure

A sample ASP.NET Core Web API project designed for Unity games. This project showcases how to build secure, scalable game backend services using .NET 8 and prepare them for deployment on Azure.

---

## 🚀 Features

- Built with **.NET 8 Web API**
- Designed for integration with **Unity games**
- Score submission endpoint with DTO modeling
- Swagger UI enabled for easy API testing
- Clean folder structure with Models and DTOs
- Ready to integrate:
  - ✅ JWT Authentication
  - ✅ Rate Limiting
  - ✅ Logging
  - ✅ API Keys / OAuth
  - ✅ Azure App Service deployment

---

## 📁 Project Structure

```bash
SampleGameApiWithDotNetAzure/
├── Controllers/
│   └── ScoreController.cs
├── Models/
│   └── ScoreSubmissionDto.cs
├── Program.cs
├── SampleGameApiWithDotNetAzure.csproj
└── README.md
```

---

## 📦 Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/sindhusingh/SampleGameApiWithDotNetAzure.git
   cd SampleGameApiWithDotNetAzure
   ```

2. **Run the API**
   ```bash
   dotnet run
   ```

3. **Test the API using Swagger**
   Open your browser at [http://localhost:5063/swagger](http://localhost:5063/swagger)

---

## 🧪 Sample API Endpoint

**POST** `/submit-score`

Request Body:
```json
{
  "playerId": "player123",
  "score": 1500
}
```

---

## 🛡️ Coming Soon

- Authentication with **PlayFab tokens**
- Middleware for **rate limiting**
- API logging with **Serilog**
- Deployment to **Azure App Service**
- Environment-specific configs
- CI/CD pipeline setup

---

## 🧠 Why This Project?

This is a foundational backend for Unity game developers looking to:
- Learn .NET Web API with real use cases
- Secure APIs using industry standards
- Host backend on Azure for global access
- Extend backend with analytics, leaderboards, and more

---

## 📜 License

MIT License. Feel free to use and extend this project in your own Unity game backend.
