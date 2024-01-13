using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DailyProgressReport.Models;
using DailyProgressReport.Classes;

namespace DailyProgressReport.Controllers
{
    public class AddMasterController : Controller
    {
        private readonly IConfiguration _configuration;

        public AddMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {

            try
            {
                // Fetch master types from the database using ADO.NET and stored procedure
                List<Master> masterTypes = GetMasterTypesFromDatabase();

                // Pass the master types to the view
                ViewBag.MasterTypes = masterTypes;

                return View();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string exceptionMessage = $"Exception in method '{methodName}'";

                // Log or rethrow the exception with the updated message
                var errorLogger = new CustomErrorLog(_configuration);
                errorLogger.LogError(ex, exceptionMessage);
                return Json(new { error = ex.Message });
            }
            
        }

        //[HttpPost]
        //public IActionResult AddMaster(string masterType)
        //{
        //    // Handle the selected master type (masterType) here
        //    // You can perform any additional logic or save it to the database

        //    return RedirectToAction("Index"); // Redirect to the desired action
        //}

        private List<Master> GetMasterTypesFromDatabase()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            List<Master> masterTypes = new List<Master>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dpr_SPGetMasterTypes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Master master = new Master
                            {
                                Id = (int)reader["Id"],
                                Name = reader["MasterType"].ToString(),
                                // Add other properties as needed
                            };

                            masterTypes.Add(master);
                        }
                    }
                }
            }

            return masterTypes;
        }
    }
}