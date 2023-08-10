namespace EduHome.Services.Interfaces;

public interface IFileService
{
    Task<string> CreateFileAsync(IFormFile file, string path, int maxFileSize, string fileType);
    void DeleteFile(string path);
}
