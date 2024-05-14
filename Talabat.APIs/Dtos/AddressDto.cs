using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class AddressDto
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
}
