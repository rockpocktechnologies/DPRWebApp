using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DailyProgressReport.Models;
using DailyProgressReport.Classes;

namespace DailyProgressReport.Controllers
{
    public class DprComponentController : Controller
    {
        private readonly IConfiguration _configuration;

        public DprComponentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {

            try
            {
                List<DprComponent> projects = new List<DprComponent>();
                string loggedInUserId = HttpContext.Session.GetString("SLoggedInUserID");

                if (loggedInUserId != null && loggedInUserId != "")
                {
                    projects = GetComponentListFromDatabase(loggedInUserId);
                }
                return View(projects);
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

        [HttpPost]
        public IActionResult AddComponent(string componentName)
        {
            try
            {
                string loggedInUserId = HttpContext.Session.GetString("SLoggedInUserID");

                if(loggedInUserId != null && loggedInUserId != "") {

                    AddComponentToDatabase(componentName, loggedInUserId); // Replace "User123" with the actual user or logged-in user
                    return Json(new { success = true, message = "Component added successfully!" });

                }
                else
                {
                    return Json(new { success = false, message = $"Error adding component, please login and retry." });

                }
                // Handle the addition of the component to the database

                // Return success message as JSON
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

        public IActionResult ComponentList()
        {
            try
            {
                // Fetch component list from the database
                List<DprComponent> components = new List<DprComponent>();
                string loggedInUserId = HttpContext.Session.GetString("SLoggedInUserID");

                if (loggedInUserId != null && loggedInUserId != "")
                {
                    components = GetComponentListFromDatabase(loggedInUserId);
                }
                // Pass the components to the view
                ViewBag.Components = components;

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

        private void AddComponentToDatabase(string componentName, string createdBy)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dpr_sp_AddComponent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ComponentName", componentName);
                    command.Parameters.AddWithValue("@CreatedBy", createdBy);

                    command.ExecuteNonQuery();
                }
            }
        }

        private List<DprComponent> GetComponentListFromDatabase(string loggedInUserId)
        {
            CommonFunction cmn = new CommonFunction();
            List<DprComponent> components = new List<DprComponent>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dpr_sp_GetComponentList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CreatedBy", loggedInUserId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DprComponent component = new DprComponent
                            {
                                Id = (int)reader["Id"],
                                ComponentName = reader["ComponentName"].ToString(),
                                CreatedDate = cmn.ConvertDateFormatmmddyytoddmmyyDuringDisplay(reader["CreatedDate"].ToString()),
                                CreatedBy = reader["CreatedBy"].ToString(),
                                ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? reader["ModifiedBy"].ToString() : null,
                                ModifiedDate = reader["ModifiedDate"] != DBNull.Value ? cmn.ConvertDateFormatmmddyytoddmmyyDuringDisplay(reader["ModifiedDate"].ToString()) : "",
                                // Add other properties as needed
                            };

                            components.Add(component);
                        }
                    }
                }
            }

            return components;
        }

        // ...

        [HttpGet]
        public IActionResult GetComponentById(int id)
        {
            try
            {
                DprComponent component = GetComponentByIdFromDatabase(id);
                return Json(new { success = true, component });
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

        [HttpPost]
        public IActionResult UpdateComponent(int id, string componentName)
        {
            try
            {
                string loggedInUserId = HttpContext.Session.GetString("SLoggedInUserID");
                if(loggedInUserId != null && loggedInUserId != "")
                {
                    UpdateComponentInDatabase(id, componentName, loggedInUserId); // Replace "User123" with the actual user or logged-in user
                    return Json(new { success = true, message = "Component updated successfully!" });

                }
                else
                {
                    return Json(new { success = false, message = $"Error updating component, please login and try again" });

                }
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

        [HttpPost]
        public IActionResult DeleteComponent(int id)
        {
            try
            {
                DeleteComponentFromDatabase(id);
                return Json(new { success = true, message = "Component deleted successfully!" });
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

        private DprComponent GetComponentByIdFromDatabase(int id)
        {
            DprComponent component = new DprComponent();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dpr_sp_GetComponentById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            component.Id = (int)reader["Id"];
                            component.ComponentName = reader["ComponentName"].ToString();
                            // Add other properties as needed
                        }
                    }
                }
            }

            return component;
        }

        private void UpdateComponentInDatabase(int id, string componentName, string modifiedBy)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dpr_sp_UpdateComponent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ComponentName", componentName);
                    command.Parameters.AddWithValue("@ModifiedBy", modifiedBy);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteComponentFromDatabase(int id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dpr_sp_DeleteComponent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
