using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using SportsPro.Models.DataLayer;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private IRepository<Product> _productRepository;

        public ProductController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [Route("[controller]s")]
        [HttpGet]
        public ViewResult List()
        {
            var queryOptions = new QueryOptions<Product>
            {
                OrderBy = p => p.ReleaseDate
            };

            var products = _productRepository.List(queryOptions);
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
                _productRepository.Insert(product);
                _productRepository.Save();
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
            var product = _productRepository.Get(id);
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
                _productRepository.Update(product);
                _productRepository.Save();
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
            var product = _productRepository.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(Product product)
        {
            product = _productRepository.Get(product.ProductID);
            if (product == null)
            {
                return NotFound();
            }
            _productRepository.Delete(product);
            _productRepository.Save();
            TempData["message"] = $"{product.Name} was deleted.";
            return RedirectToAction("List");
        }
    }
}
