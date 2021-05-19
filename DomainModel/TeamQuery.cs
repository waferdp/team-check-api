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
                var teamIds =  matched.Select(answer => answer.TeamId);
                matched = matched.Where(answer => answer.TeamId == this.TeamId);
            }
            return matched.ToList();
        }
    }
}