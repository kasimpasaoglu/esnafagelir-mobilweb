using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, string folderName);
    Task<bool> DeleteFileAsync(string filePath);
}

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folderName)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is invalid");

        // Klasör yolu oluştur
        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "uploads", folderName);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Benzersiz dosya adı oluştur
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Dosyayı kaydet
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await file.CopyToAsync(fileStream);
        }

        return Path.Combine("/images", "uploads", folderName, uniqueFileName); // Klasör ile birlikte yolu döndür
    }

    public async Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
        }
        catch (Exception ex)
        {
            // Log mekanizması varsa buraya ekleyebilirsin
            Console.WriteLine($"Dosya silme hatası: {ex.Message}");
        }

        return false;
    }
}
