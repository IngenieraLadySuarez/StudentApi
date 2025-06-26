# StudentApi
# 📚 Backend - Registro de Estudiantes (API REST)

Este proyecto corresponde al backend del sistema de gestión de estudiantes, desarrollado como parte de una prueba técnica para Interrapidísimo. Proporciona una API REST construida en .NET 6 que gestiona estudiantes, materias, profesores, asignaciones 
y visualización de compañeros de clase.

## ⚙️ Tecnologías

- ASP.NET Core 6
- C#
- Entity Framework Core
- SQL Server
- Dapper
- JWT (autenticación si se requiere extender)
- Swagger para pruebas

## 📁 Estructura del proyecto

StudentApi/
├── Controllers/
├── Models/
├── DTOs/
├── Services/
├── Interfaces/
├── Program.cs
├── appsettings.json


## 🧪 Endpoints destacados

- `GET /api/Students` – Obtener todos los estudiantes.
- `POST /api/Students` – Crear un nuevo estudiante.
- `DELETE /api/Students/{id}?course={course}` – Eliminar estudiante.
- `POST /api/StudentSubjects/{studentId}/assign/{subjectId}` – Asignar materia.
- `GET /api/StudentSubjects/{studentId}/classmates` – Ver compañeros de clase.

## 🔧 Configuración local

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/tuusuario/student-backend.git
   cd student-backend
2. Configurar la cadena de conexión en appsettings.json:
   "ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=StudentDb;Trusted_Connection=True;"
}
3. Ejecutar migraciones y actualizar la base de datos:
   dotnet ef database update
4. Iniciar la aplicación:
   dotnet run

Probar la API
Una vez ejecutada, accede a la documentación en:
http://localhost:5100/swagger

 Notas
Asegúrate de tener SQL Server activo con la base de datos StudentDb creada previamente.

Puedes poblar datos con un script inicial o usando Swagger UI.



