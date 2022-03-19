using System.Collections.Generic;

namespace DomainModel
{
    public class Team : Entity
    {
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}