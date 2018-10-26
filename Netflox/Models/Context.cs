
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Netflox.Models
{
    public class Context : DbContext

    {

        

        public Context(DbContextOptions<Context> options) : base (options) {}
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
  //      public DbSet<MovieActor> MovieActors { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActor>()
                .HasKey(bc => new { bc.MovieId, bc.ActorId });

            modelBuilder.Entity<MovieActor>()
                .HasOne(bc => bc.Movie)
                .WithMany(b => b.ActorsLink)
                .HasForeignKey(bc => bc.MovieId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(bc => bc.Actor)
                .WithMany(c => c.MoviesLink)
                .HasForeignKey(bc => bc.ActorId);


            modelBuilder.Entity<Movie>().Property(x => x.MovieId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Actor>().Property(x => x.ActorId).ValueGeneratedOnAdd();
            modelBuilder.Entity<MovieActor>().HasKey(t => new { t.MovieId, t.ActorId });


        }
    }
}
