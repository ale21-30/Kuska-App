# Kuska 💜
### *Juntas hacia el futuro · Ecuador*

> Plataforma fintech + mentorías tech para niñas y adolescentes vulnerables en Ecuador.  
> Conecta fondos comunitarios escolares con mentorías en vivo, protegiendo los datos de las menores desde el diseño.

---

## 🏆 SheShips 2026 — Hackathon Submission

| | |
|---|---|
| **Categorías** | Best Fintech Solution for Women's Economic Empowerment · Best Financial Inclusion Solution for Women |
| **Equipo** | Hacker |
| **País** | 🇪🇨 Ecuador |
| **Stack** | ASP.NET Core 10 MVC · SQL Server · Entity Framework Core · Daily.co API |

---

## 🌱 El problema que resolvemos

En Ecuador, miles de niñas de escuelas públicas abandonan sus estudios cada año por falta de recursos básicos — útiles, uniformes, acceso a tecnología. Al mismo tiempo, los programas RSE corporativos y los fondos de cooperación internacional no tienen infraestructura digital para llegar directamente a las beneficiarias de forma trazable y transparente.

**Kuska es el canal que faltaba.**

---

## ✨ La solución

Kuska conecta tres actores que hoy operan por separado:

```
Empresas RSE + Cooperación Internacional
              ↓
    Fondos comunitarios escolares
              ↓
  Niñas y adolescentes beneficiarias
              +
    Mentorías tech en vivo seguras
```

### Módulos principales

#### 💰 Fondo Comunitario Escolar
- Fondos grupales por escuela con meta, progreso y trazabilidad completa
- Fuentes de financiamiento: RSE corporativa, cooperación internacional, aportes de madres
- Cada centavo es rastreable — log inmutable de todas las transacciones
- Panel exclusivo para empresas patrocinadores con métricas agregadas y certificado RSE descargable

#### 👩‍💻 Mentorías Tech
- Catálogo de mentoras verificadas (programación, diseño UX, data science, IA, ciberseguridad)
- Verificación con cédula antes de poder ofrecer sesiones — sin registro libre
- Sistema de agenda con selección de niña, fecha y tema
- Rating y evaluación post-sesión

#### 🎥 Sala Supervisada
- Videollamada con Daily.co API — máximo 3 participantes (niña, mentora, supervisora)
- La sesión se pausa automáticamente si el tutor se desconecta
- Link único con expiración — no reutilizable
- Duración máxima: 60 minutos
- Botón de reporte 🚨 que cierra la sala de inmediato y genera alerta

---

## 🔐 Arquitectura de seguridad — *"Privacy by Design"*

> Cuando los usuarios son niñas vulnerables, la seguridad no es opcional.

### RBAC — 4 roles con permisos estrictos

| Rol | Qué puede ver |
|---|---|
| **Madre/Tutora** | Solo los datos de su hija |
| **Mentora verificada** | Nombre de pila + edad únicamente |
| **Empresa/Patrocinador** | Métricas agregadas — NUNCA datos individuales |
| **Admin** | Dashboard completo + alertas de seguridad |

### Capas de protección implementadas

- 🔑 **Verificación anti-grooming:** Las mentoras requieren validación manual con cédula antes de acceder
- 👁️ **Sala supervisada:** Tutor obligatorio presente, pausa automática si se desconecta
- 🔗 **Links únicos:** Cada sala tiene un link con expiración y no es reutilizable
- 🗄️ **Cifrado de datos sensibles:** Campos críticos encriptados en SQL Server
- 📋 **Log de auditoría inmutable:** Tabla con triggers — registro de toda acción (quién, qué, cuándo, desde dónde)
- 🛡️ **Sin IDs reales en URLs:** Tokens con expiración corta, honeypot anti-bots
- ⚖️ **Marco legal:** Cumplimiento total con **LOPDP Ecuador 2021** + Código de la Niñez y Adolescencia

---

## 🏗️ Arquitectura técnica

