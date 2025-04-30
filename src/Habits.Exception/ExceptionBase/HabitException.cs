namespace Habits.Exception.ExceptionBase;

public abstract class HabitException : SystemException
{
    protected HabitException(string message) : base(message) { }

    public abstract int StatusCode { get; }

    public abstract List<string> GetErrors();
}
