using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class OrderController : Controller
    {

        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet("/api/users/orders/{id}")]
        public IActionResult ShowOrders(int id)
        {
            var session = HttpContext.Session.GetInt32("CurrentRole");
            if (session != null)
            {

                if ((Role)session == Role.ADMIN)
                {
                    return View("~/Views/Order/ShowOrdersForAdmin.cshtml", new Order().GetAllOrders(id));
                }

                else if ((Role)session == Role.USER)
                {
                    return View("~/Views/Order/ShowOrders.cshtml", new Order().GetAllOrders(id));
                }
            }

            else
            {
                return RedirectToAction("Login", "Login");
            }

            return View("~/Views/Order/ShowOrders.cshtml", new Order().GetAllOrders(id));
        }


        [Authorize(Roles = "USER")]
        [HttpGet("/api/users/newOrder")]
        public IActionResult GetNewOrder()
        {
            var id = (int)HttpContext.Session.GetInt32("CurrentId");
            List<Product> products = new Product().GetAllProducts(id);
            ViewModel view = new ViewModel();
            view.Products = products;
            return View("~/Views/Order/NewOrder.cshtml",view);
        }

        //[Authorize(Roles = "USER")]
        //[HttpPost("/api/users/orders/newOrder")]
        //public IActionResult NewOrder(Order ordermodel)
        //{
            
        //        int id = (int)HttpContext.Session.GetInt32("CurrentId");

        //        Order order = new Order();
        //        ordermodel.OrderNumber = (int)new Order().Get8Digits();


        //        int result = order.SaveOrder(id, ordermodel);
        //        if (result > 0)
        //        {
        //            return RedirectToAction("ShowOrders", new { id = id });
        //        }
            

        //    return View("~/Views/Order/ShowOrders.cshtml");
        //}


        [Authorize(Roles = "USER")]
        [HttpGet("/api/users/orders/newOrder")]
        public IActionResult ConfirmOrder()
        {
            

            return View();
        }


        [Authorize(Roles = "USER")]
        [HttpPost("/api/users/orders/newOrder")]
        public IActionResult NewOrder(ViewModel view)
        {

            int id = (int)HttpContext.Session.GetInt32("CurrentId");
            Order order = view.Orders;
            List<Product> products = view.Products;
            //Order order = new Order();
            order.OrderNumber = (int)new Order().Get8Digits();


            int result = order.SaveOrder(id, order);
            if (result > 0)
            {
                return RedirectToAction("ShowOrders", new { id = id });
            }


            return View("~/Views/Order/ShowOrders.cshtml");
        }



    }
}
