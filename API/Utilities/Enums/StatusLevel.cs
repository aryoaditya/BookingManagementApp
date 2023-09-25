using System.ComponentModel.DataAnnotations;

namespace API.Utilities.Enums
{
    // Definisi tipe data enum untuk properti StatusLevel
    public enum StatusLevel
    {
        Requested,
        Approved,
        Rejected,
        Canceled,
        Completed,
        [Display(Name = "On Going")] OnGoing
    }
}
