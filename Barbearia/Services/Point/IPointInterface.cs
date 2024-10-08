using Barbearia.Models;

namespace Barbearia.Services.Point
{
    public interface IPointInterface
    {
        Task<ResponseModel<int>> AmountPoints();
        Task<ResponseModel<PointModel>> CreatePoint(int scheduleId);
        Task<ResponseModel<PointModel>> DeletePoint(int id);
    }
}
