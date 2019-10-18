namespace SearchSuggestions.WebAPI.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SearchEngine;
    using Types;

    /// <summary>
    /// Controller for city objects
    /// </summary>
    [Route("")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly CitySearchEngine searchEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="CitiesController" /> class
        /// </summary>
        /// <param name="searchEngine">The search engine instance</param>
        public CitiesController(CitySearchEngine searchEngine)
        {
            this.searchEngine = searchEngine;
        }

        /// <summary>
        /// Gets city suggestions given search criteria
        /// </summary>
        /// <param name="query">The query string</param>
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude</param>
        /// <returns>The search results</returns>
        [HttpGet("suggestions")]
        [ProducesResponseType(typeof(CitySuggestionResults), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CitySuggestionResults>> Search(
            [FromQuery(Name = "q")] string query,
            [FromQuery] double? latitude,
            [FromQuery] double? longitude)
        {
            var results = await this.searchEngine.Search(query, new LocationInformation(latitude, longitude));

            if (!results.IsSuccess)
            {
                return this.BadRequest(results.Errors);
            }

            return this.Ok(results.Results);
        }
    }
}