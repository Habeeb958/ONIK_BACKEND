using ONIK_BANK.DTO;
using static ONIK_BANK.DTO.Responses;

namespace ONIK_BANK.IService
{
    public interface IAuthService
    {
        Task<GeneralResponse> CreateAccount(RegisterDTO userDTO);
        Task<LogInResponse> LogInAccount(LogInDTO logInDTO);
    }
}
