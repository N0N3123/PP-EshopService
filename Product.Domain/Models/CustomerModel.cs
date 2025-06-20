namespace Product.Domain.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Phone { get; set; }
        public required string StreetName { get; set; }
        public required string StreetNumber { get; set; }
        public required string PostalCode { get; set; }
        public required string City { get; set; }
    }
}