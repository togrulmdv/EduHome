using EduHome.Exceptions;
using EduHome.Services.Interfaces;
using EduHome.Utils;

namespace EduHome.Services.Implementations;

public class FileService : IFileService
{
    //public async Task<string> CreateFileAsync(IFormFile file, string path, int maxFileSize, string fileType)
    //{
    //    if (!file.CheckFileSize(maxFileSize))
    //        throw new FileSizeException($"Image size can not exceed {maxFileSize} KB");

    //    if (!file.CheckFileType(fileType))
    //        throw new FileTypeException($"File type is not {fileType}");

    //    string fileName = $"{Guid.NewGuid()}-{file.FileName}";
    //    string resultPath = Path.Combine(path, fileName);
    //    using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
    //    {
    //        await file.CopyToAsync(fileStream);
    //    }

    //    return fileName;
    //}

    //public void DeleteFile(string path)
    //{
    //    if (System.IO.File.Exists(path))
    //    {
    //        System.IO.File.Delete(path);
    //    }
    //}

    public async Task<string> CreateFileAsync(IFormFile file, string path)
    {
        if (!file.ContentType.Contains("image/"))
        {
            throw new FileTypeException("Only Images");
        }
        if (file.Length / 1024 > 600)
        {
            throw new FileSizeException("Size too much");
        }
        string FileName = $"{Guid.NewGuid()}-{file.FileName}";
        string ResultPath = Path.Combine(path, FileName);
        using (FileStream fileStream = new FileStream(ResultPath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return FileName;

    }

    public void DeteleFile(string path)
    {
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }
}