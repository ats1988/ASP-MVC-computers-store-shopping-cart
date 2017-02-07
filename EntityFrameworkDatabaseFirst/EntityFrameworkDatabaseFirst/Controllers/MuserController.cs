using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace EntityFrameworkDatabaseFirst.Controllers
{
    public class MuserController : Controller
    {
        StoreContext db = new StoreContext();
        myHwin hash = new myHwin();

        public static byte[] GetBytes(string value)
        {
            return value.Split('-').Select(s => byte.Parse(s, System.Globalization.NumberStyles.HexNumber)).ToArray();
        }

        public void RenderEveryWhere()
        {
            List<MProduct> GetProducts = (List<MProduct>)db.MProducts.ToList();
            ViewData["Products_to"] = GetProducts;
        }

        public ActionResult Index()
        {
            var myList = db.MUsers.ToList();
            var RemoveUser = myList.First(i=>i.FirstName == "admin");
            myList.Remove(RemoveUser);
            return View(myList);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MUser emp = db.MUsers.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        [HttpGet]
        public ActionResult Create()
        {
            RenderEveryWhere();
            return View();
        }
        [HttpPost]
        public ActionResult Create(MUser muse)
        {
            if (ModelState.IsValid)
            {
                if (muse.Condition_a_Value == true)
                {
                    var salt = hash.GenerateNewSalt();
                    var hwin1 = hash.ComputeHWIN_SHA256(new UTF8Encoding(true).GetBytes(muse.Password), salt);
                    muse.Password = Convert.ToBase64String(hwin1).ToString();
                }
                db.MUsers.Add(muse);
                db.SaveChanges();
                if ((string)Session["UserName"] == "admin")
                {
                    return RedirectToAction("Index");
                }
                else {
                    return RedirectToAction("Login", "Home", new { area = "" });
                }
            }
            RenderEveryWhere();
            return RedirectToAction("Login", "Home", new { area = "" });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MUser emp = db.MUsers.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MUserID,FirstName,LastName,MiddleName,Password,Salary,Email")] MUser emp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emp);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MUser emp = db.MUsers.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MUser Emp = db.MUsers.Find(id);
            db.MUsers.Remove(Emp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Categories()
        {
            return View(db.Categories.ToList());
        }

        public Category find(int Cid)
        {
            return db.Categories.Find(Cid);
        }

        public ActionResult UsersGrid()
        {
            return View();
        }

        public async Task<ActionResult> IndexAsync()
        {
            DbHelper helper = new DbHelper();
            List<MUser> data = await helper.GetUserDataAsync();
            return View(data);
        }



    }
}