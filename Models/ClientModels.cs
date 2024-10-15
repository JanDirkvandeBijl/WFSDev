namespace WFSDev.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ConnectionString>? ConnectionStrings { get; set; }
    }

    public class ConnectionString
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ServerName { get; set; }
        public string? DatabaseName { get; set; }
        public string? DatabaseUserId { get; set; }
        public string? DatabasePassword { get; set; }
        public string? AdditionalParameters { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign key to link to Client
        public int ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
