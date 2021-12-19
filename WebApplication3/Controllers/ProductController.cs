using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class ProductController : Controller
    {

        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet("/api/users/products/{id}")]
        public IActionResult ShowProducts(int id)
        {
            var session = HttpContext.Session.GetInt32("CurrentRole");
            if (session != null)
            {

                if ((Role)session == Role.ADMIN)
                {
                    return View("~/Views/Product/ShowProductsForAdmin.cshtml", new Product().GetAllProducts(id));
                }

                else if ((Role)session == Role.USER)
                {
                    return View("~/Views/Product/ShowProducts.cshtml", new Product().GetAllProducts(id));
                }
            }

            else
            {
                return RedirectToAction("Login", "Login");
            }

            return View("~/Views/Product/ShowProducts.cshtml", new Product().GetAllProducts(id));
        }

        [Authorize(Roles = "USER")]
        [HttpGet("/api/users/newProduct")]
        public IActionResult GetNewProduct()
        {

            return View("~/Views/Product/NewProduct.cshtml");
        }

        [Authorize(Roles = "USER")]
        [HttpPost("/api/users/newProduct")]
        public IActionResult NewProduct(Product pmodel)
        {
            if (pmodel.CheckCode((int)pmodel.ProductCode) == 1)
            {
                ModelState.AddModelError("ProductCode", "Product Code must be unique!");
            }
            else
            {
                int id = (int)HttpContext.Session.GetInt32("CurrentId");

                Product product = new Product();
                int result = product.SaveProduct(id, pmodel);
                if (result > 0)
                {
                    return RedirectToAction("ShowProducts", new { id = id });
                }
            }

            return View("~/Views/Product/NewProduct.cshtml");
        }




        [Authorize(Roles = "USER")]
        [HttpGet("/api/users/products/edit/{id}")]
        public IActionResult EditProductById(int id)
        {

            return View("~/Views/Product/EditProduct.cshtml", new Product().getProductById(id));


        }
        [Authorize(Roles = "USER")]
        [HttpPost("/api/users/products/edit/{id}")]
        public IActionResult EditProduct(int id, Product pmodel)
        {


            int Userid = (int)HttpContext.Session.GetInt32("CurrentId");

            Product product = new Product();

            product.EditProduct(Userid, id, pmodel);

            return RedirectToAction("ShowProducts", "Product", new { id = Userid });


        }
        [Authorize(Roles = "USER")]
        [HttpGet("/api/users/products/delete/{id}")]
        public IActionResult DeleteProductById(int id)
        {
            Product product = new Product();
            Product pmodel = product.getProductById(id);
            int userid = (int)pmodel.UserId;

            product.DeleteProductById(id);

            return RedirectToAction("ShowProducts", "Product", new { id = userid });


        }


        [Authorize(Roles = "USER")]
        [HttpGet("/api/reports")]
        public IActionResult Reports()
        {
    
            return View();

        }


        [Authorize(Roles = "USER")]
        [HttpPost("/api/reports")]
        public IActionResult PostReports(Product product)
        {

             if(product.ProductType == null && product.ProductName==null && product.ProductPrice==null && product.ProductQuantity==null)
            {
                ViewBag.Error = "No products found.";
                return View("~/Views/Product/Reports.cshtml");
            }

           
                List<Product> list = new Product().GenerateReport(product);

                return View("~/Views/Product/GeneratedReport.cshtml", list);


        }
    }
}
