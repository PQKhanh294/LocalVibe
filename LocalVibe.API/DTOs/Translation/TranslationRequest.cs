namespace LocalVibe.API.DTOs.Translation;

public class TranslationRequest
{
    public string TargetLanguage { get; set; } = "en";  // "en", "fr", "ja"...
}
