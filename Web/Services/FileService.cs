using File = Web.Data.Entities.File;

namespace Web.Services;

public class FileService
{
    private const string RootPath = "wwwroot/uploads";
    private const int BufferSize = 81920;

    public async Task<File> CreateFileAsync(string folderPath, IFormFile file, CancellationToken ct)
    {
        var directoryPath = Path.Combine(RootPath, folderPath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var originalName = Path.GetFileName(file.FileName.Replace("\\", "/"));
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(folderPath, fileName);

        await SaveFileAsync(file.OpenReadStream(), Path.Combine(directoryPath, fileName), ct);

        return new File
        {
            Name = originalName,
            Path = filePath,
            ContentType = file.ContentType
        };
    }

    public void DeleteFile(File file)
    {
        var filePath = Path.Combine(RootPath, file.Path);
        System.IO.File.Delete(filePath);
    }

    public Stream LoadFile(string path) =>
        System.IO.File.OpenRead(Path.Combine(RootPath, path));

    private async Task SaveFileAsync(Stream stream, string path, CancellationToken ct)
    {
        await using var fileStream = new FileStream(path, FileMode.CreateNew);
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        await stream.CopyToAsync(fileStream, BufferSize, ct);
    }
}