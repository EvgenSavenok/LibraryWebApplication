namespace Application.Validation.CustomExceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string message) : base(message) { }
}
