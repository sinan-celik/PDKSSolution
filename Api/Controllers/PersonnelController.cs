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
    public class PersonnelController : ControllerBase
    {


        private readonly IConfiguration _configuration;
        private string conStr;
        public PersonnelController(IConfiguration configuration)
        {
            _configuration = configuration;
            conStr = _configuration.GetConnectionString("PDKSDB");
        }


        [HttpGet]
        [Route("GetPersonnel")]
        public async Task<IActionResult> GetPersonnel()
        {
            dynamic pers;
            using (var conn = new SqlConnection(conStr))
            {
                await conn.OpenAsync();
                pers = await conn.QueryAsync<dynamic>("exec sp_GetPersonnel",
                    commandType: System.Data.CommandType.Text);
            }
            return Ok(pers);
        }

        [HttpPost()]
        [Route("AddPersonnel")]
        public async Task<IActionResult> AddPersonnel([FromBody] Personnel model)
        {
            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();
                var sqlStatement = @"
                    INSERT INTO Personnel 
                    (DepartmentId, Name, Surname, DateOfStart)
                    VALUES (@DepartmentId, @Name, @Surname, @DateOfStart)";
                await connection.ExecuteAsync(sqlStatement, model);
            }
            return Ok();
        }

        
        [HttpPut]
        [Route("UpdatePersonnel")]
        public async Task<IActionResult> UpdatePersonnel([FromBody] Personnel model)
        {
            if (model.Id == 0)
            {
                return BadRequest();
            }

            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();
                var sqlStatement = @"
                    UPDATE Personnel 
                    SET  DepartmentId = @DepartmentId,
                        Name = @Name,
                        Surname = @Surname,
                        DateOfStart = @DateOfStart
                    WHERE Id = @Id";
                await connection.ExecuteAsync(sqlStatement, model);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        [Route("DeletePersonnel/{id}")]
        public async Task<IActionResult> DeletePersonnel(int id)
        {

            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();
                var sqlStatement = "DELETE Personnel WHERE Id = @Id";
                await connection.ExecuteAsync(sqlStatement, new { Id = id });
            }
            return Ok();
        }
    }
}
