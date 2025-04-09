using Amazon.S3;
using Amazon.S3.Model;
using LibraryManager.Core.Interfaces;
using LibraryManager.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace LibraryManager.Infrastructure.Services
{
    public class S3StorageService : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly ILogger<S3StorageService> _logger;

        public S3StorageService(AWSS3Settings settings, ILogger<S3StorageService> logger)
        {
            _logger = logger;
            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(settings.Region)
            };

            _s3Client = new AmazonS3Client(
                settings.AccessKey,
                settings.SecretKey,
                s3Config
            );

            _bucketName = settings.BucketName;
        }

        public async Task<(string originalUrl, string thumbnailUrl)> UploadImageAsync(IFormFile file, string fileName)
        {
            try
            {
                _logger.LogInformation($"Iniciando upload de {fileName} para o bucket {_bucketName}");
                
                // Garanta que o caminho contenha o prefixo 'books/'
                var key = $"books/{fileName}";
                _logger.LogInformation($"Chave de arquivo gerada: {key}");

                // Gera as URLs primeiro
                var originalUrl = $"https://{_bucketName}.s3.{_s3Client.Config.RegionEndpoint.SystemName}.amazonaws.com/{key}";
                var thumbnailUrl = $"https://{_bucketName}.s3.{_s3Client.Config.RegionEndpoint.SystemName}.amazonaws.com/thumbnails/{fileName}";

                // Faz o upload
                using var stream = file.OpenReadStream();
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key,
                    InputStream = stream,
                    ContentType = file.ContentType
                };

                await _s3Client.PutObjectAsync(putRequest);
                
                _logger.LogInformation($"Upload concluído com sucesso para {key}");
                return (originalUrl, thumbnailUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao fazer upload: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteImageAsync(string fileName)
        {
            try
            {
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = $"books/{fileName}"
                };

                var response = await _s3Client.DeleteObjectAsync(deleteRequest);
                
                // Tenta deletar a thumbnail também
                try
                {
                    var deleteThumbnailRequest = new DeleteObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = $"thumbnails/{fileName}"
                    };
                    await _s3Client.DeleteObjectAsync(deleteThumbnailRequest);
                }
                catch
                {
                    // Ignora erro ao deletar thumbnail
                }

                return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar imagem: {ex.Message}", ex);
            }
        }

        public async Task<string> GetImageUrlAsync(string fileName)
        {
            try
            {
                var getRequest = new GetObjectMetadataRequest
                {
                    BucketName = _bucketName,
                    Key = fileName // Usa o caminho completo passado
                };

                await _s3Client.GetObjectMetadataAsync(getRequest);
                return $"https://{_bucketName}.s3.{_s3Client.Config.RegionEndpoint.SystemName}.amazonaws.com/{fileName}";
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new FileNotFoundException($"Arquivo não encontrado: {fileName}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter URL da imagem: {ex.Message}", ex);
            }
        }

        public async Task<string> UploadBookImageAsync(Stream imageStream, string fileName)
        {
            // Limpe o nome do arquivo para remover caracteres problemáticos
            var safeFileName = Path.GetFileNameWithoutExtension(fileName)
                .Replace(" ", "-")
                .Replace("(", "")
                .Replace(")", ""); 
            var extension = Path.GetExtension(fileName);
            
            // Gere um nome único
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            
            // Garanta que o caminho tenha o prefixo 'books/'
            var key = $"books/{uniqueFileName}";
            
            // Configurar o pedido de upload
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = imageStream,
                ContentType = GetContentType(extension)
            };
            
            // Fazer upload para o S3
            await _s3Client.PutObjectAsync(putRequest);
            
            // Retornar o caminho completo do arquivo original
            return key;
        }

        // Método para obter a URL do thumbnail
        public string GetThumbnailUrl(string imageKey)
        {
            if (string.IsNullOrEmpty(imageKey))
                return null;
            
            // Converter o caminho books/imagem.jpg para thumbnails/imagem.jpg
            var fileName = Path.GetFileName(imageKey);
            var thumbnailKey = $"thumbnails/{fileName}";
            
            // Gerar e retornar a URL pública (ou presigned URL) do thumbnail
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = thumbnailKey,
                Expires = DateTime.UtcNow.AddHours(1) // URL válida por 1 hora
            };
            
            return _s3Client.GetPreSignedURL(request);
        }

        private string GetContentType(string fileExtension)
        {
            return fileExtension.ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream" // tipo padrão para arquivos desconhecidos
            };
        }
    }
} 