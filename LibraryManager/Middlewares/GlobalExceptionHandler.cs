using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        readonly ILogger<ExceptionHandlingMiddleware> _logger;
        readonly IWebHostEnvironment _env;

        public GlobalExceptionHandler(ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Erro: {Message}", exception.Message);

            var (statusCode, title, detail) = exception switch
            {
                // Validações e argumentos
                ArgumentNullException ex => (StatusCodes.Status400BadRequest,
                    "Argumento nulo",
                    $"O parâmetro {ex.ParamName} não pode ser nulo"),

                ArgumentException ex => (StatusCodes.Status400BadRequest,
                    "Argumento inválido",
                    ex.Message),

                ValidationException ex => (StatusCodes.Status400BadRequest,
                    "Erro de validação",
                    ex.Message),

                // Erros de banco de dados
                DbUpdateConcurrencyException => (StatusCodes.Status409Conflict,
                    "Conflito de concorrência",
                    "O registro foi modificado por outro usuário"),

                DbUpdateException ex => (StatusCodes.Status400BadRequest,
                    "Erro ao salvar dados",
                    ex.InnerException?.Message ?? ex.Message),

                // Erros de negócio
                KeyNotFoundException => (StatusCodes.Status404NotFound,
                    "Recurso não encontrado",
                    "O recurso solicitado não existe"),

                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,
                    "Não autorizado",
                    "Você não tem permissão para acessar este recurso"),

                InvalidOperationException => (StatusCodes.Status400BadRequest,
                    "Operação inválida",
                    exception.Message),

                // Timeout e conexão
                TimeoutException => (StatusCodes.Status504GatewayTimeout,
                    "Timeout",
                    "A operação excedeu o tempo limite"),

                HttpRequestException ex => (StatusCodes.Status503ServiceUnavailable,
                    "Serviço indisponível",
                    ex.Message),

                // Caso padrão
                _ => (StatusCodes.Status500InternalServerError,
                    "Erro interno do servidor",
                    "Ocorreu um erro inesperado")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = _env.IsDevelopment() ? $"{detail}\n{exception.StackTrace}" : detail,
                Instance = httpContext.Request.Path,
                Type = exception.GetType().Name
            };

            // Adiciona informações extras em desenvolvimento
            if (_env.IsDevelopment())
            {
                problemDetails.Extensions.Add("ExceptionMessage", exception.Message);
                if (exception.InnerException != null)
                {
                    problemDetails.Extensions.Add("InnerException", exception.InnerException.Message);
                }
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}