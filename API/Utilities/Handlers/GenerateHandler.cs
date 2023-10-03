using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Utilities.Handlers
{
    public class GenerateHandler
    {
        public static string GenerateNIK(string? lastNik)
        {
            if (string.IsNullOrEmpty(lastNik))
            {
                return "111111"; // Jika tidak ada data, return 111111.
            }
            else
            {
                // Jika ada data, tambahkan 1 ke NIK terakhir.
                int lastNikInt = Convert.ToInt32(lastNik);
                lastNikInt++; // Tambah 1
                return lastNikInt.ToString().PadLeft(6, '0');
            }
        }
    }
}
