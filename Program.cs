using System.Text.Json.Serialization;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Authentication;
using Microsoft.Kiota.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<GraphServiceClient>((sp) =>
{
    var tenantId = builder.Configuration["AzureAd:TenantId"];
    var clientId = builder.Configuration["AzureAd:ClientId"];
    var clientSecret = builder.Configuration["AzureAd:ClientSecret"];
    var scopes = new[] { builder.Configuration["AzureAd:Scopes"] }; // Ensure this is properly configured in your app settings

    var options = new TokenCredentialOptions
    {
        AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
    };

    var clientSecretCredential = new ClientSecretCredential(
        tenantId,
        clientId,
        clientSecret,
        options);

    // Set up GraphServiceClient with the credential and scopes
    var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

    return graphClient;
});
builder.Services.AddSingleton<ISharePointDataService, SharePointDataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
