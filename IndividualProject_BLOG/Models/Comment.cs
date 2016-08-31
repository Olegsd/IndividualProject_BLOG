using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndividualProject_BLOG.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Date = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Author_Id { get; set; }

        [ForeignKey("Author_Id")]
        public ApplicationUser Author { get; set; }

        public Post Posts { get; set; }
    }
}