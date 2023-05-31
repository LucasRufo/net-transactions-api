namespace NetTransactions.Api.Domain.Request;

public class UpdateParticipantRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
