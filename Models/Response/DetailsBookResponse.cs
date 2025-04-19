public class BookDetailResponse
{
    public long IdLibro { get; set; }
    public string Titulo { get; set; }
    public string Isbn { get; set; }
    public int AnioPublicacion { get; set; }
    public string Editorial { get; set; }
    public string Resumen { get; set; }
    public string PortadaUrl { get; set; }
    
    // Información del autor (podría ser una clase separada si se usa en otros lugares)
    public AutorInfo Autor { get; set; }
    
    // Información de categoría
    public CategoriaInfo Categoria { get; set; }
    
    // Estadísticas de reseñas
    public EstadisticasResenas Estadisticas { get; set; }
    
    // Reseñas recientes
    public List<ResenaDetalle> ResenasRecientes { get; set; } = new();
    
    // Libros relacionados
    public List<LibroRelacionado> LibrosRelacionados { get; set; } = new();
}

// Modelos auxiliares
public class AutorInfo
{
    public long IdAutor { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string NombreCompleto { get; set; }
    public string Biografia { get; set; }
    public string Nacionalidad { get; set; }
}

public class CategoriaInfo
{
    public long IdCategoria { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
}

public class EstadisticasResenas
{
    public decimal? CalificacionPromedio { get; set; }
    public int TotalResenas { get; set; }
    public int CincoEstrellas { get; set; }
    public int CuatroEstrellas { get; set; }
    public int TresEstrellas { get; set; }
    public int DosEstrellas { get; set; }
    public int UnaEstrella { get; set; }
}

public class ResenaDetalle
{
    public long IdResena { get; set; }
    public int Calificacion { get; set; }
    public string Comentario { get; set; }
    public DateTime FechaCreacion { get; set; }
    public UsuarioResena Usuario { get; set; }
}

public class UsuarioResena
{
    public long IdUsuario { get; set; }
    public string NombreUsuario { get; set; }
    public string NombreCompleto { get; set; }
}

public class LibroRelacionado
{
    public long IdLibro { get; set; }
    public string Titulo { get; set; }
    public string PortadaUrl { get; set; }
    public decimal? Rating { get; set; }
    public int AnioPublicacion { get; set; }
    public string Autor { get; set; }
    public string Categoria { get; set; }
    public decimal? CalificacionPromedio { get; set; }
}