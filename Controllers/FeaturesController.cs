using Microsoft.AspNetCore.Mvc;
using hrbs_project.Models;
using System.Linq;

public class FeatureController : Controller
{
    private readonly ApplicationDbContext _context;

    public FeatureController(ApplicationDbContext context)
    {
        _context = context;
    }

    // LIST PAGE
    public IActionResult Index()
    {
        var data = _context.Features.ToList();
        return View(data);
    }

    // ADD FEATURE (AJAX)
    [HttpPost]
    public IActionResult AddFeature(Feature feature)
    {
        if (feature.name != null)
        {
            _context.Features.Add(feature);
            _context.SaveChanges();
            return Json(new { success = true });
        }

        return Json(new { success = false });
    }

    // DELETE FEATURE
    [HttpPost]
    public IActionResult DeleteFeature(int id)
    {
        var data = _context.Features.Find(id);

        if (data != null)
        {
            _context.Features.Remove(data);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}