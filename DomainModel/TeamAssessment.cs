using System.Collections.Generic;
using DomainModel;

public class TeamAssessment
{
    public double Average {get; set;}
    
    public Dictionary<string, int> TeamDistribution { get; set; }
    public IEnumerable<string> Lowest { get; set; } 
    public IEnumerable<string> Highest { get; set; }
    public IEnumerable<string> Scattered { get; set; }
}