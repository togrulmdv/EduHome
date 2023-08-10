namespace EduHome.Exceptions;

public sealed class FileTypeException : Exception
{
    public FileTypeException(string message) : base(message)
    {
    }
}