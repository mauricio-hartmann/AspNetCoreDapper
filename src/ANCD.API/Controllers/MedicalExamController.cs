﻿using ANCD.Application.Commands;
using ANCD.Application.DTOs;
using ANCD.Application.Mediator;
using ANCD.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace ANCD.API.Controllers
{
    [Route("api/v{version:apiVersion}/medical-exam")]
    [ApiVersion("1.0")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class MedicalExamController : BaseController
    {
        public MedicalExamController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Schedule a medical exam.
        /// </summary>
        /// <param name="command">Medical exam data</param>
        /// <returns>Scheduled medical exam data</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /medical-exam
        ///     {
        ///         "doctorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "patientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "date": "2022-04-10T09:00:00.000Z"
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Returns the newly scheduled medical exam data</response>
        /// <response code="400">Returns a list of erros</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ScheduleMedicalExam([FromBody] ScheduleMedicalExamCommand command)
        {
            var result = await _mediator.SendCommand(command);

            return DefaultResponse(result, HttpStatusCode.Created);
        }

        /// <summary>
        /// Set a medical exam as accomplished.
        /// </summary>
        /// <param name="id">Medical exam Id</param>
        /// <returns>Ok if medical exam was set as accomplished</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     POST /7447b0b9-bc85-4ecf-b1f3-c53a7a4f201f/accomplish
        /// </remarks>
        /// <response code="200">Returns if medical exam was set as accomplished</response>
        /// <response code="400">Returns a list of erros</response>
        [HttpPost("{id:guid}/accomplish")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AccomplishMedicalExam([FromRoute] Guid id)
        {
            var command = new AccomplishMedicalExamCommand(id);
            var result = await _mediator.SendCommand(command);

            return DefaultResponse(result, HttpStatusCode.OK);
        }

        /// <summary>
        /// Search by a medical exam with the specified Id.
        /// </summary>
        /// <param name="id">Medical exam Id</param>
        /// <returns>Medical exam data</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /medical-exam/6522d02c-c12e-413e-ae3f-30dd549aacdd
        ///     
        /// </remarks>
        /// <response code="200">Returns the medical exam data</response>
        /// <response code="204">Data not found</response>
        /// <response code="400">Returns a list of erros</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(MedicalExamDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetMedicalExamByIdQuery(id);
            var result = await _mediator.SendQuery<GetMedicalExamByIdQuery, MedicalExamDTO>(query);
            var statusCode = result.Data is null ? HttpStatusCode.NoContent : HttpStatusCode.OK;

            return DefaultResponse(result, statusCode);
        }
    }
}
