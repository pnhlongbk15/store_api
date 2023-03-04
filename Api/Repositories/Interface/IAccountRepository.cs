using Store.Models;

namespace Store.Repositories
{
    public interface IAccountRepository
    {
        public Task<dynamic> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
    }
}
