namespace Barbearia.Models
{
    public class ResponseModel<T>
    {
        public T? Dados { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
    }
}
