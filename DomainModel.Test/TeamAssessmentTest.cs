using System;
using System.Collections.Generic;
using Xunit;
using DomainModel;
using DomainModel.UtiliTest;
using System.Text.Json;

namespace DomainModel.Test
{
    public class TeamAssessmentTest : IDisposable
    {
        private TeamAnswerGenerator _answerGenerator;

        public TeamAssessmentTest()
        {
            _answerGenerator = new TeamAnswerGenerator();
        }

        public void Dispose()
        {
        }

        [Fact]
        public void CreateTeamAssessment_WithRandomAnswers_CalculatesAverage()
        {
            var answers = _answerGenerator.GenerateRandomAnswers();
            var assessment = new TeamAssessment(answers);
            Assert.True(assessment.Average > 0.0);
        }

        [Fact]
        public void CreateTeamAssessment_WithRandomAnswers_CalculatesStandardDeviation()
        {
            var answers = _answerGenerator.GenerateRandomAnswers();
            var assessment = new TeamAssessment(answers);
            Assert.True(assessment.StandardDeviation > 0.0);
        }

        [Fact]
        public void CreateTeamAssessment_WithOneLowAnswer_FindsSingleLowAnswer()
        {
            var answers = _answerGenerator.GenerateAnswersOneLowAnswer();
            var assessment = new TeamAssessment(answers);
            Assert.Single(assessment.Low);
        }

        [Fact]
        public void CreateTeamAssessment_WithOneHighwAnswer_FindsSingleHighAnswer()
        {
            var answers = _answerGenerator.GenerateAnswersOneHighAnswer();
            var assessment = new TeamAssessment(answers);
            Assert.Single(assessment.High);
        }

        [Fact]
        public void CreateTeamAssessment_WithNoAnswers_ThrowsException()
        {
            var answers = new List<TeamAnswer>();
            Assert.ThrowsAny<Exception>(() => new TeamAssessment(answers));
        }
    }
}
