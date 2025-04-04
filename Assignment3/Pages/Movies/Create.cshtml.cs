using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Assignment3.Data;
using RazorPagesMovie.Models;
using RazorPagesMovie.Helpers;

namespace Assignment3.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly Assignment3.Data.Assignment3Context _context;

        public CreateModel(Assignment3.Data.Assignment3Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;
        public IFormFile? imageFile { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Handle image upload
            if (imageFile != null && imageFile.Length > 0)
            {
                Movie.ImageUrl = await ImageHandler.SaveImageAsync(imageFile); // Save image and get path
            }

            _context.Movie.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
