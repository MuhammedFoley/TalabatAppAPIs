using System.ComponentModel.DataAnnotations;

namespace TalabatAppAPIs.Dtos
{
    public class AdressDTO
    {
        [Required]
        public string Firstname { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]

        public string County { get; set; }
        [Required]

        public string City { get; set; }
        [Required]

        public string Street { get; set; }
    }
}
