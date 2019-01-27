using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MeMiXReMiX.Models;

namespace MeMiXReMiX.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Song> Songs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
