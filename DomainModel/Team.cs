using System.Collections.Generic;

namespace DomainModel
{
    public class Team : Entity
    {
        public string Name { get; set; }
        public ICollection<Member> Members { get; set; } = new List<Member>();
    }
}