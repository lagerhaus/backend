namespace Lagerhaus.Errors
{
    public class ValidationError : Error
    {
        public ValidationError(string message) : base(message)
        {
        }
    }
}