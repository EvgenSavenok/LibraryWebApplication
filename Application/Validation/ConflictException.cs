namespace Application.Validation;

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
