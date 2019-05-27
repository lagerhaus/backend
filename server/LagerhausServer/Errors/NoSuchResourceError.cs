namespace Lagerhaus.Errors
{
    public class NoSuchResourceError
    {
        public string code { get; } = "NoSuchResource";
        public string Message { get; set; }

        public NoSuchResourceError(string message)
        {
            this.Message = message;
        }
    }
}
