using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UriShortener.Data.Model.Dto;
using UriShortener.Options;
using UriShortener.Services;

namespace UriShortener.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UriController(IUriService _uriService, IOptions<ShortUriOptions> _shUriOpts) : ControllerBase
{
  [HttpPost]
  public async Task<ActionResult<UriResponseDto>> Shorten(UriWithKeyRequestDto dto){
    var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    var result = await _uriService.GenerateUri(dto, uid);
    if (result.Status.Equals(UriServiceStatus.EmptyTarget)) return BadRequest("Target is Required");
    else if (result.Status.Equals(UriServiceStatus.InvalidTime)) return BadRequest($"Time range: 0 - {_shUriOpts.Value.MaxLifeTimeInMinutes}");
    else if (result.Status.Equals(UriServiceStatus.LongKey)) return BadRequest($"Key can't be larger than {_shUriOpts.Value.MaxKeyLength}");
    else
      return Ok(result.Data);
  }

  [HttpPost("basic")]
  [AllowAnonymous]
  public async Task<ActionResult<UriResponseDto>> BasicShorten(UriWithoutKeyRequestDto dto){
    var result = await _uriService.GenerateUri(dto);
    if (result.Equals(UriServiceStatus.EmptyTarget)) return BadRequest("Target is Required");
    return Ok(result.Data);
  }
  [HttpGet("{key}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetTarget([FromRoute] string key){
    var target = await _uriService.GetTarget(key);
    if (target is null) return BadRequest("This uri is not mapped to any target, or may be expired (NOTE: shortened URIs are case-sensitive)");
    return Redirect(target);
  }
}