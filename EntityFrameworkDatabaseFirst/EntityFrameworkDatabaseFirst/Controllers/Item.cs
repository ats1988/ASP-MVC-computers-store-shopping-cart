using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityFrameworkDatabaseFirst.Models;

namespace EntityFrameworkDatabaseFirst.Controllers
{
    public class Item
    {
        private MProduct product = new MProduct();

        public MProduct Product
        {
            get { return product; }
            set { product = value; }
        }
        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public Item()
        {   }

        public Item(MProduct product, int quantity)
        {
            this.product = product;
            this.quantity = quantity;
        }
    }
}