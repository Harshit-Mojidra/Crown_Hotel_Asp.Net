using hrbs_project.Models;
using Microsoft.AspNetCore.Mvc;

public class FacilitiesController : Controller
{
    private readonly ApplicationDbContext db;

    public FacilitiesController(ApplicationDbContext context)
    {
        db = context;
    }

    public IActionResult Index()
    {
        return View(db.Features.ToList());
    }

    [HttpPost]
    public IActionResult Add(string Name, string Description, IFormFile IconFile)
    {
        string fileName = "";

        if (IconFile != null)
        {
            fileName = Guid.NewGuid().ToString() + Path.GetExtension(IconFile.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                IconFile.CopyTo(stream);
            }
        }

        var facility = new Facility
        {
            Name = Name,
            Description = Description,
            IconPath = "/uploads/" + fileName
        };

        db.Facilities.Add(facility);
        db.SaveChanges();

        return Ok();
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var data = db.Features.Find(id);

        if (data != null)
        {
            db.Features.Remove(data);
            db.SaveChanges();
        }

        return Ok();
    }
}