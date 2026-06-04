# 🏦 Banco Sistema - API REST Bancaria

> Sistema bancario empresarial desarrollado con **ASP.NET Core 8.0** y **Oracle Database**, containerizado con Docker para deployments escalables y reproducibles.

![.NET 8.0](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=.net)
![Oracle Database](https://img.shields.io/badge/Oracle-Database-F80000?style=flat-square&logo=oracle)
![Docker](https://img.shields.io/badge/Docker-Containerized-2496ED?style=flat-square&logo=docker)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)

---

## 📋 Descripción del Proyecto

**Banco Sistema** es una solución completa de banca digital que proporciona:

### 🎯 Funcionalidades Principales

- **👥 Gestión de Clientes** - Registro, perfiles y autenticación segura (JWT)
- **💳 Administración de Cuentas** - Cuentas corrientes con saldos en tiempo real
- **🔄 Transacciones Bancarias** - Transferencias, depósitos, retiros con auditoría completa
- **💰 Sistema de Préstamos** - Solicitud, aprobación y gestión de cuotas
- **💳 Tarjetas de Crédito/Débito** - Emisión, gestión de límites y estados
- **🏢 Gestión de Sucursales** - Múltiples sucursales con asignación de cajeros
- **💼 Control de Cajeros** - Gestión de caja y disponibilidad de efectivo
- **📊 Auditoría y Reportes** - Trazabilidad completa de operaciones

### 🏗️ Arquitectura

```
┌─────────────────────────────────────────────┐
│         Frontend (ASP.NET Core MVC)         │
│        http://localhost:5000                │
└────────────────────┬────────────────────────┘
                     │
                     │ HTTP/REST
                     ↓
┌─────────────────────────────────────────────┐
│    API REST (ASP.NET Core 8.0)              │
│  - Controllers (11 módulos)                 │
│  - Services (Lógica de negocio)             │
│  - Repositories (Acceso a datos)            │
│  - DTOs (Transferencia de datos)            │
│  - Autenticación JWT                        │
│  http://localhost:8085                      │
└────────────────────┬────────────────────────┘
                     │
                     │ Oracle Client
                     ↓
┌─────────────────────────────────────────────┐
│    Oracle Database XE (Containerizado)      │
│  - 10 Tablas normalizadas                   │
│  - 10 Secuencias para PKs                   │
│  - Constraints y Foreign Keys               │
│  - Triggers de auditoría                    │
│  localhost:1521/XEPDB1                      │
└─────────────────────────────────────────────┘
```

---

## 🚀 Inicio Rápido

### Requisitos Previos

- **Docker Desktop** - [Descargar](https://www.docker.com/products/docker-desktop)
- **.NET 8.0 SDK** - [Descargar](https://dotnet.microsoft.com/download)
- **Git** - (Opcional, para clonar el repositorio)

### Instalación y Ejecución (3 minutos)

#### 1️⃣ **Clonar el Repositorio**
```bash
git clone https://github.com/tuusuario/banco-sistema.git
cd banco-sistema
```

#### 2️⃣ **Ejecutar Contenedores (API + Base de Datos)**
```powershell
# Desde la raíz del proyecto
docker-compose up -d

# Verificar que todo esté corriendo
docker-compose ps

# Esperar a que Oracle sea "healthy" (2-3 minutos)
```

#### 3️⃣ **Ejecutar Frontend en Terminal Separada**
```powershell
cd BancoWeb\BancoWeb
dotnet restore
dotnet watch

# Se abrirá automáticamente en http://localhost:5000
```

#### 4️⃣ **Verificar que Está Funcionando**
- 🌐 **Frontend**: http://localhost:5000
- 📚 **API Swagger**: http://localhost:8085/swagger (⚠️ Ahora funciona en todos los ambientes)
- � **API JSON Schema**: http://localhost:8085/swagger/v1/swagger.json
- �💾 **Base de Datos**: localhost:1521/XEPDB1 (system/oracle123)

---

## 📦 Estructura del Proyecto

```
banco-sistema/
├── 🐳 docker-compose.yml              # Orquestación de contenedores
├── README.md                          # Este archivo
├── .gitignore                         # Archivos a ignorar en git
│
├── BancoApi/                          # API REST (.NET Core 8.0)
│   ├── 🐳 Dockerfile                  # Containerización del API
│   ├── BancoApi.csproj               # Configuración del proyecto
│   ├── Program.cs                    # Punto de entrada y configuración
│   ├── Controllers/                  # 11 controladores REST
│   │   ├── controller_Autenticacion.cs
│   │   ├── controller_Cliente.cs
│   │   ├── controller_Cuenta.cs
│   │   ├── controller_Transaccion.cs
│   │   ├── controller_Tarjeta.cs
│   │   ├── controller_Prestamo.cs
│   │   ├── controller_Cuota.cs
│   │   ├── controller_Abono.cs
│   │   ├── controller_Cajero.cs
│   │   ├── controller_Sucursal.cs
│   │   └── controller_Admin.cs
│   ├── Services/                     # Lógica de negocio (11 servicios)
│   ├── Repositories/                 # Acceso a datos (11 repositorios)
│   ├── DTOs/                         # Transferencia de datos
│   ├── Models/                       # Modelos de dominio
│   ├── appsettings.json              # Configuración
│   └── Properties/launchSettings.json
│
├── BancoWeb/                         # Frontend (ASP.NET Core MVC)
│   ├── BancoWeb.csproj
│   ├── Program.cs
│   ├── Views/                        # Vistas Razor
│   ├── wwwroot/                      # Archivos estáticos (CSS, JS, imágenes)
│   └── appsettings.json
│
└── ScriptBaseDatos/                  # Scripts SQL
    ├── SCRIPT_MAESTRO_COMPLETO.sql   # Script de inicialización automática
    └── BaseDeDatosExportadaScript/
        ├── base de datos exportada.sql
        └── datos prueba.sql
```

---

## 🛠️ Tecnología Utilizada

| Capa | Tecnología | Versión |
|------|-----------|---------|
| **Frontend** | ASP.NET Core MVC | 8.0 |
| **Backend** | ASP.NET Core Web API | 8.0 |
| **Base de Datos** | Oracle Database XE | Última |
| **Autenticación** | JWT Bearer | .NET 8.0 |
| **Containerización** | Docker & Docker Compose | Última |
| **ORM** | Oracle.ManagedDataAccess.Core | 23.26.0 |
| **API Docs** | Swashbuckle/Swagger | 10.0.1 |
| **CORS** | Microsoft.AspNetCore.Cors | 2.3.0 |

---

## 🔐 Autenticación y Seguridad

### JWT (JSON Web Tokens)
- ✅ Autenticación basada en tokens JWT
- ✅ Roles de usuario (Admin, Cliente, Cajero)
- ✅ Tokens con expiración configurable
- ✅ Refresh tokens para sesiones extendidas

### Contraseñas
- ✅ Contraseñas encriptadas en base de datos
- ✅ Validaciones de complejidad
- ✅ Recuperación segura de contraseña

### Base de Datos
- ✅ Foreign Keys para integridad referencial
- ✅ Constraints de dominio
- ✅ Auditoría de transacciones

---

## 📊 Modelo de Base de Datos

### Tablas Principales (10 tablas)

```sql
ADMINISTRADOR      -- Usuarios administradores del sistema
  ├─ ID_ADMINISTRADOR (PK)
  ├─ EMAIL_ADMINISTRADOR
  └─ CONTRASENA_ADMINISTRADOR

CLIENTE            -- Clientes del banco
  ├─ ID_CLIENTE (PK)
  ├─ NOMBRE_CLIENTE
  ├─ EMAIL_CLIENTE
  ├─ CEDULA_CLIENTE (Única)
  └─ ESTADO_CLIENTE

SUCURSAL           -- Sucursales bancarias
  ├─ ID_SUCURSAL (PK)
  ├─ NOMBRE_SUCURSAL
  ├─ DIRECCION_SUCURSAL
  └─ FK_ADMINISTRADOR

CUENTA             -- Cuentas bancarias
  ├─ ID_CUENTA (PK)
  ├─ ID_CLIENTE (FK)
  ├─ SALDO_CUENTA
  ├─ ESTADO_CUENTA
  └─ CONTRASENA_CUENTA

TARJETA            -- Tarjetas de crédito/débito
  ├─ ID_TARJETA (PK)
  ├─ ID_CUENTA (FK)
  ├─ NUMERO_TARJETA (Única)
  ├─ TIPO_TARJETA
  ├─ LIMITE_TARJETA
  └─ ESTADO_TARJETA

PRESTAMO           -- Préstamos
  ├─ ID_PRESTAMO (PK)
  ├─ ID_CLIENTE (FK)
  ├─ MONTO_PRESTAMO
  ├─ TASA_INTERES
  ├─ PLAZO_MESES
  └─ ESTADO_PRESTAMO

CUOTA              -- Cuotas de préstamos
  ├─ ID_CUOTA (PK)
  ├─ ID_PRESTAMO (FK)
  ├─ NUMERO_CUOTA
  ├─ MONTO_CUOTA
  ├─ FECHA_VENCIMIENTO
  └─ ESTADO_CUOTA

TRANSACCION        -- Histórico de transacciones
  ├─ ID_TRANSACCION (PK)
  ├─ ID_CUENTA (FK)
  ├─ TIPO_TRANSACCION
  ├─ MONTO_TRANSACCION
  └─ ESTADO_TRANSACCION

ABONO_EXTRAORDINARIO -- Abonos adicionales a préstamos
  ├─ ID_ABONO (PK)
  ├─ ID_PRESTAMO (FK)
  ├─ MONTO_ABONO
  └─ TIPO_ABONO

CAJERO             -- Cajeros de sucursales
  ├─ ID_CAJERO (PK)
  ├─ ID_SUCURSAL (FK)
  ├─ DINERO_DISPONIBLE_CAJERO
  └─ ESTADO_CAJERO
```

---

## 🐳 Docker y Containerización

### docker-compose.yml
Configura dos servicios:

1. **Oracle Database XE** (puerto 1521)
   - Base de datos precargada automáticamente
   - Volumen persistente para datos
   - Health check integrado

2. **BancoApi** (puerto 8085)
   - API REST compilada en contenedor
   - Espera a que Oracle esté listo
   - Logging centralizado

### Recompilar Contenedores

#### **Opción 1: Recompilar sin eliminar datos**
```bash
# Detener sin eliminar volumen de datos
docker-compose down

# Recompilar y ejecutar
docker-compose up -d --build

# Oracle mantendrá todos los datos del volumen anterior
```

#### **Opción 2: Recompilar y limpiar BD**
```bash
# Eliminar todo incluyendo BD
docker-compose down -v

# Recompilar desde cero
docker-compose up -d --build

# Esperar 2-3 minutos para que se inicialice Oracle
docker-compose ps

# Verificar que sea "healthy"
```

#### **Opción 3: Recompilar solo la API**
```bash
# Sin reiniciar Oracle (más rápido)
docker-compose up -d --build banco-api

# Solo reconstruye la imagen de API
```

---

## 💻 Ejecución Manual (Desarrollo)

### Frontend Local (Sin Contenedor)

#### **Paso 1: Abrir Terminal PowerShell**
```powershell
# Terminal 1 - Contenedores (si quieres usar BD en contenedor)
cd C:\ruta\del\proyecto\banco-sistema
docker-compose up -d

# O si usas Oracle local, no necesitas Docker
```

#### **Paso 2: Navegar a la carpeta del Frontend**
```powershell
# Terminal 2 - Frontend
cd BancoWeb\BancoWeb
```

#### **Paso 3: Restaurar Dependencias**
```powershell
dotnet restore
```

#### **Paso 4: Ejecutar con Hot Reload**
```powershell
dotnet watch
```

**Resultado:**
```
✔ Application started. Press Ctrl+C to shut down.
✔ Listening on: http://localhost:5000
```

Se abre automáticamente en el navegador: **http://localhost:5000**

#### **Paso 5: Hacer Cambios y Verificar (Hot Reload)**
- Modifica cualquier archivo `.cs` o `.cshtml`
- **Automáticamente** se recompila y recarga
- Presiona F5 en el navegador si no se recarga

#### **Paso 6: Detener**
```powershell
# Presiona Ctrl+C en la terminal
```

---

## 🔄 Workflow de Desarrollo

### Desarrollar Localmente

```powershell
# Terminal 1: Contenedores con BD
docker-compose up -d

# Terminal 2: API .NET
cd BancoApi\BancoApi
dotnet watch

# Terminal 3: Frontend
cd BancoWeb\BancoWeb
dotnet watch

# Ahora tienes:
# - Frontend en http://localhost:5000
# - API en http://localhost:8085
# - BD en localhost:1521
# - Todos con hot reload habilitado
```

### Hacer un Cambio y Probar

```
1. Edita un archivo (.cs, .cshtml, etc.)
   ↓
2. Guarda (Ctrl+S)
   ↓
3. El compilador recompila automáticamente
   ↓
4. Actualiza el navegador (F5)
   ↓
5. ¡Ve el cambio en tiempo real!
```

---

## 🚀 Deployment (Producción)

### Ejecutar en Producción

```bash
# Usar docker-compose (recomendado)
docker-compose -f docker-compose.yml up -d

# Escala manual
docker-compose up -d --scale banco-api=3

# Monitoreo
docker-compose logs -f banco-api
```

### Alternativa: Kubernetes (K8s)

Para ambientes empresariales, puedes usar Kubernetes con los Dockerfiles provistos.

---

## 📚 Endpoints Principales

### Autenticación
```
POST   /api/autenticacion/login          # Login
POST   /api/autenticacion/register       # Registro
POST   /api/autenticacion/refresh        # Renovar token
```

### Clientes
```
GET    /api/cliente                      # Listar clientes
GET    /api/cliente/{id}                 # Obtener cliente
POST   /api/cliente                      # Crear cliente
PUT    /api/cliente/{id}                 # Actualizar cliente
DELETE /api/cliente/{id}                 # Eliminar cliente
```

### Cuentas
```
GET    /api/cuenta                       # Listar cuentas
GET    /api/cuenta/{id}                  # Obtener cuenta
POST   /api/cuenta                       # Crear cuenta
GET    /api/cuenta/{id}/saldo            # Obtener saldo
```

### Transacciones
```
POST   /api/transaccion/transferencia    # Transferencia bancaria
POST   /api/transaccion/deposito         # Depósito
POST   /api/transaccion/retiro           # Retiro
GET    /api/transaccion/{id}/historial   # Historial
```

### Más Endpoints
- `/api/prestamo` - Gestión de préstamos
- `/api/cuota` - Gestión de cuotas
- `/api/tarjeta` - Gestión de tarjetas
- `/api/abono` - Abonos extraordinarios
- `/api/cajero` - Gestión de cajeros
- `/api/sucursal` - Gestión de sucursales
- `/api/admin` - Administración del sistema

---

## 📊 Estadísticas del Proyecto

| Métrica | Valor |
|---------|-------|
| **Líneas de Código** | 10,000+ |
| **Controladores** | 11 |
| **Servicios** | 11 |
| **Repositorios** | 11 |
| **Tablas BD** | 10 |
| **Endpoints** | 50+ |
| **Lenguaje** | C# (.NET 8.0) |
| **Framework** | ASP.NET Core |
| **BD** | Oracle XE |

---

## 🧪 Testing

### Probar API con Swagger

```
1. Ejecutar: docker-compose up -d
2. Abrir: http://localhost:8085/swagger
3. Probar endpoints interactivamente
4. Ver respuestas en JSON
```

### Probar con cURL

```bash
# Login
curl -X POST http://localhost:8085/api/autenticacion/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@banco.com","password":"admin123"}'

# Obtener clientes
curl -X GET http://localhost:8085/api/cliente \
  -H "Authorization: Bearer <tu_token>"
```

---

## 🔧 Configuración

### appsettings.json (API)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=oracle-db:1521/XEPDB1;User Id=system;Password=oracle123;"
  },
  "Jwt": {
    "SecretKey": "tu-secret-key-muy-segura-aqui",
    "Issuer": "BancoAPI",
    "Audience": "BancoClients",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Credenciales de Base de Datos

#### Desde Contenedores (API Docker)
```
Usuario:       system
Contraseña:    oracle123
Host:          oracle-db:1521
Base de Datos: XEPDB1
Connection String: User Id=system;Password=oracle123;Data Source=oracle-db:1521/XEPDB1;
```

#### Desde tu PC (Local)
```
Usuario:       system
Contraseña:    oracle123
Host:          localhost:1521
Base de Datos: XEPDB1
Connection String: User Id=system;Password=oracle123;Data Source=localhost:1521/XEPDB1;
```

#### Credenciales de Prueba (Datos precargados)
```
Admin: admin1@banco.com / admin123
Cliente 1: juan.perez@email.com (Cuenta: 1, Contraseña: 1234567890)
Cliente 2: maria.lopez@email.com (Cuenta: 2, Contraseña: 2345678901)
```

---

## 📖 Documentación Adicional

- 📚 **API Documentation**: http://localhost:8085/swagger
- 💾 **Database Schema**: Ver `ScriptBaseDatos/SCRIPT_MAESTRO_COMPLETO.sql`
- 🐳 **Docker Setup**: Ver `docker-compose.yml`

---

## 🤝 Contribuir

Para mejorar el proyecto:

```bash
1. Fork el repositorio
2. Crea una rama (git checkout -b feature/mejora)
3. Realiza cambios y commit (git commit -am 'Agrega mejora')
4. Push a la rama (git push origin feature/mejora)
5. Abre un Pull Request
```

---

## 📄 Licencia

Este proyecto está bajo licencia **MIT**. Ver `LICENSE` para más detalles.

---

## 👤 Autor

**Juan Carlos - Desarrollador FullStack**
- Portfolio: [juancarlospruebas.com](https://juancarlospruebas.com)
- Email: juancarlos@email.com
- GitHub: [@juancarlos](https://github.com/juancarlos)

---

## 📞 Soporte

Para problemas o preguntas:

1. Abre un **Issue** en GitHub
2. Contacta: juancarlos@email.com
3. Consulta la documentación en Swagger

---

## 🎓 Lessons Learned

✅ Arquitectura en capas (Controllers → Services → Repositories)
✅ Uso de DTOs para transferencia de datos
✅ Autenticación JWT en ASP.NET Core
✅ Containerización con Docker
✅ Orquestación con Docker Compose
✅ Modelos de datos normalizados
✅ Foreign Keys y Constraints
✅ Error handling y validaciones
✅ API RESTful siguiendo convenciones

---

## 🚀 Roadmap Futuro

- [ ] Agregar autenticación con OAuth 2.0
- [ ] Implementar caché con Redis
- [ ] Logging centralizado con ELK Stack
- [ ] Unit tests y integration tests
- [ ] CI/CD con GitHub Actions
- [ ] Documentación interactiva con Postman
- [ ] Internacionalización (i18n)
- [ ] Notificaciones por email/SMS

---

**¡Gracias por revisar Banco Sistema! 🙏**

*Hecho con ❤️ en ASP.NET Core 8.0*
