using SalesManagement.Domain.Models;
using SalesManagement.Domain.Models.User;

namespace SalesManagement.Repo.Interface
{
    public interface IAuthRepo
    {
        Task<NotifyModel> GetValidateLoginAsync(LoginModel model);
    }
}