using System;
using System.Collections.Generic;

namespace JobPortal.Application.DTOs.Jobs
{
    public class FilterPreferencesDto
    {
        public SalaryRangeFilterDto SalaryRange { get; set; } = new();

        public List<string> Roles { get; set; } = new();

        public List<string> ExperienceLevels { get; set; } = new();

        public List<string> JobTypes { get; set; } = new();

        public List<string> WorkModes { get; set; } = new();

        public List<string> PopularLocations { get; set; } = new();

        public List<string> PopularSkills { get; set; } = new();
    }

    public class SalaryRangeFilterDto
    {
        public int MinLpa { get; set; } = 0;

        public int MaxLpa { get; set; } = 50;

        public string Currency { get; set; } = "INR";

        public string CurrencySymbol { get; set; } = "₹";

        public string Unit { get; set; } = "LPA";

        public string MinLabel { get; set; } = "₹0";

        public string MaxLabel { get; set; } = "₹50L+";
    }
}
