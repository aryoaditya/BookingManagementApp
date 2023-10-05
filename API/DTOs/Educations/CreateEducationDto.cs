using API.DTOs.Roles;
using API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.Educations
{
    public class CreateEducationDto : GeneralEducationDto
    {
        // Operator konversi implisit dari CreateEducationDto ke Education
        public static implicit operator Education(CreateEducationDto createEducationDto)
        {
            return new Education
            {
                Guid = createEducationDto.Guid,
                Major = createEducationDto.Major,
                Degree = createEducationDto.Degree,
                Gpa = createEducationDto.Gpa,
                UniversityGuid = createEducationDto.UniversityGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
