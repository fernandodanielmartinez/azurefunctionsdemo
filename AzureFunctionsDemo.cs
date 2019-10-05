using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionsDemo
{
    public static class AzureFunctionsDemo
    {
        public static string isCatPlaying ( int hourOfDay ) {

            if ( hourOfDay >= 0 && hourOfDay <= 6 ) {
            
                return "I'm playing because is the best time and I'm doing lot of noise!!!";
            
            } else {
            
                return "I'm very tired and I want to sleep until the night.";
            
            }

        }
        [FunctionName("AzureFunctionsDemo")]
        public static async Task<IActionResult> Run(
//            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            string hourOfDay = req.Query["hourOfDay"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            hourOfDay = hourOfDay ?? data?.hourOfDay;

            int hourOfDayN = Int32.Parse(hourOfDay);
            
            return name != null && hourOfDay != null 
                ? (ActionResult)new OkObjectResult($"Hi I'm {name} and my servants' clock marks {hourOfDay} hour. {isCatPlaying ( hourOfDayN )}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
