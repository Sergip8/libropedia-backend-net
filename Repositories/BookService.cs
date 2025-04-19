

using System.Data;
using Api.FunctionApp.DataContext;
using bookstore.Repositories.Interfaces;
using bookstore.storeBackNet.Models.Request;
using bookstore.storeBackNet.Models.Response;
using ConsultorioNet.Models.Response;
using Dapper;
using Newtonsoft.Json;

namespace bookstore.storeBackNet.Repositories
{
    public class BookService : IBookInterface
    {

        private readonly DapperContext _context;
       
        public BookService(DapperContext context)
    {
        _context = context;
  
    }

        public async Task<DataPaginatedResponse<BookResponse>> FilterBookAsync(BookRequest bookRequest)
        {
            
            var result = new DataPaginatedResponse<BookResponse>{};
            Console.WriteLine(JsonConvert.SerializeObject(bookRequest));
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("p_titulo", bookRequest.Titulo, DbType.String);
            parameters.Add("p_id_autor", bookRequest.IdAutor, DbType.Int32);
            parameters.Add("p_id_categoria", bookRequest.IdCategoria, DbType.Int32);
            parameters.Add("p_orden", bookRequest.SortBy, DbType.String);
            parameters.Add("p_direccion", bookRequest.Direction, DbType.String);
            parameters.Add("p_limite", bookRequest.Limite, DbType.Int32);
            parameters.Add("p_offset", bookRequest.Offset, DbType.Int32);

            var multi = connection.QueryMultiple(
                "sp_buscar_libros",
                parameters,
                commandType: CommandType.StoredProcedure
            );
                result.Data = (await multi.ReadAsync<BookResponse>()).ToList();
                result.TotalRecords = (await multi.ReadAsync<int>()).FirstOrDefault();
     
            return result;
        }

        public async Task<BookDetailResponse> GetBookDetail(long id)
        {
            using var connection = _context.CreateConnection();
    
    using var multi = await connection.QueryMultipleAsync(
        "sp_obtener_detalle_libro",
        new { p_id_libro = id },
        commandType: CommandType.StoredProcedure);

    // Mapear los resultados
    var libroDetalle = await multi.ReadSingleAsync<dynamic>();
    var resenas = await multi.ReadAsync<dynamic>();
    var relacionados = await multi.ReadAsync<dynamic>();

    // Construir el objeto completo
    return new BookDetailResponse
    {
        IdLibro = libroDetalle.id_libro,
        Titulo = libroDetalle.titulo,
        Isbn = libroDetalle.isbn,
        AnioPublicacion = (int)libroDetalle.anio_publicacion,
        Editorial = libroDetalle.editorial,
        Resumen = libroDetalle.resumen,
        PortadaUrl = libroDetalle.portada_url,
        Autor = new AutorInfo
        {
            IdAutor = libroDetalle.autor_id,
            Nombre = libroDetalle.autor_nombre,
            Apellido = libroDetalle.autor_apellido,
            NombreCompleto = libroDetalle.autor_nombre_completo,
            Biografia = libroDetalle.autor_biografia,
            Nacionalidad = libroDetalle.autor_nacionalidad
        },
        Categoria = new CategoriaInfo
        {
            IdCategoria = libroDetalle.categoria_id,
            Nombre = libroDetalle.categoria_nombre,
            Descripcion = libroDetalle.categoria_descripcion
        },
        Estadisticas = new EstadisticasResenas
        {
            CalificacionPromedio = libroDetalle.calificacion_promedio,
            TotalResenas = (int)libroDetalle.total_resenas,
            CincoEstrellas = (int)libroDetalle.cinco_estrellas,
            CuatroEstrellas = (int)libroDetalle.cuatro_estrellas,
            TresEstrellas = (int)libroDetalle.tres_estrellas,
            DosEstrellas = (int)libroDetalle.dos_estrellas,
            UnaEstrella = (int)libroDetalle.una_estrella
        },
        ResenasRecientes = resenas.Select(r => new ResenaDetalle
        {
            IdResena = r.id_resena,
            Calificacion = (int)r.calificacion,
            Comentario = r.comentario,
            FechaCreacion = r.fecha_creacion,
            Usuario = new UsuarioResena
            {
                IdUsuario = r.usuario_id,
                NombreUsuario = r.usuario_nombre_usuario,
                NombreCompleto = r.usuario_nombre_completo
            }
        }).ToList(),
        LibrosRelacionados = relacionados.Select(l => new LibroRelacionado
        {
            IdLibro = l.id_libro,
            Titulo = l.titulo,
            PortadaUrl = l.portada_url,
            Autor = l.autor,
            Categoria = l.categoria,
            Rating = l.calificacion_promedio,
            AnioPublicacion = l.anio_publicacion
        }).ToList()
    };
        }

        public Task<IEnumerable<BookResponse>> GetBookTopQualifications(int limit)
        {
             using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("p_limite", limit, DbType.Int16);
        

            return connection.QueryAsync<BookResponse>(
                "sp_obtener_top_libros_calificados",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }

}