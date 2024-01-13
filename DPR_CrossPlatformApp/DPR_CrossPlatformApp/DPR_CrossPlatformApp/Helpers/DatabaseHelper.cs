using DPR_CrossPlatformApp.Models;
using System;
using System.Collections.Generic;
//using System.Data.SqlClient;

namespace DPR_CrossPlatformApp.Helpers
{


    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //public List<ProjectSiteEnggAssignment> GetAssignmentsBySiteEnggID(string siteEnggID)
        //{
        //    List<ProjectSiteEnggAssignment> assignments = new List<ProjectSiteEnggAssignment>();

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand("SELECT * FROM rockpock.dpr_tblProjectandSiteEnggAssignment WHERE SiteEnggID = @SiteEnggID", connection))
        //        {
        //            command.Parameters.AddWithValue("@SiteEnggID", siteEnggID);

        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    ProjectSiteEnggAssignment assignment = new ProjectSiteEnggAssignment
        //                    {
        //                        AssignmentID = reader.GetInt32(0),
        //                        ProjectID = reader.GetInt32(1),
        //                        SiteEnggID = reader.GetString(2),
        //                        AssignmentDate = reader.GetDateTime(3)
        //                    };

        //                    assignments.Add(assignment);
        //                }
        //            }
        //        }
        //    }

        //    return assignments;
        //}
    }
}
