using Barbearia.Dto.Barber;
using Barbearia.Models;

namespace Barbearia.Services.Barber
{
    public interface IBarberInterface
    {
        Task<ResponseModel<List<BarberModel>>> ListBarbers();
        Task<ResponseModel<BarberModel>> FindBarberById(int id);
        Task<ResponseModel<List<BarberModel>>> CreateBarber(CreateBarberDto createBarberDto);
        Task<ResponseModel<List<BarberModel>>> UpdateBarber(UpdateBarberDto updateBarberDto);
        Task<ResponseModel<List<BarberModel>>> DeleteBarber(int id);
    }
}
