namespace EduHome.Exceptions;

public sealed class FileSizeException : Exception
{
    public FileSizeException(string message) : base(message)
    {
    }
}