using System.Text.Json;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions.Serialization;
using Org.OpenAPITools.Models;

public class SharePointDataService : ISharePointDataService
{
    private readonly GraphServiceClient _graphServiceClient;
    private readonly IConfiguration _configuration;

    public SharePointDataService(GraphServiceClient graphServiceClient, IConfiguration configuration)
    {
        _graphServiceClient = graphServiceClient;
        _configuration = configuration;
    }

    public async Task<List<Metadata>> GetFilteredProductMetadataAsync(string productId, string? name, bool? isProtected, List<CategoryEnum>? categories, List<LanguageEnum>? languages)
    {

        var files = await GetProductsMetadataAsync(productId);

        files = files.Where(file => file.Approved).ToList();

        if (name != null)
            files = files.Where(file => file.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

        if (isProtected != true)
            files = files.Where(file => !file.HcpOnly).ToList();


        if (categories != null && categories.Count > 0)
            files = files.Where(file => file.Categories.Intersect(categories).Any()).ToList();

        if (languages != null && languages.Count > 0)
            files = files.Where(file => file.Languages.Intersect(languages).Any()).ToList();

        return files;
    }

    private async Task<List<Metadata>> GetProductsMetadataAsync(string productId)
    {
        var searchResults = await _graphServiceClient.Drives[_configuration["SharePoint:DriveId"]].Items.GetAsync(requestConf =>
        {
            requestConf.QueryParameters.Expand = ["Children"];
            requestConf.QueryParameters.Filter = $"contains(name, '{productId}')";
        });

        var folders = searchResults?.Value?.Where(driveItem => driveItem.Folder != null).ToList() ?? [];

        if (folders.Count == 0)
        {
            throw new KeyNotFoundException("No folder found with the specified name.");
        }

        var folder = folders.First();
        var items = await _graphServiceClient.Drives[_configuration["SharePoint:DriveId"]].Items[folder.Id].Children.GetAsync(requestConfig =>
        {
            requestConfig.QueryParameters.Expand = ["listItem"];
        });

        var files = items.Value.Select(item => MapDriveItemToMetadata(item)).ToList();

        return files;
    }

    private Metadata MapDriveItemToMetadata(DriveItem item)
    {
        string title = null;
        bool approved = false;
        bool hcpOnly = false;
        DateTime publishDate = DateTime.MinValue;
        DateTime modified = DateTime.MinValue;
        string modifiedBy = null;
        int orderNr = 0;
        List<CategoryEnum> categories = new List<CategoryEnum>();
        List<LanguageEnum> languages = new List<LanguageEnum>();

        if (item.ListItem.Fields.AdditionalData.TryGetValue("Title", out var titleObj) && titleObj is string)
            title = titleObj.ToString();

        if (item.ListItem.Fields.AdditionalData.TryGetValue("ModifiedBy", out var modifiedByObj) && modifiedByObj is string)
            modifiedBy = modifiedByObj.ToString();

        if (item.ListItem.Fields.AdditionalData.TryGetValue("Approved", out var approvedObj) && approvedObj is bool)
            approved = (bool)approvedObj;

        if (item.ListItem.Fields.AdditionalData.TryGetValue("HCP_Only", out var hcpOnlyObj) && hcpOnlyObj is bool)
            hcpOnly = (bool)hcpOnlyObj;

        if (item.ListItem.Fields.AdditionalData.TryGetValue("PublishDate", out var publishDateObj) && publishDateObj is DateTime)
            publishDate = (DateTime)publishDateObj;

        if (item.ListItem.Fields.AdditionalData.TryGetValue("Modified", out var modifiedObj) && modifiedObj is DateTime)
            modified = (DateTime)modifiedObj;

        if (item.ListItem.Fields.AdditionalData.TryGetValue("OrderNr", out var orderNrObj) && orderNrObj is decimal)
            orderNr = (int)Math.Floor((decimal)orderNrObj);

        if (item.ListItem.Fields.AdditionalData.TryGetValue("Categories", out var categoriesObj) && categoriesObj is UntypedArray categoriesArray)
        {
            categories = categoriesArray
                .GetValue()
                .Select(category => Enum.Parse<CategoryEnum>(((UntypedString)category).GetValue().ToString()))
                .ToList();
        }

        if (item.ListItem.Fields.AdditionalData.TryGetValue("Language", out var languagesObj) && languagesObj is UntypedArray languagesArray)
        {
            languages = languagesArray
                .GetValue()
                .Select(language => Enum.Parse<LanguageEnum>(((UntypedString)language).GetValue().ToString()))
                .ToList();
        }

        return new Metadata
        {
            Name = item.Name,
            Title = title,
            Approved = approved,
            HcpOnly = hcpOnly,
            PublishDate = publishDate,
            OrderNr = orderNr,
            Categories = categories,
            Languages = languages,
            Modified = modified,
            ModifiedBy = modifiedBy
        };
    }
}
