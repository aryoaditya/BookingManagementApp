﻿namespace API.DTOs.Educations
{
    public class GeneralEducationDto
    {
        public string Major { get; set; }
        public string Degree { get; set; }
        public float Gpa { get; set; }
        public Guid UniversityGuid { get; set; }
    }
}
