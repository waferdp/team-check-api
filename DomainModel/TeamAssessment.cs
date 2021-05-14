using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;

public class TeamAssessment
{
    public double Average {get; private set;}
    public double StandardDeviation {get; private set;}
    public Dictionary<string, int> TeamDistribution { get; private set; }
    public Dictionary<string, double> Low { get; private set; } 
    public Dictionary<string, double> High { get; private set; }

    public TeamAssessment(IEnumerable<TeamAnswer> answers)
    {
        TeamDistribution = answers.ToDictionary(answer => answer.Id.ToString(), elementSelector: (answer => CalculateSum(answer)));

        Average = TeamDistribution.Values.Average();
        StandardDeviation = CalculateStandardDeviation(TeamDistribution.Values);
        High = FindHighItems(answers);
        Low = FindLowItems(answers);
    }
    
    private static int CalculateSum(TeamAnswer answer)
    {
        return answer.Items.Select(item => item.Value).Sum();
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

    private static Dictionary<string, double> FindLowItems(IEnumerable<TeamAnswer> answers)
    {
        return FindOutliers(answers, -1);
    }

    private static Dictionary<string, double> FindHighItems(IEnumerable<TeamAnswer> answers)
    {
        return FindOutliers(answers, 1);
    }

    private static Dictionary<string, double> FindOutliers(IEnumerable<TeamAnswer> answers, int comparison)
    {
        var allAnswers = answers.SelectMany(answer => answer.Items.Select(item => item.Value));
        var averageScore = allAnswers.Average();
        var standardDeviation = CalculateStandardDeviation(allAnswers);

        var questions = answers.Select(answer => answer.Items.Count()).FirstOrDefault();
        var foundItems = new Dictionary<string, double>();
        
        var limit = comparison < 0 ? averageScore - standardDeviation : averageScore + standardDeviation;

        for(var i = 0; i < questions; i++)
        {
            var replies = answers.SelectMany(answer => answer.Items.Where(item => item.Index == i));
            var itemAverage = replies.Average(reply => reply.Value);
            
            if(itemAverage.CompareTo(limit) == comparison)
            {
                foundItems[replies.First().Key] = itemAverage;
            }
        }
        return foundItems;
    }
}