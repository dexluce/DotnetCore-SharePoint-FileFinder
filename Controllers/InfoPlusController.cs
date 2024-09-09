using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace InfoPlusApi.Controllers;

[ApiController]
[Route("[controller]")]
public class InfoPlusController : ControllerBase
{
    private readonly ISharePointDataService _sharePointDataService;

    public InfoPlusController(ISharePointDataService sharePointDataService)
    {
        _sharePointDataService = sharePointDataService;
    }


    /// <summary>
    /// Retrieves metadata for a specific product
    /// </summary>
    /// <remarks>Returns all metadata related to a single product identified by its ID.</remarks>
    /// <param name="productId">Unique identifier of the product</param>
    /// <param name="name">Filter by resource name using plain text search</param>
    /// <Param name="categories">Filter by category</Param>
    /// <Param name="languages">Filter by language</Param>
    /// <response code="200">Successful operation</response>
    /// <response code="404">Product not found</response>
    [HttpGet]
    [Route("/v1/products/{productId}/metadata")]
    [SwaggerOperation("GetProductMetadata")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<Metadata>), description: "Successful operation")]
    public virtual async Task<IActionResult> GetProductMetadataAsync([FromRoute(Name = "productId")][Required] string productId, [FromQuery(Name = "name")][Optional] string? name, [FromQuery(Name = "categories")][Optional] List<CategoryEnum>? categories, [FromQuery(Name = "languages")][Optional] List<LanguageEnum>? languages)
    {
        var isProtected = false; // is user authenticated and authorized to view the content?
        var files = await _sharePointDataService.GetFilteredProductMetadataAsync(productId, name, isProtected, categories, languages);

        return Ok(files);
    }
}
