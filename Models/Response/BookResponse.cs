namespace bookstore.storeBackNet.Models.Response
{
    public class BookResponse
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public string Isbn { get; set; }
        public int AnioPublicacion { get; set; }
        public string Resumen { get; set; }
        public string PortadaUrl { get; set; }
        public string Rating { get; set; }
        public string TotalRating { get; set; }
        public int IdAutor { get; set; }
        public string Autor { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
    }

      public class BookTopResponse
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public string PortadaUrl { get; set; }
        public int AnioPublicacion { get; set; }
        public string Rating { get; set; }
        public string TotalRating { get; set; }

        public string Autor { get; set; }
        public string Categoria { get; set; }
    }
}