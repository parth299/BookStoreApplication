using BookStoreApplication.MVC.Data;
using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;
using BookStoreApplication.MVC.Exceptions;
using BookStoreApplication.MVC.Mapping;
using BookStoreApplication.MVC.Models;
using Microsoft.EntityFrameworkCore;
using BookStoreApplication.MVC.Exceptions.RatingsAndReviewers;

namespace BookStoreApplication.MVC.Repositories.ReviewAndRatings
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly AppDbContext _context;

        public ReviewerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewerDto> CreateReviewerAsync(CreateReviewerRequestDto request)
        {
            var exists = await _context.Reviewers.AnyAsync(x => x.ReviewerId == request.ReviewerId);

            if (exists)
            {
                throw new ConflictExceptionReview("Reviewer already exists.");
            }

            var reviewer = new Reviewer
            {
                ReviewerId = request.ReviewerId,
                Name = request.Name,
                EmployedBy = request.EmployedBy
            };

            _context.Reviewers.Add(reviewer);
            await _context.SaveChangesAsync();

            return ReviewAndRatingMapper.ToReviewerDto(reviewer);
        }

        public async Task<List<ReviewerDto>> GetAllReviewersAsync()
        {
            var reviewers = await _context.Reviewers
                .AsNoTracking()
                .OrderBy(x => x.ReviewerId)
                .ToListAsync();

            return reviewers.Select(ReviewAndRatingMapper.ToReviewerDto).ToList();
        }
    }
}
