using API.Models;

namespace API.DTOs.Educations
{
    public class EducationDto : GeneralEducationDto
    {
        public Guid Guid { get; set; }

        // Operator konversi eksplisit dari Education ke EducationDto
        public static explicit operator EducationDto(Education education)
        {
            return new EducationDto
            {
                Guid = education.Guid,
                Major = education.Major,
                Degree = education.Degree,
                Gpa = education.Gpa,
                UniversityGuid = education.UniversityGuid
            };
        }

        // Operator konversi implisit dari EducationDto ke Education
        public static implicit operator Education(EducationDto educationDto)
        {
            return new Education
            {
                Guid = educationDto.Guid,
                Major = educationDto.Major,
                Degree = educationDto.Degree,
                Gpa = educationDto.Gpa,
                UniversityGuid = educationDto.UniversityGuid,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
