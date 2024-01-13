using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace DPRWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

       private readonly IConfiguration _configuration; // Inject your configuration if needed

        public ProjectController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
           


        [HttpGet("GetJobs")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("dpr_GetProjects", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<Project> projects = new List<Project>();

                            while (reader.Read())
                            {
                                projects.Add(new Project
                                {
                                    ProjectID = reader.GetInt32(0),
                                    ProjectName = reader.GetString(1),
                                    ProjectShortName = reader.GetString(2),
                                });
                            }

                            return Ok(projects);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


       
    }


    public class Project
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
    }
}
