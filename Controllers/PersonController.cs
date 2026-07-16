using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pozdravlyator.Data;
using Pozdravlyator.Models;
using Pozdravlyator.Services;

namespace Pozdravlyator.Controllers
{
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IBirthdayService _birthdayService;
        private readonly IWebHostEnvironment _env;

        public PersonController(ApplicationDbContext db, IBirthdayService birthdayService, IWebHostEnvironment env)
        {
            _db = db;
            _birthdayService = birthdayService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var people = await _birthdayService.GetAllSortedAsync();
            return View(people);
        }

        public async Task<IActionResult> Details(int id)
        {
            var person = await _db.People.FindAsync(id);
            if (person == null) return NotFound();
            return View(person);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person, IFormFile? photo)
        {
            if (!ModelState.IsValid)
                return View(person);

            person.PhotoPath = await SavePhotoIfAny(photo);

            _db.People.Add(person);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var person = await _db.People.FindAsync(id);
            if (person == null) return NotFound();
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Person updated, IFormFile? photo)
        {
            if (id != updated.Id) return NotFound();

            var person = await _db.People.FindAsync(id);
            if (person == null) return NotFound();

            if (!ModelState.IsValid)
                return View(updated);

            person.FirstName = updated.FirstName;
            person.LastName = updated.LastName;
            person.BirthDate = updated.BirthDate;
            person.Note = updated.Note;

            var newPhoto = await SavePhotoIfAny(photo);
            if (newPhoto != null)
            {
                DeletePhysicalPhoto(person.PhotoPath);
                person.PhotoPath = newPhoto;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var person = await _db.People.FindAsync(id);
            if (person == null) return NotFound();
            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _db.People.FindAsync(id);
            if (person != null)
            {
                DeletePhysicalPhoto(person.PhotoPath);
                _db.People.Remove(person);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<string?> SavePhotoIfAny(IFormFile? photo)
        {
            if (photo == null || photo.Length == 0) return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(photo.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
            {
                ModelState.AddModelError("photo", "Разрешены только изображения (jpg, png, gif, webp)");
                return null;
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        private void DeletePhysicalPhoto(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;
            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }
    }
}