namespace SearchSuggestions.WebAPI.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Types;

    /// <summary>
    /// Controller for city objects
    /// </summary>
    [Route("")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        /// <summary>
        /// Gets city suggestions given search criteria
        /// </summary>
        /// <param name="query">The query string</param>
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude</param>
        /// <returns>The search results</returns>
        [HttpGet("suggestions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CitySuggestionResults>> Search(
            [FromQuery(Name = "q")] string query,
            [FromQuery] double? latitude,
            [FromQuery] double? longitude)
        {
            if (string.IsNullOrWhiteSpace(query))
                return this.BadRequest("Query parameter is required");

            return this.Ok(new List<string>());
        }
    }
}