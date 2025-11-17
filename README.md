ProyectoIntuit – Challenge Intuit/Yappa

Este repositorio contiene el desarrollo solicitado para el Challenge de Intuit/Yappa, compuesto por:

- API REST en .NET 8 (Backend)
- Aplicación Cliente (Frontend HTML/CSS/JS)
- Proyecto de Tests (xUnit)
- Análisis de calidad con SonarQube

🛠 Tecnologías Utilizadas
Backend

- .NET 8
- Entity Framework Core
- MySQL
- MVC
- Swagger
- ILogger (logging)
- Stored Procedures
- Frontend
- HTML
- CSS
- JavaScript
- Testing
- xUnit
- Calidad
- SonarQube (última versión)

Estructura del Proyecto
- /ProyectoIntuit (Backend)
- /ProyectoIntuitWeb (Frontend)
- /ProyectoIntuit.Tests (Tests)

🗄 Base de Datos

- Motor: MySQL
- Script inicial provisto en el challenge
- Store Procedure utilizado:
GetAllClientes_SP
Obtiene todos los clientes mediante un SP.

🚀 Endpoints Implementados
- GET – Obtener todos
- GET api/Cliente/GetAll

- GET – Obtener todos desde Stored Procedure
- GET api/Cliente/GetAll_SP

- GET – Búsqueda por nombre
- GET api/Cliente/Search?nombre=facu

- GET – Obtener cliente por ID
- GET api/Cliente/Get/{id}

- POST – Insertar cliente
- POST api/Cliente/Insert

- PUT – Actualizar cliente
- PUT api/Cliente/Update/{id}

- DELETE – Eliminar cliente
- DELETE api/Cliente/Delete/{id}

✔ Validaciones Implementadas

- Unicidad del ID
- Campos obligatorios:
  -  Nombre
  -  Apellido
  -  Email
  -  Celular
  -  CUIT
  -  Razón Social
    
-Validación de formato:
  -  CUIT
  -  Email
  -  Fecha de nacimiento
 
📝 Logging

Implementado con:
  -  _logger = logger;

Registra excepciones y errores de la API.

📘 Documentación Swagger

Disponible en:
  -  /swagger/index.html

Incluye endpoints, modelos y descripciones.

🧪 Testing

- Framework: xUnit
- Tests realizados sobre los Controllers
- Integración con SonarQube
- Estado final: OK (verde)

🌐 Frontend – App Cliente

Funcionalidades:
- Buscador de clientes
- Ver detalle
- Agregar cliente
- Editar cliente
- Eliminar cliente
  
Tecnologías: HTML, CSS y JavaScript
Se comunica directamente con la API desarrollada.



