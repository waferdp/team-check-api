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

        public static DevelopmentStage AtStage(int score)
        {
            if(score < 25 || score > 100)
            {
                throw new Exception($"Minimum score: 25, maximum score: 100. Actual score {score}");
            }
            if(score < 70)
            {
                return DevelopmentStage.FormingStorming;
            }
            else if(score <85)
            {
                return DevelopmentStage.Norming;
            }
            else
            {
                return DevelopmentStage.Performing;
            }
        }
    }
}
