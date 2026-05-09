using BookStoreApplication.Web.DTOs.ReviewsAndRatings;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Repositories.ReviewAndRatings;
using FluentValidation;

namespace BookStoreApplication.Web.Services.ReviewsAndRatings
{
    public class BookReviewService : IBookReviewService
    {
        private readonly IBookReviewRepository _reviewRepository;
        private readonly IValidator<CreateReviewRequestDto> _createReviewValidator;

        public BookReviewService(IBookReviewRepository reviewRepository, IValidator<CreateReviewRequestDto> createReviewValidator)
        {
            _reviewRepository = reviewRepository;
            _createReviewValidator = createReviewValidator;
        }

        public async Task<CreateReviewResponseDto> CreateReviewAsync(CreateReviewRequestDto request)
        {
            var validationResult = await _createReviewValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage).ToArray());

                throw new RequestValidationException(errors);
            }

            return await _reviewRepository.CreateReviewAsync(request);
        }

        public async Task<List<BookReviewDto>> GetAllReviewsAsync(int page, int pageSize)
        {
            if (page < 1)
            {
                throw new BadRequestException("Page must be greater than 0.");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                throw new BadRequestException("PageSize must be between 1 and 100.");
            }

            var reviews = await _reviewRepository.GetAllReviewsAsync(page, pageSize);

            if (reviews.Count == 0)
            {
                throw new NotFoundException("No reviews found.");
            }

            return reviews;
        }

        public async Task<List<BookReviewDto>> GetBookReviewsByIsbnAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new BadRequestException("ISBN is required.");
            }

            var reviews = await _reviewRepository.GetBookReviewsByIsbnAsync(isbn);

            if (reviews.Count == 0)
            {
                throw new NotFoundException("No reviews found for this book.");
            }

            return reviews;
        }

        public async Task<List<BookReviewDto>> GetBookReviewsByReviewerIdAsync(int reviewerId)
        {
            if (reviewerId <= 0)
            {
                throw new BadRequestException("ReviewerId must be greater than 0.");
            }

            var reviews = await _reviewRepository.GetBookReviewsByReviewerIdAsync(reviewerId);

            if (reviews.Count == 0)
            {
                throw new NotFoundException("No reviews found for this reviewer.");
            }

            return reviews;
        }

        public async Task<BookAverageRatingDto> GetAverageRatingByIsbnAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new BadRequestException("ISBN is required.");
            }

            return await _reviewRepository.GetAverageRatingByIsbnAsync(isbn);
        }
        public async Task<BookReviewDto?> UpdateReviewAsync(string isbn, int reviewerId, UpdateReviewRequestDto request)
        {
            return await _reviewRepository.UpdateReviewAsync(isbn, reviewerId, request);
        }
    }
}
