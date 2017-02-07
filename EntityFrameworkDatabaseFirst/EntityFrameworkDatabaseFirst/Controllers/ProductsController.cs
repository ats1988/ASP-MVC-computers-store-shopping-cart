using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using EntityFrameworkDatabaseFirst.Repository;

namespace EntityFrameworkDatabaseFirst.Controllers
{
    public class ProductsController : Controller
    {
        //StoreContext db = new StoreContext();
        private IProductRepository repository = null;

        public ProductsController()
        {
            this.repository = new ATSProductRepository();
        }

        public ProductsController(ATSProductRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult PrdouctsPage()
        {
            List<MProduct> model_Prod = (List<MProduct>)repository.SelectAllProducts();
            //return View(db.MProducts.ToList());
            return View(model_Prod);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MProduct prod = repository.SelectProductByID(Convert.ToInt32(id));
            if (prod == null)
            {
                return HttpNotFound();
            }
            return View(prod);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(MProduct mpd)
        {
            if (ModelState.IsValid)
            {
                repository.InsertProductsToDB(mpd);
                //db.SaveChanges();
                repository.Save_Product();
                return RedirectToAction("PrdouctsPage");
            }
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MProduct emp = repository.SelectProductByID(Convert.ToInt32(id));
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,ProdcutQuantity,ProductPrice,ProductDate,ProductIamge")] MProduct prod)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
                //db.SaveChanges();
                repository.UpdateProduct(prod);
                repository.Save_Product();
                return RedirectToAction("PrdouctsPage");
            }
            return View(prod);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MProduct prod = repository.SelectProductByID(Convert.ToInt32(id));
            if (prod == null)
            {
                return HttpNotFound();
            }
            return View(prod);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.DeleteProduct(id);
            repository.Save_Product();
            return RedirectToAction("PrdouctsPage");
        }

        public ActionResult ShowAllProducts()
        {
            List<MProduct> Prod_Page = (List<MProduct>)repository.SelectAllProducts();
            return View(Prod_Page);
        }


        //private IQueryable<MProduct> MapProducts()
        //{
        //    return from p in db.MProducts select new MProduct()
        //    {
        //        ProductID=p.ProductID,ProductName=p.ProductName,ProductPrice=p.ProductPrice
        //    } ; 
        //}

        //public IEnumerable<MProduct> GetProducts()
        //{
        //    return MapProducts().AsEnumerable();
        //}

        //public MProduct GetProduct(int id)
        //{
        //    var product = (from p in MapProducts() where p.ProductID == 1 select p).FirstOrDefault();
        //    if (product == null)
        //    {
        //        return null;
        //    }
        //    return product;
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}





    }
}