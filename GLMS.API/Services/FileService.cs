using Microsoft.AspNetCore.Http;

namespace GLMS.Services
{
    // This service handles file validation and saving files to the server
    public class FileService
    {
        private readonly IWebHostEnvironment _environment;

        // Inject the web host environment so we can access wwwroot
        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // Check if the uploaded file is a PDF
        public bool IsPdf(IFormFile file)
        {
            if (file == null || string.IsNullOrEmpty(file.FileName))
            {
                return false;
            }

            return Path.GetExtension(file.FileName).ToLower() == ".pdf";
        }

        // Save the uploaded PDF to the uploads folder and return the relative file path
        public async Task<string?> SavePdfAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            // Create a unique file name so files do not overwrite each other
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Build the full uploads folder path
            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            // Make sure the uploads folder exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Full file path on the server
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the relative path to store in the database
            return "/uploads/" + uniqueFileName;
        }
    }
}