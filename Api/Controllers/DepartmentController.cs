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
    public class DepartmentController : ControllerBase
    {


        private readonly IConfiguration _configuration;
        private string conStr;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
            conStr = _configuration.GetConnectionString("PDKSDB");
        }


        [HttpGet]
        [Route("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            dynamic dept;
            using (var conn = new SqlConnection(conStr))
            {
                await conn.OpenAsync();
                dept = await conn.QueryAsync<dynamic>("exec sp_GetDepartments",
                    commandType: System.Data.CommandType.Text);
            }
            return Ok(dept);
        }

        [HttpPost()]
        [Route("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody] Departments model)
        {
            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();
                var sqlStatement = @"
                    INSERT INTO Departments 
                    (DepartmentName)
                    VALUES (@DepartmentName)";
                await connection.ExecuteAsync(sqlStatement, model);
            }
            return Ok();
        }

        // PUT api/Department/id
        [HttpPut]
        [Route("UpdateDepartments")]
        public async Task<IActionResult> UpdateDepartments([FromBody] Departments model)
        {
            if (model.Id == 0)
            {
                return BadRequest();
            }

            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();
                var sqlStatement = @"
                    UPDATE Departments 
                    SET  DepartmentName = @DepartmentName
                    WHERE Id = @Id";
                await connection.ExecuteAsync(sqlStatement, model);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        [Route("DeleteDepartment/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {

            using (var connection = new SqlConnection(conStr))
            {
                await connection.OpenAsync();
                var sqlStatement = "DELETE Departments WHERE Id = @Id";
                await connection.ExecuteAsync(sqlStatement, new { Id = id });
            }
            return Ok();
        }

    }
}
