using API.Models;

namespace API.DTOs.Universities
{
    public class UniversityDto
    {
        public Guid Guid { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        // Operator eksplisit yang mengkonversi objek University ke UniversityDto
        public static explicit operator UniversityDto(University university)
        {
            return new UniversityDto
            {
                Guid = university.Guid,   // Mengisi properti Guid dari objek University
                Code = university.Code,   // Mengisi properti Code dari objek University
                Name = university.Name    // Mengisi properti Name dari objek University
            };
        }

        // Operator implisit yang mengkonversi objek UniversityDto ke University
        public static implicit operator University(UniversityDto universityDto)
        {
            return new University
            {
                Guid = universityDto.Guid,           // Mengisi properti Guid dari objek UniversityDto
                Code = universityDto.Code,           // Mengisi properti Code dari objek UniversityDto
                Name = universityDto.Name,           // Mengisi properti Name dari objek UniversityDto
                ModifiedDate = DateTime.Now         // Mengisi properti ModifiedDate dengan waktu saat ini
            };
        }
    }
}
