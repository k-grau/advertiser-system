using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Subscribers.Models;
using Subscribers.DAL;
using Microsoft.Extensions.Configuration;


namespace Subscribers.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class SubscriberController : ControllerBase
    {

        private string error = "";
        private string error2 = "";
        private string error3 = "";
        private DataHandler dh;

        public SubscriberController(IConfiguration configuration) {
            dh = new DataHandler (configuration);
        }


        [HttpGet("{subscriberNo:int}")]
        public async Task<IActionResult> GetSubscriber(int subscriberNo) 
        {
            
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            SubscriberDetails sd = new SubscriberDetails();
            
            var task = Task.Run(() => {sd = dh.GetSubscriber(subscriberNo, out error);});
            await Task.WhenAll(task);


            if(sd == null) {
                return StatusCode(500);
            }                       
            return Ok(sd);
        }

        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetSubscribers(int subscriberNo) 
        {
            
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            List<SubscriberDetails> sdl = new List<SubscriberDetails>();
            
            var task = Task.Run(() => {sdl = dh.GetSubscribers(out error);});
            await Task.WhenAll(task);


            if(sdl == null) {
                return StatusCode(500);
            }                       
            return Ok(sdl);
        }


        [Produces("application/json")]
        [HttpPut("{subscriberNo:int}")]
        public async Task<IActionResult> UpdateSubscriber(int subscriberNo, 
        [FromBody]SubscriberDetails sd, [FromQuery]bool update = false) 
        {
     
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
          
            if(update == false) {
                return NotFound();
            }

            int i = 0;

            
            var task = Task.Run(() => {i = dh.UpdateSubscriber(sd, subscriberNo, out error);});
            await Task.WhenAll(task);


            if(i == 0) 
            {
                return StatusCode(500);
            }
            return StatusCode(200, sd);
        }

    }
}