using System;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/performance-checklist")]
    [ApiController]
    public class PerformanceChecklistController : ControllerBase
    {
        [HttpPost]
        public void NewSubmission(CheckListItem[] checklist)
        {
            foreach(var item in checklist)
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }
        }
    }
}

