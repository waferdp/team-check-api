using System;
using System.Collections.Generic;

namespace DomainModel
{
    public class TeamAnswer : Entity
    {
        public Guid TeamId { get; set; }

        public ICollection<TeamCheckItem> Items {get; set;} = new List<TeamCheckItem>();
    }
}