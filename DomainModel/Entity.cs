using System;

namespace DomainModel
{
    public class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Created { get; set; } = DateTime.Now;
    }
}