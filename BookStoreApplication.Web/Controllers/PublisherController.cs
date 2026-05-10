using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Services;
using Microsoft.AspNetCore.Mvc;
using BookStoreApplication.Web.Wrapper;

namespace BookStoreApplication.Web.Controllers;
[ApiController]
[Route("publishers")]
public class PublishersController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    public PublishersController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PublisherResponseDto>>> Create(PublisherCreateDto dto)
    {
        var created = await _publisherService.CreateAsync(dto);
        var response = ApiResponse<PublisherResponseDto>.Success(StatusCodes.Status201Created, "Publisher created successfully.", created);
        return CreatedAtAction(nameof(GetAll), new { id = created.PublisherId }, response);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PublisherResponseDto>>>> GetAll()
    {
        var publishers = await _publisherService.GetAllAsync();
        return Ok(ApiResponse<IReadOnlyList<PublisherResponseDto>>.Success(StatusCodes.Status200OK, "Publishers fetched successfully.", publishers));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<PublisherDetailsDto>>> GetDetailsById(int id)
    {
        var publisher = await _publisherService.GetDetailsByIdAsync(id);
        return Ok(ApiResponse<PublisherDetailsDto>.Success(StatusCodes.Status200OK, "Publisher details fetched successfully.", publisher));
    }

    [HttpGet("by-name/{name}")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PublisherDetailsDto>>>> GetByName(string name)
    {
        var publishers = await _publisherService.GetByNameAsync(name);
        return Ok(ApiResponse<IReadOnlyList<PublisherDetailsDto>>.Success(StatusCodes.Status200OK, "Publisher details fetched successfully.", publishers));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<PublisherResponseDto>>> Update(int id, PublisherUpdateDto dto)
    {
        var updated = await _publisherService.UpdateAsync(id, dto);
        return Ok(ApiResponse<PublisherResponseDto>.Success(StatusCodes.Status200OK, "Publisher updated successfully.", updated));
    }

    
}
