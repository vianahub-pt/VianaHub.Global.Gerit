using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;
using System.Text.Json;
using VianaHub.Global.Gerit.Application.Dtos.Base;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Domain.Tools.Notifications;

namespace VianaHub.Global.Gerit.Api.Middleware;

/// <summary>
/// Middleware global para tratamento centralizado de exceções e padronização de respostas de erro.
/// Garante que nenhum detalhe técnico seja exposto aos clientes.
/// Todos os detalhes técnicos são registrados nos logs do Serilog para diagnóstico.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// Inicializa uma nova instância do <see cref="GlobalExceptionMiddleware"/>.
    /// </summary>
    public GlobalExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    /// <summary>
    /// Executa o middleware para tratamento global de exceções.
    /// </summary>
    public async Task InvokeAsync(HttpContext context, INotify notify, ILocalizationService localization)
    {
        try
        {
            await _next(context);
        }
        catch (BadHttpRequestException ex) when (ex.InnerException is JsonException jsonEx)
        {
            // Tratamento específico para JSON malformatado
            var errorId = Guid.NewGuid().ToString("N")[..12];
            Log.Warning(ex, "⚠️ [ERROR-{ErrorId}] JSON malformatado recebido em {Path}", errorId, context.Request.Path);
            await HandleJsonException(context, jsonEx, errorId, notify, localization);
        }
        catch (JsonException jsonEx)
        {
            // Tratamento específico para erros de deserialização JSON
            var errorId = Guid.NewGuid().ToString("N")[..12];
            Log.Warning(jsonEx, "⚠️ [ERROR-{ErrorId}] Erro ao processar JSON em {Path}", errorId, context.Request.Path);
            await HandleJsonException(context, jsonEx, errorId, notify, localization);
        }
        catch (Exception ex)
        {
            // Gerar ID único para rastreamento do erro
            var errorId = Guid.NewGuid().ToString("N")[..12];

            // Log completo e estruturado do erro
            Log.Error(ex,
                "❌ [ERROR-{ErrorId}] Exceção não tratada na API\n" +
                "   📍 Path: {Path}\n" +
                "   🔧 Method: {Method}\n" +
                "   🌐 IP: {RemoteIP}\n" +
                "   🔥 Exception: {ExceptionType}\n" +
                "   💬 Message: {ExceptionMessage}\n" +
                "   👤 User: {UserId}",
                errorId,
                context.Request.Path,
                context.Request.Method,
                context.Connection.RemoteIpAddress?.ToString(),
                ex.GetType().Name,
                ex.Message,
                context.User?.Identity?.Name ?? "Anônimo");

            await HandleExceptionAsync(context, ex, errorId, notify, localization);
        }
    }

    /// <summary>
    /// Trata especificamente exceções de JSON malformatado com mensagens amigáveis.
    /// </summary>
    private async Task HandleJsonException(HttpContext context, JsonException jsonEx, string errorId, INotify notify, ILocalizationService localization)
    {
        // Evitar reprocessamento se a resposta já começou
        if (context.Response.HasStarted)
        {
            Log.Warning("⚠️ [ERROR-{ErrorId}] Resposta já iniciada, não é possível modificar", errorId);
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json; charset=utf-8";

        string friendlyMessage = GetFriendlyJsonErrorMessage(jsonEx, localization);
        notify.Add(friendlyMessage, (int)HttpStatusCode.BadRequest);

        var errorResponse = new ErrorResponse(localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.JsonFormatError"));
        errorResponse.AddError(localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.FieldLabel.Format"), friendlyMessage);
        errorResponse.AddError(localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.FieldLabel.ErrorId"), localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.ContactSupport", errorId));

        var lineNumber = GetLineNumber(jsonEx);
        var position = GetBytePosition(jsonEx);

        if (lineNumber.HasValue)
            errorResponse.AddError(localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.FieldLabel.Line"), lineNumber.Value.ToString());

        if (position.HasValue)
            errorResponse.AddError(localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.FieldLabel.Position"), position.Value.ToString());

        var json = JsonSerializer.Serialize(errorResponse, GetJsonSerializerOptions());
        await context.Response.WriteAsync(json);
    }

    /// <summary>
    /// Trata a exceção e retorna uma resposta padronizada ao cliente.
    /// </summary>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception, string errorId, INotify notify, ILocalizationService localization)
    {
        // Evitar reprocessamento se a resposta já começou
        if (context.Response.HasStarted)
        {
            Log.Warning("⚠️ [ERROR-{ErrorId}] Resposta já iniciada, não é possível modificar", errorId);
            return;
        }

        context.Response.ContentType = "application/json; charset=utf-8";

        // Tratamento específico para SqlException
        if (exception is SqlException sqlEx)
        {
            // Logar detalhes completos internos para diagnóstico
            Log.Error(sqlEx,
                "[ERROR-{ErrorId}] SqlException detalhada:\n" +
                "   🔢 Error Number: {ErrorNumber}\n" +
                "   📝 Message: {Message}\n" +
                "   🔧 Procedure: {Procedure}\n" +
                "   📍 LineNumber: {LineNumber}",
                errorId,
                sqlEx.Number,
                sqlEx.Message,
                sqlEx.Procedure ?? "N/A",
                sqlEx.LineNumber);

            notify.Add(localization.GetMessage("Api.Middleware.GlobalException.SqlException.Error.DatabaseError"), 500);

            var statusCode = (int)notify.GetStatusCode();
            context.Response.StatusCode = statusCode;

            var sqlErrorResponse = new ErrorResponse(GetErrorTitle(statusCode, localization));
            sqlErrorResponse.AddError(localization.GetMessage("Api.Middleware.GlobalException.SqlException.Error.FieldLabel.System"), localization.GetMessage("Api.Middleware.GlobalException.SqlException.Error.SystemError"));
            sqlErrorResponse.AddError(localization.GetMessage("Api.Middleware.GlobalException.SqlException.Error.FieldLabel.ErrorId"), localization.GetMessage("Api.Middleware.GlobalException.SqlException.Error.ContactSupport", errorId));

            var jsonSql = JsonSerializer.Serialize(sqlErrorResponse, GetJsonSerializerOptions());
            await context.Response.WriteAsync(jsonSql);
            return;
        }

        // Tratamento específico para exceções de atualização do EF Core
        if (exception is DbUpdateException dbEx)
        {
            // Inspecionar inner exception em busca de indicadores de violação de restrições
            var innerMessage = dbEx.InnerException?.Message ?? string.Empty;

            // Logar detalhes completos internos para diagnóstico
            Log.Error(dbEx,
                "[ERROR-{ErrorId}] DbUpdateException detalhada:\n" +
                "   📝 Message: {Message}\n" +
                "   🔥 Inner Exception: {InnerException}\n" +
                "   📋 Entries: {EntriesCount}",
                errorId,
                dbEx.Message,
                dbEx.InnerException?.Message ?? "N/A",
                dbEx.Entries?.Count() ?? 0);

            if (innerMessage.IndexOf("FK_", StringComparison.OrdinalIgnoreCase) >= 0 ||
                innerMessage.IndexOf("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                notify.Add(localization.GetMessage("Api.Middleware.GlobalException.DbUpdateException.Error.TenantNotExistOrInactive"), 400);
            }
            else if (innerMessage.IndexOf("UNIQUE", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     innerMessage.IndexOf("duplicate", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                notify.Add(localization.GetMessage("Api.Middleware.GlobalException.DbUpdateException.Error.DuplicateApplication"), 409);
            }
            else
            {
                notify.Add(localization.GetMessage("Api.Middleware.GlobalException.DbUpdateException.Error.SaveDataError"), 400);
            }

            // Construir resposta a partir do notify para retornar mensagens amigáveis ao consumidor
            var statusCode = (int)notify.GetStatusCode();
            context.Response.StatusCode = statusCode;

            var efErrorResponse = new ErrorResponse(GetErrorTitle(statusCode, localization));
            efErrorResponse.AddError(localization.GetMessage("Api.Middleware.GlobalException.DbUpdateException.Error.FieldLabel.ErrorId.ErrorResponse"), localization.GetMessage("Api.Middleware.GlobalException.DbUpdateException.Error.ContactSupport.ErrorResponse", errorId));

            foreach (var message in notify.GetErrorMessage())
            {
                if (message.Contains(":"))
                {
                    var parts = message.Split(':', 2);
                    efErrorResponse.AddError(parts[0].Trim(), parts[1].Trim());
                }
                else
                {
                    efErrorResponse.AddError(localization.GetMessage("Api.Middleware.GlobalException.DbUpdateException.Error.FieldLabel.Error"), message);
                }
            }

            var jsonEf = JsonSerializer.Serialize(efErrorResponse, GetJsonSerializerOptions());
            await context.Response.WriteAsync(jsonEf);
            return;
        }

        // Tratamento genérico para erros inesperados
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Log detalhado para diagnóstico
        Log.Error(exception,
            "[ERROR-{ErrorId}] Exceção genérica detalhada:\n" +
            "   🔥 Exception Type: {ExceptionType}\n" +
            "   📝 Message: {Message}\n" +
            "   🔗 Inner Exception: {InnerException}\n" +
            "   📚 Stack Trace: {StackTrace}",
            errorId,
            exception.GetType().FullName,
            exception.Message,
            exception.InnerException?.Message ?? "N/A",
            exception.StackTrace ?? "N/A");

        // Adicionar notificação amigável
        notify.Add(localization.GetMessage("Api.Middleware.GlobalException.DbUpdateException.Error.UnexpectedError"), 500);

        // Criar resposta padronizada - APENAS informações seguras para o cliente
        var errorResponse = new ErrorResponse(localization.GetMessage("Api.Middleware.GlobalException.DbUpdateException.Error.InternalServerError"));

        // Mensagem amigável para o usuário
        errorResponse.AddError(localization.GetMessage("Api.Middleware.GlobalException.DbUpdateException.Error.FieldLabel.System"),
            localization.GetMessage("Api.Middleware.GlobalException.DbUpdateException.Error.UnexpectedError.User"));

        // Adicionar ID do erro para rastreamento
        errorResponse.AddError(localization.GetMessage("Api.Middleware.GlobalException.DbUpdateException.Error.FieldLabel.ErrorId"), localization.GetMessage("Api.Middleware.GlobalException.DbUpdateException.Error.ContactSupport", errorId));

        // Serializar e retornar resposta
        var json = JsonSerializer.Serialize(errorResponse, GetJsonSerializerOptions());
        await context.Response.WriteAsync(json);
    }

    /// <summary>
    /// Retorna uma mensagem amigável baseada no tipo de erro JSON.
    /// </summary>
    private static string GetFriendlyJsonErrorMessage(JsonException jsonEx, ILocalizationService localization)
    {
        var message = jsonEx.Message.ToLower();

        if (message.Contains("trailing comma"))
            return localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.JsonTrailingComma");

        if (message.Contains("unexpected character") || message.Contains("invalid character"))
            return localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.JsonInvalidCharacter");

        if (message.Contains("unterminated string"))
            return localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.JsonUnterminatedString");

        if (message.Contains("expected") && message.Contains("got"))
            return localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.JsonMalformed");

        if (message.Contains("depth"))
            return localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.JsonTooManyLevels");

        if (message.Contains("property name"))
            return localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.JsonInvalidPropertyName");

        if (message.Contains("missing"))
            return localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.JsonIncomplete");

        return localization.GetMessage("Api.Middleware.GlobalException.JsonException.Error.JsonGeneric");
    }

    /// <summary>
    /// Extrai o número da linha do erro JSON, se disponível.
    /// </summary>
    private static int? GetLineNumber(JsonException jsonEx)
    {
        try
        {
            var lineNumberProperty = jsonEx.GetType().GetProperty("LineNumber");
            if (lineNumberProperty != null)
            {
                var value = lineNumberProperty.GetValue(jsonEx);
                if (value != null)
                {
                    return Convert.ToInt32(value);
                }
            }
        }
        catch
        {
            // Ignora erros ao tentar extrair informação de linha
        }
        return null;
    }

    /// <summary>
    /// Extrai a posição do byte na linha do erro JSON, se disponível.
    /// </summary>
    private static int? GetBytePosition(JsonException jsonEx)
    {
        try
        {
            var bytePositionProperty = jsonEx.GetType().GetProperty("BytePositionInLine");
            if (bytePositionProperty != null)
            {
                var value = bytePositionProperty.GetValue(jsonEx);
                if (value != null)
                {
                    return Convert.ToInt32(value);
                }
            }
        }
        catch
        {
            // Ignora erros ao tentar extrair informação de posição
        }
        return null;
    }

    /// <summary>
    /// Retorna as opções de serialização JSON padronizadas.
    /// </summary>
    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    /// <summary>
    /// Retorna um título amigável baseado no código de status HTTP.
    /// </summary>
    private static string GetErrorTitle(int statusCode, ILocalizationService localization)
    {
        return statusCode switch
        {
            400 => localization.GetMessage("Api.Middleware.GlobalException.StatusCode.Error.InvalidRequest"),
            401 => localization.GetMessage("Api.Middleware.GlobalException.StatusCode.Error.Unauthorized"),
            403 => localization.GetMessage("Api.Middleware.GlobalException.StatusCode.Error.Forbidden"),
            404 => localization.GetMessage("Api.Middleware.GlobalException.StatusCode.Error.NotFound"),
            409 => localization.GetMessage("Api.Middleware.GlobalException.StatusCode.Error.Conflict"),
            500 => localization.GetMessage("Api.Middleware.GlobalException.StatusCode.Error.InternalServerError"),
            _ => localization.GetMessage("Api.Middleware.GlobalException.StatusCode.Error.GenericError")
        };
    }
}
