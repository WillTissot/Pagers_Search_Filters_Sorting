using Pagers_Search_Filters.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Pagers_Search_Filters.DAL
{
    public class FightersContext :DbContext
    {
        public DbSet<Fighter> Fighters { get; set; }

    }
}