using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record OrderDetailsDTO(
        [Required] int OrderId,
        [Required] int ProductId,
        [Required] int PurchaseQuantity,
        [Required] int ClientId,
        [Required] string PhoneNumber,
        string Address,
        string City,
        string State,
        string ZipCode,
        string Country,
        string Email,
        [Required, DataType(DataType.Currency)] decimal UnitPrice,
        [Required, DataType(DataType.Currency)] decimal TotalPrice,
        [Required] DateTime OrderDate,
        string PaymentMethod,
        string PaymentStatus
        );
}
