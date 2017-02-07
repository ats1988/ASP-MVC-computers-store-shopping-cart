using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Sql;
using EntityFrameworkDatabaseFirst.Repository;


namespace EntityFrameworkDatabaseFirst.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult index()
        //{
        //    return View();
        //}
        private IProductRepository ForHomeRepository = null;

        public HomeController()
        {
            this.ForHomeRepository = new ATSProductRepository();
        }

        public HomeController(ATSProductRepository Home_Using)
        {
            this.ForHomeRepository = Home_Using;
        }

        public void RenderEveryWhere()
        {
            List<MProduct> GetProducts = (List<MProduct>)ForHomeRepository.SelectAllProducts();
            ViewData["Products_to"] = GetProducts;
        }

        public ActionResult Login()
        {
            RenderEveryWhere();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(MUser us)
        {
            
            if (ModelState.IsValid)
            {
                using (StoreContext db = new StoreContext())
                {
                    var obj = db.MUsers.Where(a => a.FirstName.Equals(us.FirstName) && a.Password.Equals(us.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.MUserID.ToString();
                        Session["UserName"] = obj.FirstName.ToString();
                        Session["CountItems"] = 0;
                        Session["CountOrders"] = 0;
                        //Session["GetProducts"] = null;
                        if ((string)Session["UserName"] == "admin")
                        {
                            return RedirectToAction("UserDashBoard2");
                        }
                        else
                        {
                            return RedirectToAction("UserDashBoard");
                        }
                    }
                }
            }
            RenderEveryWhere();
            return View(us);
        }

        //public ActionResult OrderNow(int id)
        //{
        //    //return Content("<script>window.location = 'http://www.example.com';</script>");
        //    //if (Session["UserName"] != null)
        //    //{
        //        StoreContext context = new StoreContext();
        //    if (Session["cart"] == null)
        //    {
        //        List<Item> cart = new List<Item>();
        //        cart.Add(new Item(context.MProducts.Find(id), 1));
        //        Session["cart"] = cart;
        //    }
        //        Session["CountItems"] = (int)Session["CountItems"] + 1;
        //        return View("Cart");
        //    //return View("Cart");
        //    //}
        //    //else
        //    //{
        //    //    Content("<script>alert('Nothing Happend..');</script>");
        //    //    return View();
        //    //}
        //}

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                //TempData["data1"] = Session["UserID"];
                Session["CountOrders"] = CountById();
                RenderEveryWhere();
                return View();
            }
            else
            {
                RenderEveryWhere();
                return RedirectToAction("Login");
            }

        }

        public ActionResult UserDashBoard2()
        {
            return View();
        }

        public ActionResult Logout()
        {
            // Session["UserID"] = null;
            Session.Clear();
            return RedirectToAction("Login","Home");
        }

        //[System.Web.Mvc.HttpGet]
        //public JsonResult Implement_Orders(int order_id)
        //{
        //    IEnumerable<MOrder> ModelList = new List<MOrder>();
        //    using (StoreContext Context = new StoreContext())
        //    {
        //        var orders = Context.MOrders.Where(x=> x.OUserID == order_id).ToList();
        //        ModelList = orders.Select(x =>
        //        new MOrder()
        //        {
        //            OrderID = x.OrderID,
        //            OrderName = x.OrderName,
        //            OrderQuantity = x.OrderQuantity,
        //            OrderDate = x.OrderDate
        //        }
        //        );
        //    }
        //        return Json(ModelList, JsonRequestBehavior.AllowGet);
        //}
        //[System.Web.Mvc.HttpGet]
        public JsonResult Implement_Orders(int id)
        {
            IEnumerable<MOrder> ModelList = new List<MOrder>();
            using (StoreContext Context = new StoreContext())
            {
                var orders = Context.MOrders.Where(x=> x.OUserID == id).ToList();
                
                ModelList = orders.Select(x =>
                new MOrder()
                {
                    OrderID = x.OrderID,
                    OrderName = x.OrderName,
                    OrderQuantity = x.OrderQuantity,
                    OrderDate = x.OrderDate
                }
                );
            }
            //var data = new {re= result, re2= ModelList };
            
            return Json(ModelList, JsonRequestBehavior.AllowGet);
        }


        public int CountById()
        {
            StoreContext db = new StoreContext();
            
            var sql = "SELECT COUNT(*) FROM dbo.MOrders WHERE (OUserID = "+ Session["UserID"] + ")";
            var count = db.Database.SqlQuery<int>(sql).First();
            return count;
        }


        //public ActionResult Admin()
        //{
        //    return View();
        //}


    }
}