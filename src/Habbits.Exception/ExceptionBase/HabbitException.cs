namespace Habbits.Exception.ExceptionBase;

public abstract class HabbitException : SystemException
{
    protected HabbitException(string message) : base(message) { }

    public abstract int StatusCode { get; }

    public abstract List<string> GetErrors();
}
