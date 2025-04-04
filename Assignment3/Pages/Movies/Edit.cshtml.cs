using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Assignment3.Data;
using RazorPagesMovie.Models;
using Microsoft.AspNetCore.Hosting;

namespace Assignment3.Pages.Movies
{
    public class EditModel : PageModel
    {
        private readonly Assignment3.Data.Assignment3Context _context;
        private readonly IWebHostEnvironment _environment;

        public EditModel(Assignment3.Data.Assignment3Context context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            Movie = movie;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var movieToUpdate = await _context.Movie.FindAsync(Movie.Id);

            if (movieToUpdate == null)
            {
                return NotFound();
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                if (!string.IsNullOrEmpty(movieToUpdate.ImageUrl))
                {
                    var oldFilePath = Path.Combine(_environment.WebRootPath, movieToUpdate.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                movieToUpdate.ImageUrl = "/images/" + uniqueFileName;
            }

            movieToUpdate.Title = Movie.Title;
            movieToUpdate.ReleaseDate = Movie.ReleaseDate;
            movieToUpdate.Genre = Movie.Genre;
            movieToUpdate.Price = Movie.Price;
            movieToUpdate.Rating = Movie.Rating;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
