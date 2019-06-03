namespace Lagerhaus.Errors
{
    public class DuplicateKeyError : DatabaseError
    {
        public DuplicateKeyError(string message) : base(message)
        {
        }
    }
}