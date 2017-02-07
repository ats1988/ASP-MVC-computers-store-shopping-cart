using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityFrameworkDatabaseFirst.Models;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EntityFrameworkDatabaseFirst.Controllers
{
    public class DbHelper
    {
        StoreContext db = new StoreContext();
        public async Task<List<MUser>> GetUserDataAsync()
        {
            var query = from HR14 in db.MUsers
                        orderby HR14.FirstName ascending
                        select HR14;
            List<MUser> data = await query.ToListAsync();
            return data;
                        
        }
    }
}