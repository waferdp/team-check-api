using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainModel
{
    public class TeamQuery
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid TeamId { get; set; }

        public IEnumerable<TeamAnswer> Match(IQueryable<TeamAnswer> answers)
        {
            //Avoid state mutation
            var matched = answers;
            if(From.HasValue)
            {
                matched = matched.Where(answer => answer.Created >= From);
            }
            if(To.HasValue)
            {
                matched = matched.Where(answer => answer.Created < To);
            }
            if(TeamId != Guid.Empty)
            {
                //Ugly hack to work around Mongo driver and Linq not working well with UUIDs
                return matched.ToList().Where(answer => answer.TeamId == TeamId);
            }
            return matched;
        }
    }
}