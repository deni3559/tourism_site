using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace WebPortal.Services.Apis
{
    public class WikiPageApi
    {
        private readonly HttpClient _httpClient;

        public WikiPageApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WikiPageDto> GetWikiPageAboutYourPlace(string place)
        {
            var response = await _httpClient.GetAsync($"/api/rest_v1/page/summary/{place}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
                
            return await response.Content.ReadFromJsonAsync<WikiPageDto>(); ;
        }

        public class WikiPageDto
        {
            public string Title { get; set; }
            public string Description { get; set; }
            [JsonPropertyName("content_urls")]
            public ContentUrl ContentUrls { get; set; }
        }

        public class ContentUrl
        {
            public DesktopContentUrl Desktop { get; set; }
        }

        public class DesktopContentUrl
        {
            public string Page { get; set; }
        }

    }
}
