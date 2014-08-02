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
    [Table("Reviews")] // Table name
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }

        [ForeignKey("Book")]
        public int BookID { get; set; }
        public string ReviewText { get; set; }

        // Since there is a one to many relationship between these models, like a Book has many Reviews 
        // So we need to take that into account for our models. 
        // This will keep track of the book this review belong too.
        public virtual Book Book { get; set; }
    }
}