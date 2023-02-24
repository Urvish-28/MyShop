using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class BasketController : Controller
    {
        IBasketService basketService;
        IOrderService orderService;

        public BasketController(IBasketService BasketService , IOrderService OrderService)
        {
            this.basketService = BasketService;
            this.orderService = OrderService;
        }

        // GET: Basket
        public ActionResult Index()
        {
            var model = basketService.GetBasketItem(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string Id)
        {
            basketService.AddToBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            basketService.RemoveFromBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);

            return PartialView(basketSummary);
        }

        public ActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(Order order)
        {
            var basketitems = basketService.GetBasketItem(this.HttpContext);
            order.OrderStatus = "Order Ceated";

            //process payment

            order.OrderStatus = "payment processed";
            orderService.CreateOrder(order, basketitems);
            basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou", new { OrderId = order.Id });
        }

        public ActionResult ThankYou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }
    }
}