using System.Collections.Generic;
using Newtonsoft.Json;

namespace DomainModel
{
    public class Team : Entity
    {
        public string Name { get; set; }
        [JsonIgnore]
        public bool IsDeleted { get; set;}
        public ICollection<Member> Members { get; set; } = new List<Member>();
    }
}