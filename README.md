appsettings.example.json

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=;Database=;User Id=;Password=;"
  },
  "JwtSettings": {
    "ClaveSecreta": "REEMPLAZAR_POR_SECRETO",
    "Emisor": "PapeleriaWebApi",
    "Audiencia": "PapeleriaCliente",
    "ExpiracionMinutos": 60
  },
  "SmtpSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "EnableSsl": false,
    "UseStartTls": true,
    "User": "correo@dominio.com",
    "Password": "REEMPLAZAR"
  },
  "BackendSettings": {
    "BaseUrl": "http://localhost:7018"
  }
}
