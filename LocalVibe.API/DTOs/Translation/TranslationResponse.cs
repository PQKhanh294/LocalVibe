namespace LocalVibe.API.DTOs.Translation;

public class TranslationResponse
{
    public string OriginalTitle { get; set; } = string.Empty;
    public string TranslatedTitle { get; set; } = string.Empty;
    public string OriginalDescription { get; set; } = string.Empty;
    public string TranslatedDescription { get; set; } = string.Empty;
    public string SourceLanguage { get; set; } = "vi";
    public string TargetLanguage { get; set; } = "en";
    public string Provider { get; set; } = "MyMemory";
}
