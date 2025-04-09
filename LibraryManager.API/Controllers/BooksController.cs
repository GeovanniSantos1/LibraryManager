using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using LibraryManager.Infrastructure.Services;

namespace LibraryManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IS3StorageService _storageService;

        public BooksController(IS3StorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido");
        
            using var stream = file.OpenReadStream();
            var imageKey = await _storageService.UploadBookImageAsync(stream, file.FileName);
        
            // Corrija aqui para usar o método assíncrono
            var imageUrl = await _storageService.GetImageUrlAsync(imageKey);
            var thumbnailUrl = _storageService.GetThumbnailUrl(imageKey);
        
            return Ok(new 
            { 
                imageKey,
                imageUrl,
                thumbnailUrl
            });
        }
    }
} 