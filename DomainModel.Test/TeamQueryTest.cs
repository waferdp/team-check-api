using System;
using System.Linq;
using DomainModel.UtiliTest;
using Xunit;

namespace DomainModel.Test
{
    public class TeamQueryTest : IDisposable
    {
        private TeamAnswerGenerator _answerGenerator;

        public TeamQueryTest()
        {
            _answerGenerator = new TeamAnswerGenerator();
        }
        
        public void Dispose()
        {
        }

        [Fact]
        public void QueryMatch_WithOnlyMatchingTeam_MatchesAll()
        {
            var query = new TeamQuery 
            {
                TeamId = Guid.NewGuid()
            };
            var answers = _answerGenerator.GenerateRandomAnswers(teamId: query.TeamId).AsQueryable();
            
            var matching = query.Match(answers);

            Assert.Equal(answers, matching);
        }

        [Fact]
        public void QueryMatch_WithMatchingTeam_MatchesSome()
        {
            var query = new TeamQuery
            {
                TeamId = Guid.NewGuid()
            };
            var answers = _answerGenerator.GenerateRandomAnswers(20, query.TeamId);
            var moreAnswers = _answerGenerator.GenerateRandomAnswers(20);
            var allAnswers = answers.Concat(moreAnswers).AsQueryable();
            var matching = query.Match(allAnswers);

            Assert.Equal(answers, matching);
        }


    }
}