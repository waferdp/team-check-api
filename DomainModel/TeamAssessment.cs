using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;

public class TeamAssessment
{
    public double Average {get; private set;}
    public double StandardDeviation {get; private set;}
    public Dictionary<string, int> TeamDistribution { get; private set; }
    public Dictionary<string, int> Low { get; private set; } 
    public Dictionary<string, int> High { get; private set; }

    public TeamAssessment(IEnumerable<TeamCheckAnswer> answers)
    {
        TeamDistribution = answers.ToDictionary(answer => answer.Id.ToString(), elementSelector: (answer => CalculateSum(answer)));

        Average = TeamDistribution.Values.Average();
        StandardDeviation = CalculateStandardDeviation(TeamDistribution.Values);
        High = FindHighResults(TeamDistribution);
        Low = FindLowResults(TeamDistribution);
    }
    
    private static int CalculateSum(TeamCheckAnswer answer)
    {
        return answer.items.Select(item => item.Value).Sum();
    }

    private static double CalculateStandardDeviation(IEnumerable<int> results)
    {
        // Step 1: Find the mean.
        var mean = results.Average();
        
        // Step 2: For each data point, find the square of its distance to the mean.
        var distances = results.Select(result => Math.Pow(result  - mean, 2));

        // Step 3: Sum the values from Step 2.
        // Step 4: Divide by the number of data points.
        // Comment: The sum of elements divided by number of elements is the definition of the mean.
        // Step 5: Take the square root.
        var standardDeviation = Math.Sqrt(distances.Average());

        return standardDeviation;
    }

    private static Dictionary<string, int> FindHighResults(Dictionary<string, int> distribution)
    {
        var average = distribution.Values.Average();
        var standardDeviation = CalculateStandardDeviation(distribution.Values);
        var highLimit = average + standardDeviation;

        var highResults = distribution.Where(result => result.Value > average + highLimit)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        return highResults;
    }

    private static Dictionary<string, int> FindLowResults(Dictionary<string, int> distribution)
    {
        var average = distribution.Values.Average();
        var standardDeviation = CalculateStandardDeviation(distribution.Values);
        var lowLimit = average - standardDeviation;

        var lowResults = distribution.Where(result => result.Value < average - lowLimit)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        return lowResults;
    }

}