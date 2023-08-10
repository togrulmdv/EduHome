namespace EduHome.Utils;

public static class Extentions
{
    public static bool CheckFileType(this IFormFile file, string fileType)
    {
        return file.ContentType.Contains(fileType);
    }

    public static bool CheckFileSize(this IFormFile file, double fileSize)
    {
        return file.Length / 1024 < fileSize;
    }
}