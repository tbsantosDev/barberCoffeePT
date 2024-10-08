using Barbearia.Data;
using Barbearia.Dto.Schedule;
using Barbearia.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace Barbearia.Services.Schedule
{
    public class ScheduleService : IScheduleInterface
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ScheduleService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseModel<List<ScheduleModel>>> CreateSchedule(CreateScheduleDto createScheduleDto)
        {
            ResponseModel<List<ScheduleModel>> response = new ResponseModel<List<ScheduleModel>>();

            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    response.Message = "Usuário não encontrado.";
                    response.Status = false;
                    return response;
                }

                var userId = int.Parse(userIdClaim.Value);
                var dateTime = createScheduleDto.DateTime;

                // Verifica se o dia é um dia útil
                if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    response.Message = "Agendamentos não estão disponíveis aos domingos.";
                    response.Status = false;
                    return response;
                }

                // Verifica se o horário está disponível
                bool isAvailable = await IsSlotAvailable(dateTime, createScheduleDto.BarberId);
                if (!isAvailable)
                {
                    response.Message = "O horário selecionado não está disponível.";
                    response.Status = false;
                    return response;
                }

                var schedule = new ScheduleModel()
                {
                    DateTime = dateTime,
                    CutTheHair = false,
                    BarberId = createScheduleDto.BarberId,
                    UserId = userId,
                };
                _context.Add(schedule);
                await _context.SaveChangesAsync();

                response.Dados = await _context.Schedules.ToListAsync();
                response.Message = "Agendamento criado com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ScheduleModel>> DeleteSchedule(int id)
        {
            ResponseModel<ScheduleModel> response = new ResponseModel<ScheduleModel>();

            try
            {
                var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == id);
                if (schedule == null)
                {
                    response.Message = "Agendamento não encontrado!";
                }
                _context.Remove(schedule);
                await _context.SaveChangesAsync();

                response.Dados = schedule;
                response.Message = "Agendamento excluido com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<DateTime>>> GetAvailableSlots(DateTime date, int barberId)
        {
            ResponseModel<List<DateTime>> response = new ResponseModel<List<DateTime>>();

            try
            {
                var start = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0); // 09:00
                var end = new DateTime(date.Year, date.Month, date.Day, 20, 0, 0);   // 20:00
                var interval = TimeSpan.FromMinutes(45);

                // Obter os horários reservados para o dia e barbeiro específicos
                var reservedSchedules = await _context.Schedules
                    .Where(s => s.DateTime.Date == date.Date && s.BarberId == barberId)
                    .Select(s => s.DateTime) // Obtemos o DateTime do horário
                    .ToListAsync();

                var availableSlots = new List<DateTime>();

                // Verifica todos os horários disponíveis
                for (var time = start; time < end; time = time.Add(interval))
                {
                    // Verifica se o horário está reservado
                    if (!reservedSchedules.Any(r => r.TimeOfDay == time.TimeOfDay))
                    {
                        availableSlots.Add(time);
                    }
                }

                response.Dados = availableSlots;
                response.Message = "Horários disponíveis coletados!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }



        public async Task<ResponseModel<List<ScheduleModel>>> ListSchedules(DateTime dateIni, DateTime dateFim, int barberId)
        {
            ResponseModel<List<ScheduleModel>> response = new ResponseModel<List<ScheduleModel>>();

            try
            {
                var schedules = await _context.Schedules
                    .Where(s => s.DateTime.Date >= dateIni.Date && s.DateTime.Date <= dateFim.Date && s.BarberId == barberId)
                    .Include(s => s.Barber)
                    .OrderBy(s => s.DateTime)
                    .ToListAsync();
                if (schedules.Count <= 0)
                {
                    response.Message = "Nenhum agendamento para este colaborador";
                    return response;
                }
                response.Dados = schedules;
                response.Message = "Agendamentos coletados!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ScheduleModel>> UpdateSchedule(UpdateScheduleDto updateScheduleDto)
        {
            ResponseModel<ScheduleModel> response = new ResponseModel<ScheduleModel>();

            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    response.Message = "Usuário não encontrado.";
                    response.Status = false;
                    return response;
                }

                var userId = int.Parse(userIdClaim.Value);
                var dateTime = updateScheduleDto.DateTime;

                // Verifica se o dia é um dia útil
                if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    response.Message = "Agendamentos não estão disponíveis aos domingos.";
                    response.Status = false;
                    return response;
                }

                // Verifica se o horário está disponível
                bool isAvailable = await IsSlotAvailable(dateTime, updateScheduleDto.BarberId);
                if (!isAvailable)
                {
                    response.Message = "O horário selecionado não está disponível.";
                    response.Status = false;
                    return response;
                }

                var schedule = new ScheduleModel()
                {
                    Id = updateScheduleDto.Id,
                    DateTime = dateTime,
                    CutTheHair = false,
                    BarberId = updateScheduleDto.BarberId,
                    UserId = userId,
                };
                _context.Update(schedule);
                await _context.SaveChangesAsync();

                response.Dados = schedule;
                response.Message = "Agendamento alterado com sucesso!";
                return response;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<bool> IsSlotAvailable(DateTime dateTime, int barberId)
        {
            // Verifica se o dia é um dia útil (segunda a sábado)
            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                return false; // Se for domingo, retorna false
            }

            var reservedSlot = await _context.Schedules
                .AnyAsync(s => s.DateTime == dateTime && s.BarberId == barberId);

            return !reservedSlot;
        }
    }
}
