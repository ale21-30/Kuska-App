# Kuska 💜
### *Juntas hacia el futuro · Ecuador*

> Plataforma fintech + mentorías tech para niñas y adolescentes vulnerables en Ecuador.  
> Conecta apadrinamiento directo de útiles escolares, mentorías tech en vivo supervisadas y fondos comunitarios — protegiendo los datos de las menores desde el diseño.

---

## 🏆 SheShips 2026 — Hackathon Submission

| | |
|---|---|
| **Categorías** | Best Fintech Solution for Women's Economic Empowerment · Best Financial Inclusion Solution for Women (Sezzle) |
| **País** | 🇪🇨 Ecuador |
| **Stack** | ASP.NET Core 10 MVC · SQL Server · Entity Framework Core · Daily.co API |
| **Repo** | https://github.com/ale21-30/Kuska-App |

---

## 🌱 El problema que resolvemos

En Ecuador, miles de niñas de escuelas públicas abandonan sus estudios cada año por falta de recursos básicos — útiles, uniformes, acceso a tecnología. Al mismo tiempo, los programas RSE corporativos y los fondos de cooperación internacional no tienen infraestructura digital para llegar directamente a las beneficiarias de forma trazable y transparente.

**Kuska es el canal que faltaba.**

---

## ✨ La solución

Kuska conecta tres actores que hoy operan por separado:

```
Empresas RSE + Donantes individuales
              ↓
    Apadrinamiento directo a la niña
    (útiles + uniforme con precio real)
              ↓
  Niñas y adolescentes beneficiarias
              +
    Mentorías tech en vivo seguras
```

---

## 📱 Módulos del sistema

### 💜 Apadrinar una niña — *el corazón de Kuska*
- Galería de niñas con avatar cartoon, edad, área de interés e historia en 2 frases
- La mamá/tutora carga la lista real de útiles y uniforme con precios
- El donante elige a quién apoyar y qué ítems cubrir — **sin intermediarios ni escuelas**
- Trazabilidad completa: cada ítem muestra quién lo apadrinó y cuándo
- Protección de datos: donante solo ve nombre de pila y edad — nunca apellidos ni dirección
- Cumplimiento total con **LOPDP Ecuador 2021**

### 💰 Fondo Comunitario Escolar
- Fondos grupales por escuela con meta, progreso y trazabilidad completa
- Fuentes: RSE corporativa, cooperación internacional, aportes personales
- Historial inmutable de aportes con log de auditoría
- Barra de progreso en tiempo real

### 👩‍💻 Mentorías Tech
- Catálogo de mentoras verificadas: programación, diseño UX, data science, IA, ciberseguridad
- Verificación con cédula antes de poder ofrecer sesiones — sin registro libre
- Agenda con selección de niña, fecha y tema
- Rating y evaluación post-sesión

### 🎥 Sala Supervisada
- Videollamada con Daily.co API — máximo 3 participantes
- Indicador EN VIVO con punto rojo pulsante
- Supervisora siempre presente — pausa automática si se desconecta
- Link único `kuska.sala/{id}` con expiración — no reutilizable
- Controles: 🎤 · 📷 · 💬 · 🚨 reporte inmediato · 📞 salir
- Panel de privacidad: datos visibles vs ocultos + log de auditoría + LOPDP

### 👧 Mi hija
- La mamá registra a su hija: nombre de pila, edad, interés, historia
- Agrega lista de útiles con precios reales
- Historial de apadrinamientos recibidos

---

## 🔐 Seguridad — *"Privacy by Design"*

### RBAC — 4 roles

| Rol | Acceso |
|---|---|
| **Madre/Tutora** | Datos de su hija · Fondos · Mentorías · Apadrinar |
| **Mentora verificada** | Nombre de pila + edad únicamente en sala |
| **Empresa/Patrocinador** | Métricas agregadas — NUNCA datos individuales |
| **Admin** | Dashboard completo + alertas |

### Capas de protección
- 🔑 Verificación anti-grooming con cédula para mentoras
- 👁️ Sala supervisada con pausa automática
- 🔗 Links únicos con expiración
- 📋 Log de auditoría inmutable (SQL trigger + C#)
- 🛡️ Datos mínimos: solo nombre de pila y edad para terceros
- ⚖️ LOPDP Ecuador 2021 + Código de la Niñez y Adolescencia

---

## 🏗️ Stack técnico

| Capa | Tecnología |
|---|---|
| Backend | ASP.NET Core 10 MVC (C#) |
| Base de datos | SQL Server + Entity Framework Core 10 |
| Frontend | HTML5 · CSS3 custom · sin dependencias externas |
| Tipografía | Cormorant Garamond + Nunito |
| Video | Daily.co API |
| Auth | Session-based + SHA256 |

---

## 🚀 Instalación local

```bash
git clone https://github.com/ale21-30/Kuska-App.git
cd Kuska-App
# Ejecutar script SQL en SSMS
# Editar appsettings.json con connection string
cd src/Kuska.Web
dotnet run
# → http://localhost:5036
```

### Usuarios de prueba

| Rol | Email | Contraseña |
|---|---|---|
| Madre/Tutora | maria@kuska.ec | 123 |
| Admin | admin@kuska.ec | 123 |
| Empresa | empresa@kuska.ec | 123 |

---

## 📊 Impacto proyectado 2026

| Métrica | Meta |
|---|---|
| Niñas beneficiadas | 500 |
| Mentoras verificadas | 50 |
| Útiles apadrinados | 2,000+ ítems |
| Fondos distribuidos | $30,000 |
| Empresas RSE | 20+ |

---

## 🗺️ Roadmap

| Fase | Timeline | Hitos |
|---|---|---|
| **MVP** ✅ | Mar 2026 | Apadrinamiento · Mentorías · Sala supervisada · LOPDP |
| **V1.0** | May 2026 | App móvil · Integración Ministerio Educación |
| **V2.0** | Sep 2026 | 15 provincias · IA para matching |
| **V3.0** | 2027 | Expansión: Perú · Colombia |

---

**Kuska** significa *"juntas"* en quechua — el idioma de los pueblos originarios de Ecuador.

*Kuska · SheShips 2026 · Ecuador 🇪🇨*
