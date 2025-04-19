namespace bookstore.storeBackNet.Models.Response
{
    public class CommentUserResponse
    {
        public int IdResena { get; set; }
        public int Calificacion { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaResena { get; set; }
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public int AnioPublicacion { get; set; }
        public string Editorial { get; set; }
        public string PortadaUrl { get; set; }
    }
}