using BookStoreApplication.Web.DTOs.Book_Publisher;
using BookStoreApplication.Web.Services.Book_Publisher;
using BookStoreApplication.Web.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers.Books_Publisher;

[ApiController]
[Route("publishers")]
//[Authorize(Roles = "Guest,RegisteredUser,StoreOwner,Admin")]
public class PublishersController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    public PublishersController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    [Authorize(Roles = "StoreOwner,Admin")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PublisherResponseDto>>> Create(PublisherCreateDto dto)
    {
        var created = await _publisherService.CreateAsync(dto);

        var response = ApiResponse<PublisherResponseDto>.SuccessResponse(
            created,
            "Publisher created successfully.",
            StatusCodes.Status201Created);

        return CreatedAtAction(
            nameof(GetAll),
            new { id = created.PublisherId },
            response);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PublisherResponseDto>>>> GetAll()
    {
        var publishers = await _publisherService.GetAllAsync();

        return Ok(
            ApiResponse<IReadOnlyList<PublisherResponseDto>>.SuccessResponse(
                publishers,
                "Publishers fetched successfully.",
                StatusCodes.Status200OK));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<PublisherDetailsDto>>> GetDetailsById(int id)
    {
        var publisher = await _publisherService.GetDetailsByIdAsync(id);

        return Ok(
            ApiResponse<PublisherDetailsDto>.SuccessResponse(
                publisher,
                "Publisher details fetched successfully.",
                StatusCodes.Status200OK));
    }

    [HttpGet("by-name/{name}")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PublisherDetailsDto>>>> GetByName(string name)
    {
        var publishers = await _publisherService.GetByNameAsync(name);

        return Ok(
            ApiResponse<IReadOnlyList<PublisherDetailsDto>>.SuccessResponse(
                publishers,
                "Publisher details fetched successfully.",
                StatusCodes.Status200OK));
    }

    [Authorize(Roles = "StoreOwner,Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<PublisherResponseDto>>> Update(int id, PublisherUpdateDto dto)
    {
        var updated = await _publisherService.UpdateAsync(id, dto);

        return Ok(
            ApiResponse<PublisherResponseDto>.SuccessResponse(
                updated,
                "Publisher updated successfully.",
                StatusCodes.Status200OK));
    }
}
