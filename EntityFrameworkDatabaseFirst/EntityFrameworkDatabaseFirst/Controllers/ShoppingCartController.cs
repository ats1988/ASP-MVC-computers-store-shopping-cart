using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityFrameworkDatabaseFirst.Models;
using System.Data.Entity.Validation;

namespace EntityFrameworkDatabaseFirst.Controllers
{
    public class ShoppingCartController : Controller
    {
        StoreContext db = new StoreContext();
        //public string _start { get; set; }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cart()
        {
            
            ViewData["_start"] = "Start Your SHOPPINGS";
            return View("Cart");
        }

        private int isExisting(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            //int sess = Convert.ToInt32(this.Session["UserID"]);
            //foreach (Item item in (List<Item>)Session["cart"])
            //{
            //    item.Product.UserID = sess;
            //}
            for (int i=0; i<cart.Count;i++)
                if (cart[i].Product.ProductID == id)
                    return i;
            Session["CountItems"] = (int)Session["CountItems"];
            return -1;
        }

        public ActionResult Delete(int id)
        {
            int index = isExisting(id);
            List<Item> cart = (List<Item>)Session["cart"];
            cart.RemoveAt(index);
            Session["cart"] = cart;
            Session["CountItems"] = (int)Session["CountItems"] - 1;
            return View("Cart");
        }

        public ActionResult OrderNow(int id)
        {
            //int sess = Convert.ToInt32(this.Session["UserID"]);
            //ViewData["UserID"] = System.Web.HttpContext.Current.Session["UserID"];
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item(db.MProducts.Find(id),1));
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = isExisting(id);
                if (index == -1) { cart.Add(new Item(db.MProducts.Find(id), 1));
                    
                }
                else { cart[index].Quantity++; Session["CountItems"] = (int)Session["CountItems"] - 1; }
                //foreach (Item item in (List<Item>)Session["cart"])
                //{
                //    item.Product.UserID = Convert.ToInt32(TempData["data1"]);
                //}
                //cart.Single<Item>(x => x.Product.UserID == sess);
                //int kolombos = Convert.ToInt32(Session["UserID"]);
                //cart.Single(x=>x.Product.UserID == kolombos);
                Session["cart"] = cart;
                
            }
            Session["CountItems"] = (int)Session["CountItems"] + 1;
            return View("Cart");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create( MOrder mo)
        {
            List<Item> cart = (List<Item>)Session["cart"];

            Session["cart"] = cart;

            var cartItem = db.MOrders.SingleOrDefault(
                c => c.OProductID == mo.OProductID
                );


            if (ModelState.IsValid)
                {
                if (cartItem == null)
                {
                    for (int i = 0; i < cart.Count; i++)
                    {
                        cartItem = new MOrder
                        {
                            OProductID = mo.OProductID,
                            OrderName = cart[i].Product.ProductName,
                            OrderDate = cart[i].Product.ProductDate,
                            OrderQuantity = cart[i].Product.ProdcutQuantity,
                            OrderIamge = cart[i].Product.ProductIamge
                        };

                        db.Set<MOrder>().Add(cartItem);
                        db.SaveChanges();
                    }
                    
                 }
                else {

                }
                
            }
            //if (ModelState.IsValid)
            //{

            //        db.MOrders.Add(ord);

            //    db.SaveChanges();
            //    ModelState.Clear();
            //    //return RedirectToAction("Index");
            //}
            return View();
        }

        public ActionResult CheckOut()
        {
            if (ModelState.IsValid)
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    List<Item> cart = (List<Item>)Session["cart"];
                    //Save Order
                    MOrder order = new MOrder();

                    foreach (var item in cart)
                    {
                        order.OProductID = item.Product.ProductID;
                        order.OrderDate = item.Product.ProductDate;
                        order.OUserID = 1;//(int)Session["UserID"]; //(int)item.Product.UserID;
                        order.OrderName = item.Product.ProductName;
                        order.OrderQuantity = item.Product.ProdcutQuantity;
                        order.OrderShipment = null;
                        order.OrderIamge = item.Product.ProductIamge;
                        db.MOrders.Add(order);
                        db.SaveChanges();
                    }
                    Session.Remove("cart");
                    Session["CountItems"] = (int)Session["CountItems"] * 0;
                }
                
            }
            return View();
        }

        public ActionResult CheckOut2()
        {
            MOrder order = new MOrder();
            if (ModelState.IsValid)
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    List<Item> cart = (List<Item>)Session["cart"];
                    //Save Order

                    Session["cart"] = cart;

                    //var cartItem = cart.FindIndex(5);
                    //int _check = (int)Session["UserID"];

                    foreach (var ee in cart)
                    {
                        order.OProductID = ee.Product.ProductID;
                        order.OrderDate = DateTime.Now;
                        order.OUserID =Convert.ToInt32(Session["UserID"]); //(int)item.Product.UserID;
                        order.OrderName = ee.Product.ProductName;//(string)Session["UserName"];
                        order.OrderQuantity = ee.Product.ProdcutQuantity;
                        order.OrderShipment = null;
                        order.OrderIamge = null;
                        db.MOrders.Add(order);
                        db.SaveChanges();
                    }
                }

            }
            return RedirectToAction("CheckOut", "ShoppingCart"); ;
        }

        public JsonResult ListAll()
        {
            //
            List<Item> cart = (List<Item>)Session["cart"];

            return Json(cart.ToList(),JsonRequestBehavior.AllowGet);
        }

    }
}