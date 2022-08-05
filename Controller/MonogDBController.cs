using Microsoft.AspNetCore.Mvc;
using PublishAPI.Filters;
using PublishAPI.Models;
using PublishAPI.Repository;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PublishAPI.Repository.Interfaces;
using StackExchange.Profiling;
using Serilog;
using Newtonsoft.Json;

namespace PublishAPI.Controller
{

        [OpenApiTag("MongoDB", Description = "Get Voterid")]
        [Route("api/mongodb")]
        [ApiController]
        public class MongoDBController : ControllerBase
        {
            public readonly IDataRepository _dataRepository;
        //public readonly object ModelState;
       

        public MongoDBController(IDataRepository dataRepository)
            {
                _dataRepository = dataRepository;
            }
            [Consumes("application/json")]
            [Produces("application/json")]
            [HttpPost("getVoterid")]
            [ValidateModel]

            public ActionResult Result([FromBody] VoterIDRequest voteridRequest)
            {
                return _dataRepository.GetVoterid(voteridRequest);
            }


        #region IsAlive
        [ApiVersion("1.0")]
        [HttpGet("health", Name = "IsAlive")]



        public async Task<IActionResult> IsAliveAsync()
        {
            var health = await _dataRepository.IsAliveAsync();
            using (MiniProfiler.Current.Step(Constants.Health))
            {
                Log.Information(Constants.Health);
                return Content(JsonConvert.SerializeObject(health));
            }
        }

        #endregion
    }

}