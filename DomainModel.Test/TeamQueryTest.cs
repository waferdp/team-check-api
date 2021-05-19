using System;
using DomainModel.UtiliTest;

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
        QueryMatch_WithMatchingTeam_MatchesAnswers()
        {
            var teamId = Guid.NewGuid();
            var answers = _answerGenerator.GenerateRandomAnswers(teamId: teamId);
        }

    }
}