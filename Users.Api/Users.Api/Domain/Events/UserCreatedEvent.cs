namespace Users.Api.Domain.Events
{
    public class UserCreatedEvent
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public DateTime OccurredAt { get; set; }
    }
}
