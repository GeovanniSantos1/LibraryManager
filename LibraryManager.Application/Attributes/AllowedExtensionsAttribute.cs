using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!_extensions.Contains(extension.ToLower()))
            {
                return new ValidationResult($"Apenas os formatos {string.Join(", ", _extensions)} são permitidos!");
            }
        }

        return ValidationResult.Success;
    }
} 