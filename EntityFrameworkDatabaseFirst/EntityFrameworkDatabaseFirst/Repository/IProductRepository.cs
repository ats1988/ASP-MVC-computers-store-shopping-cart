using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkDatabaseFirst.Repository
{
    interface IProductRepository
    {
        IEnumerable<MProduct> SelectAllProducts();
        MProduct SelectProductByID(int id);
        void UpdateProduct(MProduct prod);
        void DeleteProduct(int id);
        void InsertProductsToDB(MProduct prod_to_db);
        void Save_Product(); 
    }
}
