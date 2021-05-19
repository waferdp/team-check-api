using System;
using System.Collections.Generic;

namespace DomainModel.UtiliTest
{
    public class TeamAnswerGenerator
    {
        private const int QUESTIONS = 25;
        private Random _randomizer;
        private int _teamSize;

        public TeamAnswerGenerator(int? teamSize = null)
        {
            _randomizer = new Random();
            _teamSize = teamSize ?? _randomizer.Next(5, 8);
        }

        public IEnumerable<TeamAnswer> GenerateRandomAnswers(int? amount = null, Guid? teamId = null)
        {
            var answers = new List<TeamAnswer>();
            amount = amount ?? _teamSize;
            for(var i = 0; i < amount; i++)
            {
                answers.Add(GenerateAnswer(_randomizer.Next(25, 100), teamId: teamId));
            }
            return answers;
        }

        public IEnumerable<TeamAnswer> GenerateAnswersOneLowAnswer(int? amount = null)
        {
            var answers = new List<TeamAnswer>();
            amount = amount ?? _teamSize;
            for(var i = 0; i < amount; i++)
            {
                var answer = GenerateAnswer(_randomizer.Next(72, 96), 24);
                answer.Items.Add(GenerateCheckItem(1, 24));
                answers.Add(answer);
            }
            return answers; 
        }

        public IEnumerable<TeamAnswer> GenerateAnswersOneHighAnswer(int? amount = null)
        {
            var answers = new List<TeamAnswer>();
            amount = amount ?? _teamSize;
            for(var i = 0; i < amount; i++)
            {
                var answer = GenerateAnswer(_randomizer.Next(24, 48), 24);
                answer.Items.Add(GenerateCheckItem(4, 24));
                answers.Add(answer);
            }
            return answers; 
        }
        private TeamAnswer GenerateAnswer(int score, int questions = QUESTIONS, Guid? teamId = null)
        {
            var average = score / questions;
            var rest = score - (average * questions);

            var answer = new TeamAnswer
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                TeamId = teamId ?? Guid.NewGuid()
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
    }
}