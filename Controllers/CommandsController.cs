using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MVC_REST_API.Data;
using MVC_REST_API.Dtos;
using MVC_REST_API.Models;
using System.Collections;
using System.Collections.Generic;

namespace MVC_REST_API.Controllers
{
    // /api/commands
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {

        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //private readonly MockCommanderRepo _repository = new MockCommanderRepo();
        // GET api/commands
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        // GET api/commands/{id}
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);

            if (commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }

            return NotFound();
        }

        /// <summary>
        /// Creates a CommandItem
        /// </summary>
        ///<remarks>
        /// Sample request:
        ///
        ///     POST /Command
        ///     {
        ///        "howTo": "run a react app",
        ///        "line": "npm start",
        ///        "platform": "React JS"
        ///     }
        ///
        /// </remarks>
        /// <param name="commandCreateDto"></param>
        /// <returns>A newly created CommandItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response> 
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
        }

        // PUT api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommanderUpdateDto commanderUpdateDto)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null) 
            {
                return NotFound();
            }

            // this mapping has updated the commandModelFromRepo with values coming from
            // commandUpdate. so we don't actually have to do anything other than save changes
            _mapper.Map(commanderUpdateDto, commandModelFromRepo);

            // in this case we don't have to use the following but keeping this is good pratice
            _repository.UpdateCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }

        // PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommanderUpdateDto> patchDoc) 
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommanderUpdateDto>(commandModelFromRepo);

            patchDoc.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch)) 
            {
                return ValidationProblem(ModelState);
            }


            _mapper.Map(commandToPatch, commandModelFromRepo);

            _repository.UpdateCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deletes a specific CommandItem
        /// </summary>
        /// <param name="id">primary key value of the CommandItem to be deleted</param>
        /// <returns>CommandItem deleted</returns>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="404">If the Item is null</response>
        [HttpDelete("{id}")]
        public ActionResult<CommandReadDto> DeleteCommand(int id) 
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }
            _repository.DeleteCommand(commandModelFromRepo);
            _repository.SaveChanges();

            return Ok(_mapper.Map<CommandReadDto>(commandModelFromRepo));
        }

    }
}
