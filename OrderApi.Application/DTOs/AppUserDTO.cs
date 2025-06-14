using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record AppUserDTO(
        int Id,
        [Required] string Name,
        [Required, DataType(DataType.EmailAddress)] string Email,
        [Required] string Address,
        [Required] string City,
        [Required] string State,
        [Required] string ZipCode,
        [Required] string Country,
        [Required] string PhoneNumber,
        [Required] string Password,
        [Required] string Role
        );
}
