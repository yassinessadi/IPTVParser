using IPTV_Parser.Models;
using IPTV_Parser.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IPTV_Parser.Controllers.v1
{
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class HomeController : ControllerBase
    {
        protected ApiResponse _response;
        public IParserService _Service;
        public HomeController(IParserService Service)
        {
            _Service = Service;
            this._response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status411LengthRequired)]
        public async Task<ActionResult<ApiResponse>> Index(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                _response.StatusCode = HttpStatusCode.LengthRequired;
                _response.ErrorMessages.Add("Please Insert your m3u stream");
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            try
            {
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                var result = await _Service.GetChannelsAsync(url);
                if (result == null || result.Count() == 0)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _response.Result = result;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }

        //[HttpGet("")]
        //public async Task<ActionResult<ApiResponse>> get(string url)
        //{

        //}
    }
}
