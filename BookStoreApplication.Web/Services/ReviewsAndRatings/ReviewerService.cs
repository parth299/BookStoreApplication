using BookStoreApplication.Web.DTOs.ReviewsAndRatings;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Repositories.ReviewAndRatings;
using FluentValidation;

namespace BookStoreApplication.Web.Services.ReviewsAndRatings
{
    public class ReviewerService : IReviewerService
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IValidator<CreateReviewerRequestDto> _createReviewerValidator;

        public ReviewerService(IReviewerRepository reviewerRepository, IValidator<CreateReviewerRequestDto> createReviewerValidator)
        {
            _reviewerRepository = reviewerRepository;
            _createReviewerValidator = createReviewerValidator;
        }

        public async Task<ReviewerDto> CreateReviewerAsync(CreateReviewerRequestDto request)
        {
            var validationResult = await _createReviewerValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage).ToArray());

                throw new RequestValidationException(errors);
            }

            return await _reviewerRepository.CreateReviewerAsync(request);
        }

        public async Task<List<ReviewerDto>> GetAllReviewersAsync()
        {
            var reviewers = await _reviewerRepository.GetAllReviewersAsync();

            if (reviewers.Count == 0)
            {
                throw new NotFoundException("No reviewers found.");
            }

            return reviewers;
        }
    }
}
