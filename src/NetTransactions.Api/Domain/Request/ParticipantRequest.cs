namespace NetTransactions.Api.Domain.Request;

public class ParticipantRequest
{
    public string Name { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
