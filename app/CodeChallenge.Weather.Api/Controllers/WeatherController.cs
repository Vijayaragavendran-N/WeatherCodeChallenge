namespace CodeChallenge.Weather.Api.Controllers
{
    using CodeChallenge.Weather.Api.Model;
    using CodeChallenge.Weather.Domain;
    using CodeChallenge.Weather.Domain.Service;
    using CodeChallenge.Weather.Infrastructure.EntityFramework;
    using CodeChallenge.Weather.Infrastructure.OpenWeatherMap;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Weather API v1
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly WeatherContext _weatherContext;
        /// <summary>
        /// Constructor of weather controller
        /// </summary>
        /// <param name="logger"></param>
        public WeatherController(ILogger<WeatherController> logger, WeatherContext weatherContext)
        {
            _logger = logger;
            _weatherContext = weatherContext;
        }

        /// <summary>
        /// Get the information from the repository about the city, when is bad weather also includes the sensor(s)
        /// to explain the cause of the bad weather.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Barcelona
        ///
        /// </remarks>
        /// <param name="city"></param>
        /// <returns>Returns a model that includes all of this information</returns>
        /// <response code="200">Returns the results</response>
        /// <response code="400">If the requests is invalid</response>
        [HttpGet("{city}", Name = "GetCityWeather")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(string city)
        {
            // TODO: The candidate must implement here
            // - Get the weather from IWeatherRepository
            // - Returns good weather or bad weather
            //   If is bad weather returns the reason of that, indicate the sensor(s) (rain, snow, wind, etc)
            try
            {
                string description = string.Empty;
                WeatherDetectorService weatherDetectorService = new WeatherDetectorService();
                var guid = _weatherContext.Weather.Where(x => x.CityName.ToLower() == city.ToLower()).Select(x => x.Id).ToList();
                if (guid.Count > 0)
                {
                    Weather result = weatherDetectorService.FindById(guid[0]);
                    description = result.Description.ToString().ToLower() != "clear" ? "Bad weather in " + city + " as it is " + result.Description.ToString().ToLower()
                                                                                      : "Weather is good in " + city + " as it is " + result.Description.ToString().ToLower();
                }
                return description != string.Empty ? Ok(description) : NotFound(city + " is not found");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Occured!");
            }
        }

        /// <summary>
        /// Process a new petition to get sensros from OpenWeatherMap API, detects the weather and stores in a repository
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "city": "Barcelona"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="weatherCity"></param>
        /// <returns>The status of the petition</returns>
        /// <response code="201">Returns when is created succesfully</response>
        /// <response code="400">Returns for a bad request</response>
        /// <response code="406">Returns when other internal resasons</response>
        /// <response code="409">Returns if id already exists</response>
        [HttpPost(Name = "StoreCityWeather")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<IActionResult> PostAsync([FromBody] WeatherCity weatherCity) 
        {
            // TODO: The candidate must implement here
            // - Call the OpenWeatherMap API with the city from body
            // - Call WeatherDetectorService and store the results together with the sensors 
            // - Store the data in the repository IWeatherRepository
            try
            {
                if (weatherCity.City != null)
                {
                    if(weatherCity.City == "")
                    {
                        return StatusCode(StatusCodes.Status406NotAcceptable,
                                      "City name is empty!");
                    }
                    WeatherClient weatherclient = new WeatherClient();                    
                    Domain.Weather weatherResponse = await weatherclient.GetWeatherAsync(weatherCity.City);
                    return weatherResponse.CityName == null ? StatusCode(StatusCodes.Status406NotAcceptable,
                                      $"The petition to store the weather for {weatherCity.City} has not been processed because it is not found, please name it correctly!") :
                                      StatusCode(StatusCodes.Status201Created,
                                      $"The petition to store the weather for {weatherCity.City} has been processed!");
                }
                return StatusCode(StatusCodes.Status406NotAcceptable,
                                      "Could not find the 'city' property!");
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Occured!");
            }
        }
    }
}
