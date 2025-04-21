
# Libropedia
Proyecto backend desarrollado con **Azure Functions en .NET 9**, diseñado para exponer endpoints serverless con autenticación JWT, integración con SQL Server a través de Dapper, y configuración local mediante `local.settings.json`.

---

# Documentación Técnica - Proyecto Libropedia

## Descripción General
Libropedia es una API REST desarrollada con Azure Functions en .NET 9, diseñada para gestionar una biblioteca digital. El proyecto implementa una arquitectura serverless y utiliza autenticación JWT para proteger sus endpoints.

## Tecnologías Principales
- Azure Functions v4
- .NET 9
- MySQL (con Dapper como ORM)
- JWT para autenticación
- Application Insights para telemetría

## Arquitectura del Proyecto

### 1. Estructura de Capas
El proyecto sigue una arquitectura en capas bien definida:

- **Functions**: Controladores de Azure Functions que manejan los endpoints HTTP
- **Repositories**: Capa de acceso a datos con implementaciones de servicios
- **Models**: DTOs y modelos de dominio
- **DataContext**: Configuración de conexión a la base de datos
- **Helpers**: Utilidades y servicios auxiliares
- **Middleware**: Componentes de procesamiento de solicitudes

### 2. Componentes Principales

#### 2.1 Gestión de Autenticación
- Implementa autenticación JWT con middleware personalizado
- Configuración flexible de JWT mediante JwtSettings
- Manejo de tokens con validación de claims

#### 2.2 Endpoints Principales
- **UserManage**: Registro y autenticación de usuarios
- **BookManage**: CRUD y búsqueda de libros
- **AuthorManage**: Gestión de autores
- **CategoryManage**: Gestión de categorías
- **CommentManage**: Gestión de reseñas y comentarios

#### 2.3 Base de Datos
- Uso de MySQL con Dapper para acceso a datos
- Procedimientos almacenados para operaciones complejas
- Esquema relacional con tablas para:
  - Usuarios
  - Libros
  - Autores
  - Categorías
  - Reseñas

### 3. Características Técnicas

#### 3.1 Seguridad
- Autenticación mediante tokens JWT
- Middleware de autenticación personalizado
- Validación de roles y permisos
- Manejo seguro de contraseñas

#### 3.2 Patrones de Diseño
- Patrón Repository
- Inyección de Dependencias
- Middleware Pattern
- Data Transfer Objects (DTOs)

#### 3.3 Funcionalidades Destacadas
- Paginación de resultados
- Filtrado y búsqueda avanzada
- Sistema de calificaciones y reseñas
- Gestión de relaciones entre entidades

### 4. Configuración y Despliegue
- Configuración mediante local.settings.json para desarrollo
- Variables de entorno para producción
- Soporte para Application Insights
- Integración con Azure Functions

### 5. Base de Datos

#### Tablas Principales:
- **usuarios**: Gestión de usuarios y autenticación
- **libros**: Catálogo de libros
- **autores**: Información de autores
- **categorias**: Categorización de libros
- **resenas**: Sistema de reseñas y calificaciones

### 6. Endpoints API

#### Autenticación
- POST /api/Register: Registro de usuarios
- POST /api/Login: Autenticación de usuarios

#### Libros
- POST /api/GetAllBooksPaginated: Lista paginada de libros
- GET /api/GetBookTopQualifications/{limit}: Top de libros mejor calificados
- GET /api/GetBookDetail/{id}: Detalles de un libro

#### Autores y Categorías
- POST /api/GetAuthorFilter: Filtrado de autores
- POST /api/GetCategoryFilter: Filtrado de categorías

#### Reseñas
- POST /api/StoreComment: Crear reseña
- PUT /api/UpdateComment: Actualizar reseña
- POST /api/GetUserComments: Obtener reseñas de usuario
- DELETE /api/DeleteComment/{commentId}: Eliminar reseña

## Consideraciones de Desarrollo
- Código modular y mantenible
- Separación clara de responsabilidades
- Uso extensivo de interfaces para desacoplamiento
- Manejo de errores consistente
- Documentación de código mediante comentarios

### 7. Pruebas
Configuración de Pruebas
El proyecto incluye un conjunto de pruebas unitarias implementadas con xUnit, Moq y FluentAssertions. Estas pruebas están diseñadas para garantizar la funcionalidad de los servicios principales y la lógica de negocio.

#### Ejecución de Pruebas
Para ejecutar las pruebas, utiliza el siguiente comando:
- dotnet test

#### Cobertura de Pruebas
Las pruebas cubren los siguientes servicios:

- AuthorService: Filtrado de autores
- BookService: Gestión de libros
- CategoryService: Filtrado de categorías
- CommentService: Gestión de reseñas
- UserService: Registro y autenticación de usuarios

### 8. Instalación y Ejecución
#### Requisitos Previos
- .NET 9 SDK: Asegúrate de tener instalado el SDK de .NET 9.
- MySQL: Configura una base de datos MySQL y actualiza la cadena de conexión en local.settings.json.
- Azure Functions Core Tools: Instala las herramientas de Azure Functions para ejecutar el proyecto localmente.

Pasos para Instalar
1. Clona el repositorio:
    ```bash
    git clone https://github.com/Sergip8/libropedia-backend-net.git
2. Restaura las dependencias:
    ```bash
    dotnet restore
3. Configura la base de datos:

- Crea las tablas necesarias ejecutando el script database.sql.
- Actualiza la cadena de conexión en local.settings.json.
Ejecución del Proyecto

#### Ejecución del Proyecto
1. Ejecuta el proyecto localmente:
    ```bash
    func start