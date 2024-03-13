using static Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Client;

public class CatalogClient
{
    private readonly HttpClient _httpClient;
    public CatalogClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemAsync()
    {
        var items = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/items"); 
        return items;
    }
}
