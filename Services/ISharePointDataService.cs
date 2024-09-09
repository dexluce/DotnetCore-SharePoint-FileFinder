using Org.OpenAPITools.Models;

public interface ISharePointDataService
{
    Task<List<Metadata>> GetFilteredProductMetadataAsync(string productId, string? name, bool? isProtected, List<CategoryEnum>? categories, List<LanguageEnum>? languages);
}
