namespace bookstore.storeBackNet.Models.Request
{
    public class BookRequest
    {
        public string Titulo { get; set; }
        public int IdAutor { get; set; }
        public int IdCategoria { get; set; }
        public string SortBy { get; set; }
        public string Direction { get; set; }

        public int Limite { get; set; }
        public int Offset { get; set; }
    }
}