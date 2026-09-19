using System.Net.Http.Headers;
using System.Net.Http.Json;
using DigiCard.Contracts.Media;
using Microsoft.AspNetCore.Components.Forms;

namespace DigiCard.Web.Client.Editor;

/// <summary>
/// Sends a chosen file to the server and hands back what it became.
///
/// Everything that decides whether the file is acceptable happens on the server - this only
/// refuses what is obviously too big, so someone on a slow connection is told before spending
/// two minutes uploading it rather than after.
/// </summary>
public sealed class MediaUploader(HttpClient http)
{
    public const long MaxBytes = 12 * 1024 * 1024;

    public sealed record Result(MediaSummary? Media, string? Error)
    {
        public static Result Failed(string message) => new(null, message);
    }

    public async Task<Result> UploadAsync(IBrowserFile file, string kind, CancellationToken ct = default)
    {
        if (file.Size > MaxBytes) return Result.Failed("حجم فایل بیش از ۱۲ مگابایت است.");

        try
        {
            var token = await TokenAsync(ct);
            if (token is null) return Result.Failed("برای آپلود وارد حساب خود شوید.");

            using var content = new MultipartFormDataContent();

            // OpenReadStream defaults to half a megabyte, which quietly rejects every real photo.
            var stream = file.OpenReadStream(MaxBytes, ct);
            using var part = new StreamContent(stream);
            part.Headers.ContentType = new MediaTypeHeaderValue(
                string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType);

            content.Add(part, "file", file.Name);
            content.Add(new StringContent(kind), "kind");

            using var request = new HttpRequestMessage(HttpMethod.Post, "api/media/") { Content = content };
            request.Headers.Add("X-CSRF-TOKEN", token);

            using var response = await http.SendAsync(request, ct);

            if (response.IsSuccessStatusCode)
            {
                var media = await ReadJsonAsync<MediaSummary>(response, ct);
                return media is null ? Result.Failed("پاسخ سرور خوانده نشد.") : new Result(media, null);
            }

            if (response.StatusCode is System.Net.HttpStatusCode.Unauthorized
                or System.Net.HttpStatusCode.Forbidden)
                return Result.Failed("برای آپلود وارد حساب خود شوید.");

            var problem = await ReadJsonAsync<ErrorBody>(response, ct);
            return Result.Failed(problem?.Error ?? "آپلود انجام نشد.");
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return Result.Failed("آپلود انجام نشد. دوباره تلاش کنید.");
        }
    }

    private async Task<string?> TokenAsync(CancellationToken ct)
    {
        using var response = await http.GetAsync("api/antiforgery", ct);
        if (!response.IsSuccessStatusCode) return null;

        var token = await ReadJsonAsync<CsrfToken>(response, ct);
        return token?.Token;
    }

    /// <summary>
    /// Not ReadFromJsonAsync: a body that is not JSON is a normal outcome here, not an exception -
    /// and an exception thrown out of an upload handler freezes the UI rather than reporting.
    /// </summary>
    private static async Task<T?> ReadJsonAsync<T>(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.Content.Headers.ContentType?.MediaType != "application/json") return default;

        try
        {
            return await response.Content.ReadFromJsonAsync<T>(ct);
        }
        catch (System.Text.Json.JsonException)
        {
            return default;
        }
    }

    private sealed record CsrfToken(string Token);

    private sealed record ErrorBody(string Error);
}
