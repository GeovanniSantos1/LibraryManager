using Microsoft.AspNetCore.Http;

public interface IStorageService
{
    Task<(string originalUrl, string thumbnailUrl)> UploadImageAsync(IFormFile file, string fileName);
    Task<bool> DeleteImageAsync(string fileName);
    Task<string> GetImageUrlAsync(string fileName);
}