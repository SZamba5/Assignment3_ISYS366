using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RazorPagesMovie.Helpers
{
    public static class ImageHandler
    {
        public static async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            // Define the folder where images will be stored
            var uploadsFolder = Path.Combine("wwwroot", "images");

            // Ensure the folder exists
            Directory.CreateDirectory(uploadsFolder);

            // Generate a unique filename for the uploaded image
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Return the relative path to be stored in the database
            return $"/images/{uniqueFileName}";
        }
    }
}
