using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using System.Linq;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private SportsProContext context { get; set; }

        public ProductController(SportsProContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public ViewResult List()
        {
            var products = context.Products.OrderBy(p => p.ReleaseDate).ToList();
            return View(products);
        }

        [HttpGet]
        public ViewResult Add()
        {
            var product = new Product();
            product.ReleaseDate = product.ReleaseDate.AddMilliseconds(-product.ReleaseDate.Millisecond);
            ViewBag.Mode = "Add";
            return View("Edit", product);
        }

        [HttpPost]
        public ActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                context.Products.Add(product);
                context.SaveChanges();
                TempData["message"] = $"{product.Name} was added.";
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Mode = "Add";
                return View("Edit", product);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Mode = "Edit";
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                context.Products.Update(product);
                context.SaveChanges();
                TempData["message"] = $"{product.Name} was edited.";
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Mode = "Edit";
                return View(product);
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(Product product)
        {
            product = context.Products.Find(product.ProductID);

            if (product == null)
            {
                return NotFound();
            }

            context.Remove(product);
            context.SaveChanges();
            TempData["message"] = $"{product.Name} was deleted.";
            return RedirectToAction("List");
        }
    }
}
