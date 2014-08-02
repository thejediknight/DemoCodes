using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

// DB Context Class : for performing all the CRUD operations on the Models
namespace codeFirstSample.Models
{
    public class BooksDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}