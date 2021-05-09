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

        public TeamAssessmentTest()
        {
            _randomizer = new Random();
        }

        public void Dispose()
        {
        }

        private IEnumerable<TeamCheckAnswer> GenerateRandomAnswers(int amount)
        {
            var answers = new List<TeamCheckAnswer>();
            for(var i = 0; i < amount; i++)
            {
                answers.Add(GenerateAnswer(_randomizer.Next(25, 100)));
            }
            return answers;
        }

        private IEnumerable<TeamCheckAnswer> GenerateAnswersOneLowAnswer(int amount)
        {
            var answers = new List<TeamCheckAnswer>();
            for(var i = 0; i < amount; i++)
            {
                var answer = GenerateAnswer(_randomizer.Next(72, 96), 24);
                answer.Items.Add(GenerateCheckItem(1, 24));
                answers.Add(answer);
            }
            return answers; 
        }

        private IEnumerable<TeamCheckAnswer> GenerateAnswersOneHighAnswer(int amount)
        {
            var answers = new List<TeamCheckAnswer>();
            for(var i = 0; i < amount; i++)
            {
                var answer = GenerateAnswer(_randomizer.Next(24, 48), 24);
                answer.Items.Add(GenerateCheckItem(4, 24));
                answers.Add(answer);
            }
            return answers; 
        }

        private TeamCheckAnswer GenerateAnswer(int score, int questions = QUESTIONS)
        {
            var average = score / questions;
            var rest = score - (average * questions);

            var answer = new TeamCheckAnswer
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
            };

            for(var i = 0; i < questions; i++)
            {
                var itemScore = average;
                if(rest > 0)
                {
                    if(rest > 0 && itemScore < 4)
                    {
                        itemScore++;
                        rest--;
                    }
                }
                answer.Items.Add(GenerateCheckItem(itemScore, i));
            }
            return answer;
        }

        private TeamCheckItem GenerateCheckItem(int score, int index)
        {
            return new TeamCheckItem
            {
                Index = index,
                Key = $"Random question {index+1}",
                Value = score
            };
        }

        [Fact]
        public void CreateTeamAssessment_WithRandomAnswers_CalculatesAverage()
        {
            var teamSize = _randomizer.Next(5,8);
            var answers = GenerateRandomAnswers(teamSize);

            var assessment = new TeamAssessment(answers);

            Assert.True(assessment.Average > 0.0);
        }

        [Fact]
        public void CreateTeamAssessment_WithRandomAnswers_CalculatesStandardDeviation()
        {
            var teamSize = _randomizer.Next(5,8);
            var answers = GenerateRandomAnswers(teamSize);

            var assessment = new TeamAssessment(answers);

            Assert.True(assessment.StandardDeviation > 0.0);
        }

        [Fact]
        public void CreateTeamAssessment_WithOneLowAnswer_FindsSingleLowAnswer()
        {
            var teamSize = _randomizer.Next(5,8);
            var answers = GenerateAnswersOneLowAnswer(teamSize);

            var assessment = new TeamAssessment(answers);

            Assert.Single(assessment.Low);
        }

        [Fact]
        public void CreateTeamAssessment_WithOneHighwAnswer_FindsSingleHighAnswer()
        {
            var teamSize = _randomizer.Next(5,8);
            var answers = GenerateAnswersOneHighAnswer(teamSize);

            var assessment = new TeamAssessment(answers);

            Assert.Single(assessment.High);
        }
    }
}
