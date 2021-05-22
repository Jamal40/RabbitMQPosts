using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Receive.Data
{
    class PostContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Event> Events { get; set; }
        public PostContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=.\SQLEXPRESS; initial catalog=PostDatabasse; integrated security= true");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
