namespace DailyProgressReport
{
    public class ProjectViewModel
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectStartDate { get; set; }
        public string ProjectEndDate { get; set; }

        public string ProjectRevisedDate { get; set; }

        public bool IsActive { get; set; }

    }
}
