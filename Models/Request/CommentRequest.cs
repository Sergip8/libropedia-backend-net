namespace bookstore.storeBackNet.Models.Request
{
    public class CommentRequest
    {
        public int IdLibro { get; set; }
        public int IdUsuario { get; set; }
        public int Calificacion { get; set; }
        public string Comentario { get; set; }
    }
     public class CommentUpdateRequest
    {
        public int IdResena { get; set; }
        public int IdUsuario { get; set; }
        public int Calificacion { get; set; }
        public string Comentario { get; set; }
    }
}