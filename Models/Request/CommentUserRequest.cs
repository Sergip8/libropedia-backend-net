namespace bookstore.storeBackNet.Models.Request
{
    public class CommentUserRequest
    {
        public int UserId { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }
}