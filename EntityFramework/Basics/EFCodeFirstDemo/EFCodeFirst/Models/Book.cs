using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Model Class
// To hint the database generation module about the persistence information like
// table names, key columns etc, we need to specify the attributes Table, Key, ForeignKey. 
namespace codeFirstSample.Models
{
    [Table("Books")] // Table name
    public class Book
    {
        [Key] // Primary key
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string ISBN { get; set; }

        // Since there is a one to many relationship between these models, like a Book has many Reviews 
        // So we need to take that into account for our models.
        // This is to maintain the many reviews associated with a book entity.
        public virtual ICollection<Review> Reviews { get; set; }
    }
}