namespace BookStoreApplication.MVC.Services.Author_Category
{
    /// <summary>
    /// Service for handling file uploads to local storage
    /// Saves files to wwwroot/uploads/authors/ directory
    /// </summary>
    public class FileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(IWebHostEnvironment environment, ILogger<FileUploadService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        /// <summary>
        /// Uploads an author photo to wwwroot/uploads/authors/
        /// </summary>
        /// <param name="file">The uploaded file</param>
        /// <param name="authorId">Author ID for unique filename</param>
        /// <returns>Relative path to saved file (e.g., /uploads/authors/123-abc.jpg)</returns>
        public async Task<string> UploadAuthorPhotoAsync(IFormFile file, int authorId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided");

            // Validate file type
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                throw new InvalidOperationException($"Invalid file type. Allowed: JPEG, PNG, WEBP. Got: {file.ContentType}");

            // Validate file size (max 5MB)
            const int maxSize = 5 * 1024 * 1024; // 5MB
            if (file.Length > maxSize)
                throw new InvalidOperationException($"File too large. Max size: 5MB. Got: {file.Length / 1024 / 1024}MB");

            // Ensure WebRootPath exists (create wwwroot if missing)
            var webRootPath = _environment.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
                _logger.LogWarning("WebRootPath was null, using: {Path}", webRootPath);
            }

            // Create uploads directory if it doesn't exist
            var uploadsFolder = Path.Combine(webRootPath, "uploads", "authors");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
                _logger.LogInformation("Created uploads directory: {Path}", uploadsFolder);
            }

            // Generate unique filename: {authorId}-{timestamp}{extension}
            var extension = Path.GetExtension(file.FileName).ToLower();
            var fileName = $"{authorId}-{DateTime.Now:yyyyMMddHHmmss}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path for storing in DB
            var relativePath = $"/uploads/authors/{fileName}";
            
            _logger.LogInformation(
                "Uploaded photo for author {AuthorId}: {FileName} ({Size} bytes)",
                authorId, fileName, file.Length);

            return relativePath;
        }

        /// <summary>
        /// Deletes an existing author photo
        /// </summary>
        /// <param name="relativePath">Relative path stored in DB (e.g., /uploads/authors/123-abc.jpg)</param>
        /// <returns>True if deleted successfully, false if file not found</returns>
        public bool DeleteAuthorPhoto(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return false;

            try
            {
                // Ensure WebRootPath exists
                var webRootPath = _environment.WebRootPath;
                if (string.IsNullOrEmpty(webRootPath))
                {
                    webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
                }

                var filePath = Path.Combine(webRootPath, relativePath.TrimStart('/'));
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("Deleted photo: {Path}", relativePath);
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete photo: {Path}", relativePath);
                return false;
            }
        }
    }
}
