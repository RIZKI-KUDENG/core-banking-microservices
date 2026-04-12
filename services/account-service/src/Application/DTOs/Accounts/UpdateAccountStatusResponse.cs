using Domain.Entities;

namespace Application.DTOs.Accounts;
public class UpdateAccountStatusResponse
{
    public long Id { get; set; }
    public string AccountNumber { get; set; } = null!;
    public AccountStatus Status { get; set; }
    public DateTime? UpdatedAt { get; set; }
}