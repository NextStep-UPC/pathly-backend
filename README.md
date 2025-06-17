# Pathly Backend - Develop (Rama de testeo)

Este es el backend de Pathly, construido con ASP.NET Core 8, Entity Framework Core y MySQL. Implementa autenticación JWT y está estructurado siguiendo DDD.

---

## 🚀 Tecnologías principales

- ASP.NET Core (.NET 8)
- Entity Framework Core
- MySQL
- JWT Authentication
- Swagger para documentación
- DDD (Domain-Driven Design)

---

## ⚙️ Configuración rápida

### 1. Clonar el repositorio

```
bash
git clone https://github.com/NextStep-UPC/pathly-backend.git
cd pathly-backen
```

### 2. Configurar `appsettings.json`

Edita el archivo `appsettings.json` y coloca tu cadena de conexi贸n:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=pathly_backend;user=root;password=tu_contraseña"
}
```

### 3. Presiona `F5` para iniciar el proyecto.

Asegúrate de que el servicio de MySQL esté en ejecución antes de iniciar el proyecto, de lo contrario, la aplicación no podrá conectarse a la base de datos y fallará al arrancar.
