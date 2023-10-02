using API.Models;

namespace API.DTOs.Universities
{
    public class CreateUniversityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }

        // Operator implisit yang memungkinkan konversi dari CreateUniversityDto ke University.
        public static implicit operator University(CreateUniversityDto createUniversityDto)
        {
            // Menginisialisasi entitas University dengan data dari CreateUniversityDto.
            return new University
            {
                Code = createUniversityDto.Code,  // Menggunakan kode dari DTO.
                Name = createUniversityDto.Name,  // Menggunakan nama dari DTO.
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now 
            };
        }

    }
}
