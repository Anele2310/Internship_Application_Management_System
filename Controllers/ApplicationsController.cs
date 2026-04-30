using Internship_Application_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class ApplicationsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ApplicationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        var applications = _context.Applications
            .Include(a => a.Internship)
            .ToList();

        return View(applications);
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Apply(int internshipId, IFormFile cvFile)
    {
        var user = await _userManager.GetUserAsync(User);

        // Prevents duplicate applications
        var exists = _context.Applications
            .Any(a => a.InternshipId == internshipId && a.UserId == user.Id);

        if (exists)
        {
            TempData["Message"] = "You already applied for this internship.";
            return RedirectToAction("Index", "Internships");
        }

        string filePath = "";

        // Save the cv
        if (cvFile != null)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(cvFile.FileName);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/cvs", fileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await cvFile.CopyToAsync(stream);
            }

            filePath = "/cvs/" + fileName;
        }

        var application = new Application
        {
            InternshipId = internshipId,
            UserId = user.Id,
            CVFilePath = filePath
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Application submitted successfully!";
        return RedirectToAction("Index", "Internships");
    }
}