using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Freelance;

namespace Freelance.Data
{
    public class FreelanceContext : DbContext
    {
        public FreelanceContext (DbContextOptions<FreelanceContext> options)
            : base(options)
        {
        }

        public DbSet<Freelance.Contact> Contact { get; set; } = default!;
    }
}
