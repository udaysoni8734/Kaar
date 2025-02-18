using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Api.Models.Requests;
using Api.Models.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Kaar.Domain.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockTrackingController : ControllerBase
    {
        private readonly IStockPriceService _stockPriceService;
        private readonly IUserPreferenceService _userPreferenceService;
        private readonly IMapper _mapper;

        public StockTrackingController(
            IStockPriceService stockPriceService,
            IUserPreferenceService userPreferenceService,
            IMapper mapper)
        {
            _stockPriceService = stockPriceService;
            _userPreferenceService = userPreferenceService;
            _mapper = mapper;
        }

        [HttpGet("prices")]
        [ProducesResponseType(typeof(IEnumerable<StockPriceResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StockPriceResponse>>> GetStockPrices(
            [FromQuery] int limit = 100)
        {
            var prices = await _stockPriceService.GetLatestPricesAsync(limit);
            return Ok(_mapper.Map<IEnumerable<StockPriceResponse>>(prices));
        }

        [HttpGet("prices/{symbol}")]
        [ProducesResponseType(typeof(StockPriceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StockPriceResponse>> GetStockPrice(string symbol)
        {
            var price = await _stockPriceService.GetLatestPriceAsync(symbol);
            if (price == null)
                return NotFound($"No price found for symbol {symbol}");

            return Ok(_mapper.Map<StockPriceResponse>(price));
        }

        [HttpPost("prices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateStockPrice(
            [FromBody] StockPriceUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _stockPriceService.UpdatePriceAsync(_mapper.Map<StockPriceUpdateDto>(request));
            return Ok();
        }

        [HttpGet("preferences/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<UserPreferenceResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserPreferenceResponse>>> GetUserPreferences(
            int userId)
        {
            var preferences = await _userPreferenceService.GetUserPreferencesAsync(userId);
            return Ok(_mapper.Map<IEnumerable<UserPreferenceResponse>>(preferences));
        }

        [HttpGet("preferences/detail/{id}")]
        [ProducesResponseType(typeof(UserPreferenceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserPreferenceResponse>> GetUserPreference(int id)
        {
            var preference = await _userPreferenceService.GetPreferenceByIdAsync(id);
            if (preference == null)
                return NotFound($"No preference found with ID {id}");

            return Ok(_mapper.Map<UserPreferenceResponse>(preference));
        }

        [HttpPost("preferences")]
        [ProducesResponseType(typeof(UserPreferenceResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserPreferenceResponse>> CreateUserPreference(
            [FromBody] CreateUserPreferenceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var preference = await _userPreferenceService.CreatePreferenceAsync(
                _mapper.Map<CreateUserPreferenceDto>(request));

            return CreatedAtAction(
                nameof(GetUserPreference),
                new { id = preference.Id },
                _mapper.Map<UserPreferenceResponse>(preference));
        }

        [HttpPut("preferences/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateUserPreference(
            int id,
            [FromBody] UpdateUserPreferenceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _userPreferenceService.UpdatePreferenceAsync(id, 
                    _mapper.Map<UpdateUserPreferenceDto>(request));
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("preferences/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUserPreference(int id)
        {
            try
            {
                await _userPreferenceService.DeletePreferenceAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

// Custom Exception for Not Found scenarios
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}