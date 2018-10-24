using Netflox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Netflox.ViewModels
{
    public class ViewActor: BaseModelo
    {
        public IEnumerable<Actor> Actors { get; set; }
    }
}