```
kuska-app/
└── src/
    ├── Kuska.Core/          # Entidades y modelos de dominio
    │   └── Entities/
    │       ├── Usuario.cs   # Con RBAC
    │       ├── Nina.cs      # Solo nombre de pila — sin datos sensibles
    │       ├── Fondo.cs     # Fondo comunitario con trazabilidad
    │       ├── Mentora.cs   # Con verificación de cédula
    │       ├── Sesion.cs    # Sala supervisada
    │       └── AuditoriaLog.cs  # Log inmutable
    │
    ├── Kuska.Data/          # Acceso a datos
    │   ├── KuskaDbContext.cs    # EF Core + configuración de relaciones
    │   └── AuditoriaService.cs  # Registro automático de acciones
    │
    ├── Kuska.Services/      # Lógica de negocio
    │
    └── Kuska.Web/           # ASP.NET Core MVC
        ├── Controllers/
        │   ├── AuthController.cs       # Login + Registro por roles
        │   ├── FondosController.cs     # Módulo financiero
        │   ├── MentoriasController.cs  # Agenda + evaluación
        │   └── SesionesController.cs   # Sala supervisada + Daily.co
        └── Views/
            ├── Auth/       # Login · Registro
            ├── Fondos/     # Lista · Detalle · Crear
            ├── Mentorias/  # Catálogo · Agendar · Mis sesiones
            └── Sesiones/   # Sala supervisada
```

### Stack tecnológico

| Capa | Tecnología |
|---|---|
| Backend | ASP.NET Core 10 MVC (C#) |
| Base de datos | SQL Server + Entity Framework Core 10 |
| Frontend | HTML5 · CSS3 · Bootstrap-free (CSS custom) |
| Tipografía | Cormorant Garamond (Canela-style) + Nunito |
| Video | Daily.co API (salas con expiración y max_participants: 3) |
| Autenticación | Session-based + SHA256 password hashing |
| Auditoría | SQL Triggers + AuditoriaService (C#) |
| Demo | localhost + ngrok |

---

## 🚀 Instalación y ejecución local

### Prerrequisitos
- .NET 10 SDK
- SQL Server (cualquier edición)
- Git

### Pasos

```bash
# 1. Clonar el repositorio
git clone https://github.com/TU_USUARIO/kuska-app.git
cd kuska-app

# 2. Crear la base de datos
# Abrir SQL Server Management Studio y ejecutar:
# /scripts/KuskaDB_Create.sql

# 3. Configurar la conexión
# Editar src/Kuska.Web/appsettings.json:
# "KuskaDB": "Server=.;Database=KuskaDB;Trusted_Connection=True;TrustServerCertificate=True;"

# 4. Ejecutar la aplicación
cd src/Kuska.Web
dotnet run
```

### Usuarios de prueba

| Rol | Email | Contraseña |
|---|---|---|
| Madre/Tutora | maria@kuska.ec | 123 |
| Admin | admin@kuska.ec | 123 |
| Empresa | empresa@kuska.ec | 123 |

---

## 💼 Modelo de financiamiento

Kuska no reinventa el dinero — canaliza recursos que ya existen pero no tienen infraestructura digital:

| Fuente | Descripción |
|---|---|
| **RSE Corporativa** | Banco Pichincha, Corporación Favorita, Grupo Nobis — fondo base garantizado |
| **Cooperación Internacional** | UNICEF, BID, CAF, Plan Internacional — casos de mayor vulnerabilidad |
| **Aportes comunitarios** | Madres de familia — multiplicador opcional |
| **Estado (Q2 2026)** | Integración futura con Ministerio de Educación Ecuador |

> **Pitch frame:** Kuska es el canal de distribución transparente y trazable para programas RSE y cooperación que ya existen pero no tienen infraestructura digital.

---

## 📊 Impacto proyectado — Ciclo 2026

| Métrica | Meta |
|---|---|
| Niñas beneficiadas | 500 |
| Mentoras verificadas | 50 |
| Sesiones de mentoría | 500+ |
| Fondos distribuidos | $30,000 |
| Provincias cubiertas | 15 |
| Empresas RSE | 20+ |

---

## 🗺️ Roadmap

| Fase | Timeline | Hitos |
|---|---|---|
| **MVP** ✅ | Mar 2026 | Fondos + Mentorías + Sala supervisada + LOPDP |
| **V1.0** | May 2026 | Integración Ministerio Educación · App móvil |
| **V2.0** | Sep 2026 | 15 provincias · IA para matching mentora-niña |
| **V3.0** | 2027 | Expansión regional · Perú · Colombia |

---

## 👩‍💻 Sobre el nombre

**Kuska** significa *"juntas"* en quechua — el idioma de los pueblos originarios de Ecuador y gran parte de Sudamérica. Elegimos este nombre porque refleja exactamente lo que hacemos: juntar a niñas, mentoras, familias y empresas para construir el futuro que todas merecen.

---

*Kuska · SheShips 2026 · Ecuador 🇪🇨*
