using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LibraryManagerImageProcessorS3;

public class Function
{
    private readonly IAmazonS3 _s3Client;

    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance.
    /// </summary>
    public Function()
    {
        _s3Client = new AmazonS3Client();
    }

    /// <summary>
    /// Constructor with dependency injection for testing
    /// </summary>
    public Function(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an S3 event object and processes it.
    /// </summary>
    /// <param name="s3Event">The S3 event that contains the information about the image to process</param>
    /// <param name="context">The Lambda context that provides methods for logging</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task FunctionHandler(S3Event s3Event, ILambdaContext context)
    {
        try
        {
            var s3EventRecord = s3Event.Records?[0];
            if (s3EventRecord == null) return;

            var bucketName = s3EventRecord.S3.Bucket.Name;
            var key = s3EventRecord.S3.Object.Key;

            // Log que a função foi acionada
            context.Logger.LogInformation($"Processando arquivo {key} do bucket {bucketName}");

            // Ignorar arquivos que já estão na pasta thumbnails
            if (key.StartsWith("thumbnails/"))
            {
                context.Logger.LogInformation($"Arquivo {key} já está na pasta de thumbnails. Ignorando.");
                return;
            }

            // Verificar se é uma imagem
            if (!IsImageFile(key))
            {
                context.Logger.LogInformation($"Arquivo {key} não é uma imagem. Ignorando.");
                return;
            }

            // Baixar a imagem original do S3
            var response = await _s3Client.GetObjectAsync(bucketName, key);
            using var originalImageStream = response.ResponseStream;
            
            // Nome do arquivo de thumbnailcd
            var fileName = Path.GetFileName(key);
            var thumbnailKey = $"thumbnails/{fileName}";

            // Processar a imagem e criar thumbnail
            using var memoryStream = new MemoryStream();
            using (var image = await Image.LoadAsync(originalImageStream))
            {
                // Redimensionar para 100x100 mantendo a proporção
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(100, 100),
                    Mode = ResizeMode.Max
                }));

                // Salvar a imagem redimensionada em memória
                await image.SaveAsync(memoryStream, image.Metadata.DecodedImageFormat ?? 
                    SixLabors.ImageSharp.Formats.Jpeg.JpegFormat.Instance);
            }

            // Resetar a posição do stream
            memoryStream.Position = 0;

            // Fazer upload do thumbnail para o S3
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = thumbnailKey,
                InputStream = memoryStream,
                ContentType = GetContentType(fileName)
            };

            await _s3Client.PutObjectAsync(putRequest);
            context.Logger.LogInformation($"Thumbnail criado com sucesso: {thumbnailKey}");
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Erro ao processar imagem: {ex.Message}");
            context.Logger.LogError($"StackTrace: {ex.StackTrace}");
            throw;
        }
    }

    private static bool IsImageFile(string key)
    {
        var extension = Path.GetExtension(key).ToLowerInvariant();
        return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif";
    }

    private static string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }
}
