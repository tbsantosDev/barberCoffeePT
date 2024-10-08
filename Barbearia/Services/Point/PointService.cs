using Barbearia.Data;
using Barbearia.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Barbearia.Services.Point
{
    public class PointService : IPointInterface
    {
        private readonly AppDbContext _context;
        public PointService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<PointModel>> CreatePoint(int scheduleId)
        {
            ResponseModel<PointModel> response = new ResponseModel<PointModel>();

            try
            {
                var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == scheduleId);

                if (schedule == null)
                {
                    response.Message = "Agendamento não encontrado.";
                    response.Status = false;
                    return response;
                }

                if (schedule.CutTheHair)
                {
                    response.Message = "O cliente já recebeu pontos por este corte.";
                    response.Status = false;
                    return response;
                }

                schedule.CutTheHair = true;
                _context.Schedules.Update(schedule);

                var points = new PointModel()
                {
                    Amount = 1,
                    DateTime = DateTime.Now,
                    UserId = schedule.UserId
                };

                await _context.Points.AddAsync(points);
                await _context.SaveChangesAsync();

                response.Dados = points;
                response.Message = "Pontos criados e agendamento atualizado!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<PointModel>> DeletePoint(int id)
        {
            ResponseModel<PointModel> response = new ResponseModel<PointModel>();

            try
            {
                var point = await _context.Points.FirstOrDefaultAsync(p => p.Id == id);
                if (point == null)
                {
                    response.Message = "Pontos não localizado";
                }
                _context.Remove(point);
                await _context.SaveChangesAsync();

                response.Dados = point;
                response.Message = "Pontos excluidos com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<int>> AmountPoints()
        {
            ResponseModel<int> response = new ResponseModel<int>();
            try
            {
                int totalPoints = await _context.Points.SumAsync(p => p.Amount);

                response.Dados = totalPoints;
                response.Message = "Pontos gerais coletados";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

    }
}
