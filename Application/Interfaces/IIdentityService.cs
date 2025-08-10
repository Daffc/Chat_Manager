public interface IIdentityService
{
    Guid GetCurrentUserId();
    string? GeCurrentUserEmail();
    string? GeCurrentUserNickname();
}