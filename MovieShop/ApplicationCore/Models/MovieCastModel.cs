using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class MovieCastModel
    {
        public int MovieId { get; set; }
        public string Character { get; set; }
        public string PosterUrl { get; set; }

    }
}
