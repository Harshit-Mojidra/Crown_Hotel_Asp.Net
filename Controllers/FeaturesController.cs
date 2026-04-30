using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using hrbs_project.Models;

public class FeaturesController : Controller
{
    static List<Feature> features = new List<Feature>()
    {
        new Feature{ id=1, name="Kitchen"},
        new Feature{ id=2, name="Balcony"},
        new Feature{ id=3, name="Bedroom"}
    };

    public IActionResult Index()
    {
        return View(features);
    }

    [HttpPost]
    public IActionResult AddFeature(string name)
    {
        int newId = features.Count + 1;
        features.Add(new Feature { id = newId, name = name });

        return Json(true);
    }

    [HttpPost]
    public IActionResult DeleteFeature(int id)
    {
        var item = features.FirstOrDefault(x => x.id == id);
        if (item != null)
            features.Remove(item);

        return Json(true);
    }
}