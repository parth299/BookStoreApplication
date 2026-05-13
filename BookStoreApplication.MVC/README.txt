BookStoreApplication.MVC - Reviews/Ratings/Reviewers only

This MVC project contains only your Review, Rating, and Reviewer module converted from API controllers to MVC controllers and Razor views.

Included:
- Models with navigation properties: Book, Bookreview, Reviewer
- Minimal AppDbContext for book, bookreview, reviewer tables
- DTOs for reviews and reviewers
- Repositories, services, validators, mapper
- MVC controllers: ReviewController, ReviewerController
- Razor views for list/create/edit/book reviews/reviewer reviews/average rating

Before running:
1. Update appsettings.json connection string if needed.
2. Make sure your SQL Server database has these tables: book, bookreview, reviewer.
3. Run: dotnet restore
4. Run: dotnet run

Default route opens Review/Index.
