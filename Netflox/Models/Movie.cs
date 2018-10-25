using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Netflox.Models
{
    public class Movie
    {
        //       private Movie movie;

        //public Movie(Movie movie)
        //{
        //    this.movie = movie;
        //}
        public Movie()
        {
            this.ActorsLink = new HashSet<MovieActor>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int MovieId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Director { get; set; }
        [NotMapped]
        [Display(Name ="Poster")]
        public IFormFile Pic { get; set; }
        public byte [] Pc { get; set; }
        [StringLength(200)]
        [Display(Name="File name")]
        public string PicName { get; set; }
        [NotMapped]
        public IFormFile Video { get; set; }
        public byte[] Vd { get; set; }
        [StringLength(200)]
        public string VideoName { get; set; }

        //      public virtual ICollection<Actor> Actors { get; set; } 

        public ICollection<MovieActor> ActorsLink { get; } = new List<MovieActor>();
    }

 
}
