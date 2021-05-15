using System.Collections.Generic;

namespace DomainModel
{
    public class TeamAnswer : Entity
    {
        public ICollection<TeamCheckItem> Items {get; set;} = new List<TeamCheckItem>();
    }
}