using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Netflox.Models
{
    public class Actor
    {
        //       private Actor actor;

        //public Actor(Actor actor)
        //{
        //    this.actor = actor;
        //}

        public Actor()
        {
            this.MoviesLink = new HashSet<MovieActor>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ActorId { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name ="Name")]
        public string ActorName { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Display(Name ="Birth date")]
        public DateTime Birthdate { get; set; }
        [NotMapped]
        public IFormFile Pic { get; set; }
        public byte[] Pc { get; set; }
        [Display(Name ="Picture File")]
        public string PicName { get; set; }

        //     public virtual ICollection<Movie> Movies { get; set; } 
        public ICollection<MovieActor> MoviesLink { get; } = new List<MovieActor>();
    }
}