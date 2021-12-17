using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet("/api/users/products/{id}")]
        public IActionResult ShowProducts(int id)
        {

            return View("~/Views/Product/ShowProducts.cshtml", new Product().GetAllProducts(id));
        }

        [HttpGet("/api/newProduct")]
        public IActionResult GetNewProduct()
        {

            return View("~/Views/Product/NewProduct.cshtml");
        }


        [HttpPost("/api/newProduct")]
        public IActionResult NewProduct()
        {

            return View("~/Views/Product/NewProduct.cshtml");
        }




    }
}
