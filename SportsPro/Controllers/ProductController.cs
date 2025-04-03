using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.DataLayer;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private IRepository<Product> products { get; set; }

        public ProductController(IRepository<Product> rep)
        {
            products = rep;
        }

        [Route("[controller]s")]
        [HttpGet]
        public ViewResult List()
        {
            var queryOptions = new QueryOptions<Product>();
            queryOptions.OrderBy = p => p.ReleaseDate;

            var products = this.products.List(queryOptions);
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
                products.Insert(product);
                products.Save();
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
            var product = products.Get(id);

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
                products.Update(product);
                products.Save();
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
            var product = products.Get(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(Product product)
        {
            product = products.Get(product.ProductID);

            if (product == null)
            {
                return NotFound();
            }

            products.Delete(product);
            products.Save();
            TempData["message"] = $"{product.Name} was deleted.";
            return RedirectToAction("List");
        }
    }
}
