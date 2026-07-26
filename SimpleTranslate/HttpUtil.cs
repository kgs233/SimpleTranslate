using System.Net.Http.Json;
using System.Text.Json.Serialization;

using Microsoft.UI.Dispatching;

namespace SimpleTranslate;

public record TranslationRequest
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = "";

    [JsonPropertyName("source_lang")]
    public string SourceLang { get; set; } = "zho_Hans";

    [JsonPropertyName("target_lang")]
    public string TargetLang { get; set; } = "eng_Latn";
}


public record TranslationResponse
{
    [JsonPropertyName("translated_text")]
    public string TranslatedText { get; set; } = "";

    [JsonPropertyName("source_lang")]
    public string SourceLang { get; set; } = "";

    [JsonPropertyName("target_lang")]
    public string TargetLang { get; set; } = "";
}


public static class HttpUtil
{
    private static readonly HttpClient client = new();


    public static async Task SendTranslation()
    {
        if (TranslateState.Instance.SourceText == "" ||
            TranslateState.Instance.SourceLanguage == "" ||
            TranslateState.Instance.TargetLanguage == "")
        {
            return;
        }

        try
        {
            TranslateState.Instance.IsTranslating = true;
            var request = new TranslationRequest
            {
                Text = TranslateState.Instance.SourceText,
                SourceLang = TranslateState.Instance.SourceLanguage,
                TargetLang = TranslateState.Instance.TargetLanguage
            };

            string url = $"http://{SettingsModel.API}/translate";

            HttpResponseMessage response = await client.PostAsJsonAsync(
                url,
                request
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Translation API returned {response.StatusCode}"
                );
            }

            TranslationResponse? result =
                await response.Content.ReadFromJsonAsync<TranslationResponse>();

            TranslateState.Instance.ResultText = result?.TranslatedText ?? "";
        }
        finally
        {
            TranslateState.Instance.IsTranslating = false;
        }
    }
}
