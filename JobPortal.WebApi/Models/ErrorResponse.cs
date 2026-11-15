namespace JobPortal.WebApi.Models
{
    public class ErrorResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public int StatusCode { get; set; }
    }
}
