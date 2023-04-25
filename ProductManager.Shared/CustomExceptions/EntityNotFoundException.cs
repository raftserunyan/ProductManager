namespace ProductManager.Shared.CustomExceptions
{
    public class EntityNotFoundException : Exception
    {
        public const string DefaultMessage = "Not found.";

        public EntityNotFoundException(string message = DefaultMessage) : base(message)
        {
        }
    }
}
