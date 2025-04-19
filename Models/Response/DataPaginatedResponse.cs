namespace ConsultorioNet.Models.Response
{
    public class DataPaginatedResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalRecords { get; set; }

    }
    
}