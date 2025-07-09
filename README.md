# Pathly Backend

Este repositorio contiene el **backend** de la plataforma Pathly, una aplicación de test vocacional y acompañamiento psicológico. Está organizada en varios *bounded contexts* siguiendo principios de DDD:

* **IAM**: Autenticación y autorización. Registra usuarios, gestiona JWT, roles (Admin, Psychologist, Student).
* **Profile**: Información de perfil de usuario (datos personales, colegio/facultad).
* **Sessions**: Reserva y gestión de sesiones vocacionales. Incluye chat en tiempo real con SignalR, feedback de sesiones, reportes de malas conductas.
* **Analytics**: Estadísticas y métricas globales para el dashboard de Admin (usuarios por rol, sesiones por estado, tiempo medio de confirmación, total de reportes y feedback).
* **SanctionsAndAppeals**: Sistema de sanciones (baneos) y apelaciones, con endpoints para imponer, revocar sanciones y gestionar solicitudes de apelación.

---

## 🔧 Requisitos

* .NET 8
* MySQL 8+ (cadena de conexión en `appsettings.json`)

## 🚀 Configuración y ejecución

1. **Clonar** el repositorio

   ```bash
   git clone https://github.com/tu-org/pathly-backend.git
   cd pathly-backend
   ```

2. **Actualizar cadena de conexión** en `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "Default": "server=...;port=3306;database=pathly;uid=...;pwd=...;"
   }
   ```

---

## Estructura de carpetas

```
src/
├─ IAM/
├─ Profile/
├─ Sessions/
├─ Analytics/
├─ SanctionsAndAppeals/
└─ Program.cs
```

Cada contexto incluye:

* **Domain**: Entidades, enums, repositorios.
* **Infrastructure**: DbContext, EF Core mapping, repositorios.
* **Application**: DTOs, servicios, interfaces.
* **Interfaces/REST** o **Interfaces/SignalR**: Controladores y hubs.

---

## Autenticación y autorización

* Se usa **JWT Bearer**.
* Endpoints protegidos con `[Authorize]`.
* Roles: `Student`, `Psychologist`, `Admin`.
* Configuración del esquema JWT en `Program.cs`.

---

## Características principales

### Sesiones (`/api/sessions`)

* Reservar, confirmar, cancelar y finalizar sesiones.
* Envío de notificaciones internas.
* Chat en tiempo real con SignalR y persistencia de mensajes.
* Feedback de sesiones y reportes de mala conducta.

### Estadísticas (`/api/admin/stats`)

* Total de usuarios por rol.
* Sesiones por estado.
* Tiempo medio entre reserva y confirmación.
* Totales de reportes y feedback.

### Sanciones y apelaciones

* Crear sanciones temporales o permanentes (`/api/sanctions`).
* Obtener sanción activa del usuario (`/api/sanctions/me`).
* Apelar sanciones (`/api/sanctions/{id}/appeal`).
* Listar y resolver apelaciones (`/api/admin/appeals`, `/api/appeals/{id}/resolve`).
* **Middleware global** bloquea rutas para usuarios sancionados.

---

## 🧪 Testing [TO-DO]

* **Unit tests** para servicios y validaciones (en carpeta `tests/`).
* **Integration tests** con `WebApplicationFactory` y base en memoria.
* **SignalR tests** usando el transport en memoria.