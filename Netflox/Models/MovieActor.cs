using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Netflox.Models
{
    public class MovieActor
    {

        public MovieActor(Movie movie)
        {
            this.Movie = movie;
        }
        public MovieActor(Actor actor)
        {
            this.Actor = actor;
        }
        public MovieActor() { }

        [Key]
        [Column(Order = 0)]
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Actor")]
        public int ActorId { get; set; }
        public virtual Actor Actor { get; set; }
    }
}
