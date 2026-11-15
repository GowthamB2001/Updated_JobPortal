namespace JobPortal.Application.DTOs.JobSeekers
{
    public class CompanyExperienceDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public int Years { get; set; } = 0;
        public int Months { get; set; } = 0;
    }
}
