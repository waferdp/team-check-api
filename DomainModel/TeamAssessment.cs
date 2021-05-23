using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainModel
{
    public class TeamAssessment
    {
        public double Average {get; private set;}
        public string Stage
        {
            get { return Maturity.AtStage(Average); } 
        }
        public double StandardDeviation {get; private set;}
        public Dictionary<string, int> TeamDistribution { get; private set; }
        public ICollection<TeamAssessmentItem> Low { get; private set; }  
        public ICollection<TeamAssessmentItem> High { get; private set; }

        public TeamAssessment(IEnumerable<TeamAnswer> answers)
        {
            if(!answers.Any()) {
                throw new ArgumentException("No answers found");
            }

            TeamDistribution = answers.ToDictionary(answer => answer.Id.ToString(), elementSelector: (answer => CalculateSum(answer)));
            Average = TeamDistribution.Values.Average();
            StandardDeviation = CalculateStandardDeviation(TeamDistribution.Values);
            High = FindHighItems(answers);
            Low = FindLowItems(answers);
        }

        private void CreateEmptyAssessment()
        {
            Average = 50.0;
            StandardDeviation = 0.0;
            TeamDistribution = new Dictionary<string, int>();
            Low = new List<TeamAssessmentItem>();
            High = new List<TeamAssessmentItem>();
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

        private static ICollection<TeamAssessmentItem> FindLowItems(IEnumerable<TeamAnswer> answers)
        {
            return FindOutliers(answers, -1);
        }

        private static ICollection<TeamAssessmentItem> FindHighItems(IEnumerable<TeamAnswer> answers)
        {
            return FindOutliers(answers, 1);
        }

        private static ICollection<TeamAssessmentItem> FindOutliers(IEnumerable<TeamAnswer> answers, int comparison)
        {
            var allAnswers = answers.SelectMany(answer => answer.Items.Select(item => item.Value));
            var averageScore = allAnswers.Average();
            var standardDeviation = CalculateStandardDeviation(allAnswers);

            var questions = answers.Select(answer => answer.Items.Count()).FirstOrDefault();
            var foundItems = new List<TeamAssessmentItem>();
            
            var limit = comparison < 0 ? averageScore - standardDeviation : averageScore + standardDeviation;

            for(var i = 0; i < questions; i++)
            {
                var replies = answers.SelectMany(answer => answer.Items.Where(item => item.Index == i));
                var itemAverage = replies.Average(reply => reply.Value);
                
                if(itemAverage.CompareTo(limit) == comparison)
                {
                    var outlier = new TeamAssessmentItem
                    {
                        Key = replies.First().Key,
                        Value = itemAverage
                    };
                    foundItems.Add(outlier);
                }
            }
            return foundItems;
        }
    }    
}
