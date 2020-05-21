using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnualLeaveController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private string conStr;
        public AnnualLeaveController(IConfiguration configuration)
        {
            _configuration = configuration;
            conStr = _configuration.GetConnectionString("PDKSDB");
        }


        [HttpGet]
        [Route("GetAnnualLeave")]
        public async Task<IActionResult> GetAnnualLeave()
        {
            dynamic annualLeave;
            using (var conn = new SqlConnection(conStr))
            {
                await conn.OpenAsync();
                annualLeave = await conn.QueryAsync<dynamic>("exec sp_GetAnnualLeave",
                    commandType: System.Data.CommandType.Text);
            }
            return Ok(annualLeave);
        }

        [HttpPost()]
        [Route("AddAnnualLeave")]
        public async Task<IActionResult> AddAnnualLeave([FromBody] AnnualLeave model)
        {
            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();


                var sqlStatement1 = $"exec sp_CheckForLeave {model.PersonnelId}, '{model.LeaveStart.ToString("yyyy-MM-dd")}','{model.LeaveEnd.ToString("yyyy-MM-dd")}'";
                var result = await connection.QueryAsync<DataResponse>(sqlStatement1);

                if (result.First().Response != 0)
                {
                    return BadRequest(result.First().Message);
                }
                else
                {
                    var sqlStatement2 = @"
                    INSERT INTO AnnualLeave 
                    (PersonnelId, LeaveStart, LeaveEnd)
                    VALUES (@PersonnelId, @LeaveStart, @LeaveEnd)";
                    await connection.ExecuteAsync(sqlStatement2, model);
                    return Ok();
                }

               
            }
            
        }

        // PUT api/Department/id
        [HttpPut]
        [Route("UpdateAnnualLeave")]
        public async Task<IActionResult> UpdateAnnualLeave([FromBody] AnnualLeave model)
        {
            if (model.Id == 0)
            {
                return BadRequest();
            }

            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();
                var sqlStatement = @"
                    UPDATE AnnualLeave 
                    SET  PersonnelId = @PersonnelId,
                        LeaveStart = @LeaveStart,
                        LeaveEnd = @LeaveEnd
                    WHERE Id = @Id";
                await connection.ExecuteAsync(sqlStatement, model);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        [Route("DeleteAnnualLeave/{id}")]
        public async Task<IActionResult> DeleteAnnualLeave(int id)
        {

            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();
                var sqlStatement = "DELETE AnnualLeave WHERE Id = @Id";
                await connection.ExecuteAsync(sqlStatement, new { Id = id });
            }
            return Ok();
        }
    }
}
