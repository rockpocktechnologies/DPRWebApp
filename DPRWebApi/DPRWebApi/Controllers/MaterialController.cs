using DailyProgressReport.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace DPRWebApi.Controllers
{
    [ApiController]
    [Route("api/material")]
    public class MaterialController : ControllerBase
    {

        private readonly IConfiguration _configuration; // Inject your configuration if needed

        public MaterialController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult AddMaterial([FromBody] MaterialModel material)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("InsertMaterial", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Date", material.Date);
                    command.Parameters.AddWithValue("@JobCodeID", material.JobCodeID);
                    command.Parameters.AddWithValue("@BlockID", material.BlockID);
                    command.Parameters.AddWithValue("@ComponentID", material.ComponentID);
                    command.Parameters.AddWithValue("@LocationID", material.LocationID);
                    command.Parameters.AddWithValue("@VillageNameID", material.VillageNameID);
                    command.Parameters.AddWithValue("@BOQHeadID", material.BOQHeadID);
                    command.Parameters.AddWithValue("@BOQReferenceID", material.BOQReferenceID);
                    command.Parameters.AddWithValue("@ActivityID", material.ActivityID);
                    command.Parameters.AddWithValue("@DesignQuantity", material.DesignQuantity);
                    command.Parameters.AddWithValue("@TypeOfPipe", material.TypeOfPipe);
                    command.Parameters.AddWithValue("@DiaOfPipe", material.DiaOfPipe);
                    command.Parameters.AddWithValue("@UOM", material.UOM);
                    command.Parameters.AddWithValue("@TotalQuantity", material.TotalQuantity);
                    command.Parameters.AddWithValue("@DayQuantity", material.DayQuantity);
                    command.Parameters.AddWithValue("@Username", material.username);
                    command.Parameters.AddWithValue("@Token", material.token);

                    command.ExecuteNonQuery();
                }
            }

            return Ok();
        }



        [HttpGet]
        [Route("api/GetMaterials")]
        public IActionResult GetMaterials()
        {
            List<MaterialModel> materials = new List<MaterialModel>();


            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("GetMaterials", connection)) // Assuming you have a stored procedure named GetMaterials
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MaterialModel material = new MaterialModel
                                {
                                    MaterialTransactionID = Convert.ToInt32(reader["Id"])
                                    //Date = Convert.ToDateTime(reader["Date"]),
                                    //// Map other properties similarly
                                    //username = Convert.ToString(reader["Username"])
                                };

                                materials.Add(material);
                            }
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
              
            }


            return Ok(materials);
        }

        public class MaterialModel
        {
            // Primary key
            public int MaterialTransactionID { get; set; }

            public string token { get; set; }

            public string username { get; set; }

            public DateTime Date { get; set; }
            public int JobCodeID { get; set; }
            public int BlockID { get; set; }
            public int ComponentID { get; set; }
            public int LocationID { get; set; }
            public int VillageNameID { get; set; }
            public int BOQHeadID { get; set; }
            public int BOQReferenceID { get; set; }
            public int ActivityID { get; set; }

            public int DesignQuantity { get; set; }
            public string TypeOfPipe { get; set; }
            public string DiaOfPipe { get; set; }
            public string UOM { get; set; }
            public int TotalQuantity { get; set; }
            public int DayQuantity { get; set; }
        }

    }

}
