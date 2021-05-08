using System;
using System.Collections.Generic;
using Xunit;
using DomainModel;

namespace DomainModel.Test
{
    public class TeamAssessmentTest : IDisposable
    {
        private const int QUESTIONS = 25;
        private Random _randomizer;
        private IEnumerable<TeamCheckAnswer> _answers;

        public TeamAssessmentTest()
        {
            _randomizer = new Random();
            _answers = GenerateAnswers(_randomizer.Next(5,8));
        }

        public void Dispose()
        {
        }

        private IEnumerable<TeamCheckAnswer> GenerateAnswers(int amount)
        {
            var randomizer = new Random();
            var answers = new List<TeamCheckAnswer>();
            for(var i = 0; i < amount; i++)
            {
                answers.Add(GenerateAnswer(_randomizer.Next(25, 100)));
            }
            return answers;
        }

        private TeamCheckAnswer GenerateAnswer(int score)
        {
            var average = score / QUESTIONS;
            var rest = score - (average * QUESTIONS);

            var answer = new TeamCheckAnswer
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
            };

            for(var i = 0; i < QUESTIONS; i++)
            {
                var item = new TeamCheckItem
                {
                    Index = i,
                    Key = $"Random question {i+1}",
                    Value = average
                };
                if(rest > 0) 
                {
                    item.Value = average + rest;
                    rest = 0;
                }
                answer.items.Add(item);
            }
            
            return answer;
        }

        [Fact]
        public void CalculatesAverage()
        {
            var assessment = new TeamAssessment(_answers);
            Assert.True(assessment.Average > 0.0);
        }

        [Fact]
        public void CalculatesStandardDeviation()
        {
            var assessment = new TeamAssessment(_answers);
            Assert.True(assessment.StandardDeviation > 0.0);
        }
    }
}
