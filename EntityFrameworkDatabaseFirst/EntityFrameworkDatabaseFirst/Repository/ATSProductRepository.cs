using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkDatabaseFirst.Repository
{
    public class ATSProductRepository : IProductRepository
    {
        public StoreContext db = null;

        public ATSProductRepository()
        {
            this.db = new StoreContext();
        }

        public ATSProductRepository(StoreContext _db)
        {
            this.db = _db;
        }

        public MProduct SelectProductByID(int _id)
        {
            return db.MProducts.Find(_id);
        }

        public void UpdateProduct(MProduct _prod)
        {
            db.Entry(_prod).State = System.Data.Entity.EntityState.Modified;
        }

        public void DeleteProduct(int _id)
        {
            MProduct exist = db.MProducts.Find(_id);
            db.MProducts.Remove(exist);
        }

        public void InsertProductsToDB(MProduct _prod)
        {
            db.MProducts.Add(_prod);
        }

        public IEnumerable<MProduct> SelectAllProducts()
        {
            return db.MProducts.ToList();
        }

        public void Save_Product()
        {
            db.SaveChanges();
        }
    }
}