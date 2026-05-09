using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.DTOs.ReviewsAndRatings;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Mapping;
using BookStoreApplication.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApplication.Web.Repositories.ReviewAndRatings
{
    public class BookReviewRepository : IBookReviewRepository
    {
        private readonly AppDbContext _context;

        public BookReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CreateReviewResponseDto> CreateReviewAsync(CreateReviewRequestDto request)
        {
            var bookExists = await _context.Books.AnyAsync(x => x.Isbn == request.Isbn);

            if (!bookExists)
            {
                throw new NotFoundException("Book not found.");
            }

            var reviewer = await _context.Reviewers.FirstOrDefaultAsync(x => x.ReviewerId == request.ReviewerId);

            if (reviewer == null)
            {
                reviewer = new Reviewer
                {
                    ReviewerId = request.ReviewerId,
                    Name = request.Name,
                    EmployedBy = request.EmployedBy
                };

                _context.Reviewers.Add(reviewer);
                await _context.SaveChangesAsync();
            }

            var reviewExists = await _context.Bookreviews.AnyAsync(x => x.Isbn == request.Isbn && x.ReviewerId == request.ReviewerId);

            if (reviewExists)
            {
                throw new ConflictException("This reviewer has already reviewed this book.");
            }

            var review = new Bookreview
            {
                Isbn = request.Isbn,
                ReviewerId = reviewer.ReviewerId,
                Rating = request.Rating,
                Comments = request.Comments
            };

            _context.Bookreviews.Add(review);
            await _context.SaveChangesAsync();

            return ReviewAndRatingMapper.ToCreateReviewResponseDto(review, reviewer);
        }

        public async Task<List<BookReviewDto>> GetAllReviewsAsync(int page, int pageSize)
        {
            var reviews = await _context.Bookreviews
                .AsNoTracking()
                .Include(x => x.Reviewer)
                .OrderBy(x => x.Isbn)
                .ThenBy(x => x.ReviewerId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return reviews.Select(ReviewAndRatingMapper.ToBookReviewDto).ToList();
        }

        public async Task<List<BookReviewDto>> GetBookReviewsByIsbnAsync(string isbn)
        {
            var reviews = await _context.Bookreviews
                .AsNoTracking()
                .Where(x => x.Isbn == isbn)
                .Include(x => x.Reviewer)
                .ToListAsync();

            return reviews.Select(ReviewAndRatingMapper.ToBookReviewDto).ToList();
        }

        public async Task<List<BookReviewDto>> GetBookReviewsByReviewerIdAsync(int reviewerId)
        {
            var reviews = await _context.Bookreviews
                .AsNoTracking()
                .Where(x => x.ReviewerId == reviewerId)
                .Include(x => x.Reviewer)
                .ToListAsync();

            return reviews.Select(ReviewAndRatingMapper.ToBookReviewDto).ToList();
        }

        public async Task<BookAverageRatingDto> GetAverageRatingByIsbnAsync(string isbn)
        {
            var bookExists = await _context.Books.AnyAsync(x => x.Isbn == isbn);

            if (!bookExists)
            {
                throw new NotFoundException("Book not found.");
            }

            var ratings = await _context.Bookreviews
                .AsNoTracking()
                .Where(x => x.Isbn == isbn && x.Rating.HasValue)
                .Select(x => x.Rating!.Value)
                .ToListAsync();

            if (ratings.Count == 0)
            {
                throw new NotFoundException("No ratings found for this book.");
            }

            return new BookAverageRatingDto
            {
                Isbn = isbn,
                AverageRating = Math.Round(ratings.Average(), 2),
                ReviewCount = ratings.Count
            };
        }
        public async Task<BookReviewDto?> UpdateReviewAsync(string isbn, int reviewerId, UpdateReviewRequestDto request)
        {
            var review = await _context.Bookreviews
                .Include(x => x.Reviewer)
                .FirstOrDefaultAsync(x => x.Isbn == isbn && x.ReviewerId == reviewerId);

            if (review == null)
            {
                return null;
            }



            if (request.Comments != null)
            {
                review.Comments = request.Comments;
            }

            await _context.SaveChangesAsync();

            return new BookReviewDto
            {
                Isbn = review.Isbn,
                ReviewerId = review.ReviewerId,
                ReviewerName = review.Reviewer.Name,
                EmployedBy = review.Reviewer.EmployedBy,
                Rating = review.Rating,
                Comments = review.Comments
            };
        }
    }
}
