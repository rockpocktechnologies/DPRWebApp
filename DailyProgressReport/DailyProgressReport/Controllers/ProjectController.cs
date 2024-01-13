using DailyProgressReport.Classes;
using DailyProgressReport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace DailyProgressReport.Controllers
{
    public class ProjectController : Controller
    {


        private readonly IConfiguration _configuration; // Inject your configuration if needed

        public ProjectController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {

            try
            {
                // Assuming GetProjects() returns a List<ProjectViewModel>
                List<ProjectViewModel> projects = GetProjects();
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
        public ActionResult SaveProject(SaveProjectModel model)
        {
            CommonFunction cmn = new CommonFunction();

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("dpr_SPSaveProject", connection))
                {
                   command.CommandType = CommandType.StoredProcedure;

        // Add parameters
        command.Parameters.Add(new SqlParameter("@ProjectID", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    if (model.ProjectID == null)
                    {
                        command.Parameters.AddWithValue("@ProjectIDValue", DBNull.Value);

                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ProjectIDValue", model.ProjectID); // Assuming model.ProjectID is an integer

                    }
                    command.Parameters.AddWithValue("@ProjectName", model.ProjectName);
        command.Parameters.AddWithValue("@ProjectShortName", model.ProjectShortName);
        command.Parameters.AddWithValue("@ProjectCode", model.ProjectCode);
        command.Parameters.AddWithValue("@ProjectStartDate", cmn.ConvertDateFormatddmmyytommddyyDuringSave(model.ProjectStartDate));
        command.Parameters.AddWithValue("@ProjectEndDate", cmn.ConvertDateFormatddmmyytommddyyDuringSave(model.ProjectEndDate));
        command.Parameters.AddWithValue("@ProjectRevisedDate", cmn.ConvertDateFormatddmmyytommddyyDuringSave(model.ProjectRevisedDate));

       
        command.ExecuteNonQuery();

        // Retrieve the updated or newly inserted ProjectID
        int updatedProjectID = Convert.ToInt32(command.Parameters["@ProjectID"].Value);
                    model.ProjectID = updatedProjectID;

                    connection.Close();
                }
            }

            return RedirectToAction("Index");
        }

        //public IActionResult Editproject(int? editProjectId)
        //{
        //    List<ProjectViewModel> projects = GetProjects();
        //    ViewBag.EditProjectId = editProjectId; // Pass the EditProjectId to the view
        //    return View(projects);
        //}

        public IActionResult EditProject(int projectId)
        {
            try
            {
                ProjectViewModel project = GetProjectById(projectId);
                return View("Index", new List<ProjectViewModel> { project });
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
        public ActionResult DeleteProject(int projectId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("dpr_SPDeleteProject", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@ProjectID", projectId);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();
                    }
                }

                return Json(new { success = true, message = "Project deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting project", error = ex.Message });
            }
        }


        private ProjectViewModel GetProjectById(int projectId)
        {
            CommonFunction cmn = new CommonFunction();

            ProjectViewModel project = new ProjectViewModel();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM dpr_tblProjects WHERE ProjectID = @ProjectID order by ProjectID desc", connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", projectId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            project.ProjectID = Convert.ToInt32(reader["ProjectID"]);
                            project.ProjectName = reader["ProjectName"].ToString();
                            project.ProjectShortName = reader["ProjectShortName"].ToString();
                            project.ProjectCode = reader["ProjectCode"].ToString();
                            project.ProjectStartDate = cmn.ConvertDateFormatmmddyytoddmmyyDuringDisplay(reader["ProjectStartDate"].ToString());
                            project.ProjectEndDate = cmn.ConvertDateFormatmmddyytoddmmyyDuringDisplay(reader["ProjectEndDate"].ToString());
                            project.ProjectRevisedDate = cmn.ConvertDateFormatmmddyytoddmmyyDuringDisplay(reader["ProjectRevisedDate"].ToString());
                        }
                    }
                }
            }

            return project;
        }


        private List<ProjectViewModel> GetProjects()
        {
            CommonFunction cmn = new CommonFunction();
            List<ProjectViewModel> projects = new List<ProjectViewModel>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM dpr_tblProjects", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProjectViewModel project = new ProjectViewModel
                            {
                                ProjectID = Convert.ToInt32(reader["ProjectID"]),
                                ProjectName = reader["ProjectName"].ToString(),
                                ProjectShortName = reader["ProjectShortName"].ToString(),
                                ProjectCode = reader["ProjectCode"].ToString(),
                                ProjectStartDate = cmn.ConvertDateFormatmmddyytoddmmyyDuringDisplay(reader["ProjectStartDate"].ToString()),
                                ProjectEndDate = cmn.ConvertDateFormatmmddyytoddmmyyDuringDisplay(reader["ProjectEndDate"].ToString()),
                                ProjectRevisedDate = cmn.ConvertDateFormatmmddyytoddmmyyDuringDisplay(reader["ProjectRevisedDate"].ToString())
                            };

                            projects.Add(project);
                        }
                    }
                }
            }

            return projects;
        }



    }


}
