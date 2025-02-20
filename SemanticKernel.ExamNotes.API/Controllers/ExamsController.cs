﻿using Microsoft.AspNetCore.Mvc;
using SemanticKernel.ExamNotes.Business.Services.Interfaces;
using SemanticKernel.ExamNotes.Data.Models;

namespace SemanticKernel.ExamNotes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamsController(IExamService examService)
        {
            _examService = examService;
        }

        /// <summary>
        /// Evaluate an exam
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        [HttpPost("evaluate")]
        public async Task<IActionResult> EvaluateExam([FromBody] Exam exam, CancellationToken cancelationToken)
        {
            if (exam is null || string.IsNullOrWhiteSpace(exam.Subject) || !exam.Questions.Any())
            {
                return BadRequest("the exam payload is invalid.");
            }

            var (score, feedback) = await _examService.EvaluateExamAsync(exam, cancelationToken);

            var response = new
            {
                Score = score,
                Feedback = feedback
            };
            return Ok(response);
        }
    }
}
