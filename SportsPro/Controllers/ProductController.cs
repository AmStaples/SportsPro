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
            ViewBag.Mode = "Add";
            product.ReleaseDate = product.ReleaseDate.AddMilliseconds(-product.ReleaseDate.Millisecond);
            return View("Edit", product);
        }

        //[HttpPost]
        //public ActionResult Add(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _productRepository.Insert(product);
        //        _productRepository.Save();
        //        TempData["message"] = $"{product.Name} was added.";
        //        return RedirectToAction("List");
        //    }
        //    else
        //    {
        //        ViewBag.Mode = "Add";
        //        return View("Edit", product);
        //    }
        //}

        [HttpPost]
        public ActionResult Save(ProductEditViewModel model)
        {
           if(model.Mode == "Edit")
            { 
                if(ModelState.IsValid)
                {
                    Product product = new Product();
                    product.ProductID = model.ProductID;
                    product.ProductCode = model.ProductCode;
                    product.Name = model.Name;
                    product.YearlyPrice = model.YearlyPrice;
                    product.ReleaseDate = model.ReleaseDate;

                    _productRepository.Update(product);
                    _productRepository.Save();
                    TempData["message"] = $"{product.Name} was edited.";
                    return RedirectToAction("List");
                } 
                else
                {
                    return View("Edit", model);
                }
            } 
            else
            {
                if(ModelState.IsValid)
                {
                    Product product = new Product();
                    product.ProductCode = model.ProductCode;
                    product.Name = model.Name;
                    product.YearlyPrice = model.YearlyPrice;
                    product.ReleaseDate = model.ReleaseDate;

                    _productRepository.Insert(product);
                    _productRepository.Save();
                    TempData["message"] = $"{product.Name} was added.";
                    return RedirectToAction("List");
                }
                else
                {
                    return View("Edit", model);
                }
            }
        }
        //[HttpPost]
        //public ActionResult Edit(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _productRepository.Update(product);
        //        _productRepository.Save();
        //        TempData["message"] = $"{product.Name} was edited.";
        //        return RedirectToAction("List");
        //    }
        //    else
        //    {
        //        ViewBag.Mode = "Edit";
        //        return View(product);
        //    }
        //}

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = _productRepository.Get(id);
            Product viewModel = new Product();
            
            
            if (product == null)
            {
                return NotFound();
            }
            viewModel.ProductID = product.ProductID;
            viewModel.ProductCode = product.ProductCode;
            viewModel.Name = product.Name;
            viewModel.YearlyPrice = product.YearlyPrice;
            viewModel.ReleaseDate = product.ReleaseDate;
            ViewBag.Mode = "Edit";
            return View(viewModel);
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
