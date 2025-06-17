using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record AppUserDTO(
        int Id,
        [Required] string UserName,
        [Required, DataType(DataType.EmailAddress)] string Email,
        [Required] string Address,
        [Required] string City,
        [Required] string Province,
        [Required] string ZipCode,
        [Required] string Country,
        [Required] string PhoneNumber,
        [Required] string Password,
        [Required] string Role
        );
}
