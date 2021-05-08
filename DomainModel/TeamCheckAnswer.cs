using System.Collections.Generic;
using System;

namespace DomainModel
{
    public class TeamCheckAnswer
    {
        public Guid Id {get; set;}
        public DateTime Created { get; set;}
        public ICollection<TeamCheckItem> items {get; set;} = new List<TeamCheckItem>();
    }
}