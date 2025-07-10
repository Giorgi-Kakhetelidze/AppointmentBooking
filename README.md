# 📅 Appointment Booking API

An API for managing appointments with service providers — built with **.NET 8**, **MediatR**, **FluentValidation**, and **PostgreSQL**.

---

## 📚 Table of Contents

- [Overview](#overview)  
- [Technologies](#technologies)  
- [Getting Started](#getting-started)  
- [API Endpoints](#api-endpoints)  
- [Validation Strategy](#validation-strategy)  
- [Configuration](#configuration)  
- [How to Run](#how-to-run)  

---

## 🔍 Overview

The Appointment Booking API enables clients to:

- Create and manage appointments  
- Validate provider availability and working hours  
- Prevent overlapping bookings  
- Support recurring appointments  
- Send automated email notifications upon successful bookings  

---

## 🛠 Technologies

- **.NET 8** (Minimal APIs)  
- **MediatR** for CQRS pattern  
- **FluentValidation** for input validation  
- **Entity Framework Core** with **PostgreSQL**  
- **SMTP** for email notifications (e.g., Gmail)  
- **Swagger UI** for testing and exploration  

---

## 🚀 Getting Started

1. **Clone the repository**  
2. **Configure database and email** in `appsettings.json` or using environment variables  
3. **Apply migrations**  
   ```bash
   dotnet ef database update
   ```
4. **Run the application**  
   ```bash
   dotnet run
   ```
5. **Access Swagger UI**  
   Navigate to `https://localhost:{port}/swagger`  

---

## 🔌 API Endpoints

### 📅 Create Appointment

- **POST** `/appointments`  
- **Request Example**:
```json
{
  "providerId": "guid",
  "customerName": "John Doe",
  "customerEmail": "john@example.com",
  "customerPhone": "+123456789",
  "appointmentDate": "2025-07-14T00:00:00",
  "startTime": "09:00:00",
  "endTime": "09:30:00",
  "timeZoneId": "Georgian Standard Time",
  "isRecurring": false,
  "recurrenceRule": null,
  "parentAppointmentId": null
}
```

---

### ✏️ Update Appointment

- **PUT** `/appointments/{id}`  
- Includes all validations from create, plus:
  - Appointment must exist  
  - Valid status must be provided (`Scheduled`, `Cancelled`, etc.)  
  - `ParentAppointmentId` must reference a valid recurring series, if used  

---

### ❌ Delete Appointment

- **DELETE** `/appointments/{id}`  
- Appointment must exist  
- Deletion rules may depend on status (e.g., only if `Scheduled`)  

---

## ✅ Validation Strategy

A layered validation approach is used to enforce data quality and business rules.

### 🔧 Validation Layers

| Layer                     | Purpose                                                       | Tool                     |
|--------------------------|---------------------------------------------------------------|--------------------------|
| **Input Validation**     | Ensures correct structure, required fields, valid formats     | `FluentValidation`       |
| **Business Validation**  | Enforces domain logic: working hours, conflicts, availability | `AppointmentValidatorService` |

---

### 📄 Field-Level Rules (`POST /appointments`)

| Field                | Rule                                                     |
|---------------------|----------------------------------------------------------|
| `customerName`      | Required                                                 |
| `customerEmail`     | Required, valid email format                             |
| `customerPhone`     | Required                                                 |
| `appointmentDate`   | Required                                                 |
| `startTime`/`endTime`| Required, valid, and `startTime < endTime`              |
| `timeZoneId`        | Must be a valid system timezone                          |
| `isRecurring`       | If `true`, `recurrenceRule` must be provided            |
| `providerId`        | Must exist and be active in the database                 |

---

### 🔒 Business Rules

- Appointment must fall **within provider’s working hours**
- Working hours must exist for the selected `DayOfWeek`
- No **overlapping appointments** allowed for the same provider
- Provider must be **active**

On **update**, the current appointment is **excluded** from the overlap check.

---

## ⚙️ Configuration

Modify `appsettings.json` or set environment variables for:

- PostgreSQL connection string  
- SMTP settings for email (host, port, username, password)  
- Application-level options (time zone handling, recurrence defaults, etc.)

---

## 🧪 How to Run

```bash
# 1. Restore dependencies
dotnet restore

# 2. Apply database migrations
dotnet ef database update

# 3. Run the API
dotnet run

# 4. Test via Swagger UI
https://localhost:{port}/swagger
```

---

Happy coding! 🚀
