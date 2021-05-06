using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;

public class TeamAssessment
{
    public double Average {get; set;}
    
    public Dictionary<string, int> TeamDistribution { get; set; }
    public IEnumerable<string> Lowest { get; set; } 
    public IEnumerable<string> Highest { get; set; }
    public IEnumerable<string> Scattered { get; set; }

    public TeamAssessment(IEnumerable<TeamCheckAnswer> answers)
    {
        TeamDistribution = answers.ToDictionary(answer => answer.Id.ToString(), elementSelector: (answer => CalculateSum(answer)));
        Average = TeamDistribution.Values.Average();
    }
    
    private int CalculateSum(TeamCheckAnswer answer)
    {
        return answer.items.Select(item => item.Value).Sum();
    }
    
}