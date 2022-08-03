using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class ReviewModel
    {
        public int userId {get; set;}
        public int movieId { get; set; }
        public string title { get; set; }
        public decimal rating { get; set; }
        public string reviewText { get; set; }
    }
}
