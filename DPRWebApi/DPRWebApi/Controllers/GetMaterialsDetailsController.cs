using Microsoft.AspNetCore.Mvc;

namespace DPRWebApi.Controllers
{
    using DailyProgressReport.Classes;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class GetMaterialsDetailsController : ControllerBase
    {
        private readonly IConfiguration _configuration; // Inject your configuration if needed

        public GetMaterialsDetailsController(IConfiguration configuration)
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
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string exceptionMessage = $"Exception in method '{methodName}'";

                // Log or rethrow the exception with the updated message
                var errorLogger = new CustomErrorLog(_configuration);
                errorLogger.LogError(ex, exceptionMessage);

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



        [HttpGet("GetBlocksBasedOnProject")]
        public async Task<ActionResult<IEnumerable<Block>>> GetBlocksBasedOnProject(int projectId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("dpr_sp_GetBlocksBasedOnProject", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for ProjectId
                        command.Parameters.Add(new SqlParameter("@ProjectId", SqlDbType.Int) { Value = projectId });

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<Block> blocks = new List<Block>();

                            while (reader.Read())
                            {
                                blocks.Add(new Block
                                {
                                    Id = reader.GetInt32(0),
                                    BlockName = reader.GetString(1)

                                });
                            }

                            return Ok(blocks);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string exceptionMessage = $"Exception in method '{methodName}'";

                // Log or rethrow the exception with the updated message
                var errorLogger = new CustomErrorLog(_configuration);
                errorLogger.LogError(ex, exceptionMessage);

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("GetLocationsByBlock")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsByBlock(int blockId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("dpr_spGetLocationsByBlock", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BlockId", blockId);


                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<Location> locations = new List<Location>();

                            while (reader.Read())
                            {
                                locations.Add(new Location
                                {
                                    Id = reader.GetInt32(0),
                                    LocationName = reader.GetString(1),
                                    // Add more properties as needed
                                });
                            }

                            return Ok(locations);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string exceptionMessage = $"Exception in method '{methodName}'";

                // Log or rethrow the exception with the updated message
                var errorLogger = new CustomErrorLog(_configuration);
                errorLogger.LogError(ex, exceptionMessage);

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet("GetVillagesByLocation")]
        public async Task<ActionResult<IEnumerable<Village>>> GetVillagesByLocation(int locationId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("dpr_spGetVillagesByLocation", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@LocationId", locationId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<Village> villages = new List<Village>();

                            while (reader.Read())
                            {
                                villages.Add(new Village
                                {
                                    Id = reader.GetInt32(0),
                                    VillageName = reader.GetString(1),
                                    // Add more properties as needed
                                });
                            }

                            return Ok(villages);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string exceptionMessage = $"Exception in method '{methodName}'";

                // Log or rethrow the exception with the updated message
                var errorLogger = new CustomErrorLog(_configuration);
                errorLogger.LogError(ex, exceptionMessage);

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet("GetBoQHeadsByProject")]
        public async Task<ActionResult<IEnumerable<BoQHead>>> GetBoQHeadsByProject(int projectId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("dpr_spGetBoQHeadsByProject", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProjectId", projectId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<BoQHead> boqHeads = new List<BoQHead>();

                            while (reader.Read())
                            {
                                boqHeads.Add(new BoQHead
                                {
                                    Id = reader.GetInt32(0),
                                    BOQHeadName = reader.GetString(1),
                                    // Add more properties as needed
                                });
                            }

                            return Ok(boqHeads);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string exceptionMessage = $"Exception in method '{methodName}'";

                // Log or rethrow the exception with the updated message
                var errorLogger = new CustomErrorLog(_configuration);
                errorLogger.LogError(ex, exceptionMessage);

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("GetBoQReferencesByBoQHead")]
        public async Task<ActionResult<IEnumerable<BoQReference>>> GetBoQReferencesByBoQHead(int boQHeadId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("dpr_sp_GetBoQReferencesBasedOnBoQHeadId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BoQHeadId", boQHeadId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<BoQReference> boqReferences = new List<BoQReference>();

                            while (reader.Read())
                            {
                                boqReferences.Add(new BoQReference
                                {
                                    Id = reader.GetInt32(0),
                                    BOQNo = reader.GetString(1),
                                    // Add more properties as needed
                                });
                            }

                            return Ok(boqReferences);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string exceptionMessage = $"Exception in method '{methodName}'";

                // Log or rethrow the exception with the updated message
                var errorLogger = new CustomErrorLog(_configuration);
                errorLogger.LogError(ex, exceptionMessage);

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet("GetBoQDetailsByReference")]
        public async Task<ActionResult<BoQReferenceDetails>> GetBoQDetailsByReference(int boQReferenceId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("dpr_sp_GetBoQDetailsByReference", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BoQReferenceId", boQReferenceId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                BoQReferenceDetails boqReferenceDetails = new BoQReferenceDetails
                                {
                                    BOQHead = reader.IsDBNull(0) ? null : reader.GetString(0),
                                    BOQNo = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    WBSNumber = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    BOQDescription = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    UOM = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Length = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    TypeOfPipeClass = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    Diameter = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    BlockQuantity = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                                    CreatedDate = reader.GetDateTime(9),
                                    CreatedBy = reader.IsDBNull(10) ? null : reader.GetString(10),
                                    UpdatedDate = reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11),
                                    UpdatedBy = reader.IsDBNull(12) ? null : reader.GetString(12),
                                    // Add more properties as needed
                                };

                                return Ok(boqReferenceDetails);
                            }

                            return NotFound(); // or appropriate status code
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpPost("SaveMaterialTransaction")]
        public IActionResult SaveMaterialTransaction([FromBody] MaterialTransactionDto model)
        {
            // Token validation logic goes here

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                  
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("dpr_SaveMaterialTransactions", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command
                        command.Parameters.AddWithValue("@Id", model.Id);
                        command.Parameters.AddWithValue("@Date", model.Date);
                        command.Parameters.AddWithValue("@JobCodeID", model.JobCodeID);
                        command.Parameters.AddWithValue("@BlockID", model.BlockID);
                        command.Parameters.AddWithValue("@ComponentID", model.ComponentID);
                        command.Parameters.AddWithValue("@LocationID", model.LocationID);
                        command.Parameters.AddWithValue("@VillageNameID", model.VillageNameID);
                        command.Parameters.AddWithValue("@BOQHeadID", model.BOQHeadID);
                        command.Parameters.AddWithValue("@BOQReferenceID", model.BOQReferenceID);
                        command.Parameters.AddWithValue("@ActivityID", model.ActivityID);
                        command.Parameters.AddWithValue("@BlockQuantity", model.BlockQuantity);
                        command.Parameters.AddWithValue("@TypeOfPipe", model.TypeOfPipe);
                        command.Parameters.AddWithValue("@DiaOfPipe", model.DiaOfPipe);
                        command.Parameters.AddWithValue("@UOM", model.UOM);
                        command.Parameters.AddWithValue("@JTDQuantity", model.JTDQuantity);
                        command.Parameters.AddWithValue("@DayQuantity", model.DayQuantity);
                        command.Parameters.AddWithValue("@username", model.Username);
                        command.Parameters.AddWithValue("@IsSubmitted", model.IsSubmitted);
                        command.Parameters.AddWithValue("@WBSNumber", model.WBSNumber);

                        command.ExecuteNonQuery();
                    }
                }

                return Ok(new { Message = "Record saved successfully" });
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string exceptionMessage = $"Exception in method '{methodName}'";

                // Log or rethrow the exception with the updated message
                var errorLogger = new CustomErrorLog(_configuration);
                errorLogger.LogError(ex, exceptionMessage);


                return StatusCode(500, new { Message = "Internal server error" });
            }

        }

        // Assuming you are using ASP.NET Web API
        [HttpGet("GetMaterialTransactionsSummaryByDateAndUsername")]
        public async Task<ActionResult<IEnumerable<MaterialTransactionSummary>>> GetMaterialTransactionsSummaryByDateAndUsername(DateTime date, string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("dpr_spGetMaterialTransactionsSummaryByDateAndUsername", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<MaterialTransactionSummary> transactionsSummary = new List<MaterialTransactionSummary>();

                            while (reader.Read())
                            {
                                MaterialTransactionSummary transaction = new MaterialTransactionSummary
                                {
                                    Block = reader.IsDBNull(0) ? null : reader.GetString(0),
                                    Village = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    BoQReference = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    DayQuantity = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                };

                                transactionsSummary.Add(transaction);
                            }

                            return Ok(transactionsSummary);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string exceptionMessage = $"Exception in method '{methodName}'";

                // Log or rethrow the exception with the updated message
                var errorLogger = new CustomErrorLog(_configuration);
                errorLogger.LogError(ex, exceptionMessage);

                return StatusCode(500, new { Message = "Internal server error" });
            }
        }


        [HttpGet("GetComponentsByProjectId")]
        public async Task<ActionResult<IEnumerable<Component>>> GetComponentsByProjectId(int projectId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("dpr_SPGetComponentsByProjectId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProjectId", projectId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<Component> components = new List<Component>();

                            while (reader.Read())
                            {
                                Component component = new Component
                                {
                                    Id = reader.GetInt32(0),
                                    ComponentName = reader.GetString(1),
                                    // Add more properties if needed
                                };

                                components.Add(component);
                            }

                            return Ok(components);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string exceptionMessage = $"Exception in method '{methodName}'";

                // Log or rethrow the exception with the updated message
                var errorLogger = new CustomErrorLog(_configuration);
                errorLogger.LogError(ex, exceptionMessage);

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("CheckUnsubmittedData")]
        public async Task<string> CheckUnsubmittedData(string username, DateTime date)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("dpr_SPCheckUnsubmittedData", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Date", date.Date);

                        var result = await command.ExecuteScalarAsync();

                        if (result != null)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            // Handle the case when the result is null
                            return "An unexpected error occurred.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string exceptionMessage = $"Exception in method '{methodName}'";

                // Log or rethrow the exception with the updated message
                var errorLogger = new CustomErrorLog(_configuration);
                errorLogger.LogError(ex, exceptionMessage);

                return "An unexpected error occurred.";
            }
        }


        public class MaterialTransactionSummary
        {
            public string Block { get; set; }
            public string Village { get; set; }
            public string BoQReference { get; set; }
            public int DayQuantity { get; set; }
        }

        public class MaterialTransactionDto
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public int JobCodeID { get; set; }
            public int BlockID { get; set; }
            public int ComponentID { get; set; }
            public int LocationID { get; set; }
            public int VillageNameID { get; set; }
            public int BOQHeadID { get; set; }
            public int BOQReferenceID { get; set; }
            public int ActivityID { get; set; }
            public int BlockQuantity { get; set; }
            public string TypeOfPipe { get; set; }
            public string DiaOfPipe { get; set; }
            public string UOM { get; set; }
            public int JTDQuantity { get; set; }
            public int DayQuantity { get; set; }
            public string Username { get; set; }
            public bool IsSubmitted { get; set; }
            public string token { get; set; }
            public string WBSNumber { get; set; }
        }

        public class BoQReferenceDetails
        {
            public string BOQHead { get; set; }
            public string BOQNo { get; set; }
            public string WBSNumber { get; set; }
            public string BOQDescription { get; set; }
            public string UOM { get; set; }
            public string Length { get; set; }
            public string TypeOfPipeClass { get; set; }
            public string Diameter { get; set; }
            public int BlockQuantity { get; set; }
            public DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public string UpdatedBy { get; set; }

            // Add more properties as needed
        }

        public class BoQReference
        {
            public int Id { get; set; }
            public string BOQNo { get; set; }

            // Add other properties as needed

            // You may also add navigation properties if needed
            // public int BoQHeadId { get; set; }
            // public BoQHead BoQHead { get; set; }
        }


        public class BoQHead
        {
            public int Id { get; set; }
            public string BOQHeadName { get; set; }
            // Add more properties as needed

            // If you're using C# 9 or later, you can use record type for brevity:
            // public record BoQHead(int Id, string BOQHeadName);
        }

        public class Component
        {
            public int Id { get; set; }
            public string ComponentName { get; set; }
            // Add more properties as needed
        }



        public class Village
        {
            public int Id { get; set; }
            public string VillageName { get; set; }
            // Add more properties as needed
        }

        public class Block
        {
            public int Id { get; set; }
            public string BlockName { get; set; }
            public int ProjectId { get; set; }
            public DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public string UpdatedBy { get; set; }
        }

        public class Location
        {
            public int Id { get; set; }
            public string LocationName { get; set; }
            // Add more properties as needed
        }


    }
}
