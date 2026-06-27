namespace LocalVibe.API.DTOs.Photos;

public class PhotoItem
{
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string FullUrl { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;  // link về Pixabay để credit
}

public class PhotoSuggestionResponse
{
    public string Query { get; set; } = string.Empty;
    public IEnumerable<PhotoItem> Photos { get; set; } = Enumerable.Empty<PhotoItem>();
}
