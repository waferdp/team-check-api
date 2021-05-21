using System;

namespace DomainModel
{
    public class Maturity
    {
        public enum DevelopmentStage{
            Forming,
            Storming,
            FormingStorming,
            Norming,
            Performing
        }

        public static string AtStage(double score)
        {
            if(score < 25.0 || score > 100.0)
            {
                throw new Exception($"Minimum score: 25, maximum score: 100. Actual score {score}");
            }
            if(score < 70.0)
            {
                return DevelopmentStage.FormingStorming.ToString();
            }
            else if(score <85.0)
            {
                return DevelopmentStage.Norming.ToString();
            }
            else
            {
                return DevelopmentStage.Performing.ToString();
            }
        }
    }
}
