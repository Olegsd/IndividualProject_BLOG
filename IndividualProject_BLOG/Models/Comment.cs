using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IndividualProject_BLOG.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public Post Post { get; set; }
    }
}