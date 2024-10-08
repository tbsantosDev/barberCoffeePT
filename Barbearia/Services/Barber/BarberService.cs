using Barbearia.Data;
using Barbearia.Dto.Barber;
using Barbearia.Models;
using Microsoft.EntityFrameworkCore;

namespace Barbearia.Services.Barber
{
    public class BarberService : IBarberInterface
    {
        private readonly AppDbContext _context;

        public BarberService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<List<BarberModel>>> CreateBarber(CreateBarberDto createBarberDto)
        {
            ResponseModel<List<BarberModel>> response = new ResponseModel<List<BarberModel>>();

            try
            {
                var barber = new BarberModel()
                {
                    Name = createBarberDto.Name
                };
                _context.Add(barber);
                await _context.SaveChangesAsync();

                response.Dados = await _context.Barbers.ToListAsync();
                response.Message = "Colaborador cadastrado com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<BarberModel>>> DeleteBarber(int id)
        {
            ResponseModel<List<BarberModel>> response = new ResponseModel<List<BarberModel>>();

            try
            {
                var barber = _context.Barbers.FirstOrDefault(b => b.Id == id);

                if (barber == null)
                {
                    response.Message = "Nenhum colaborador encontrado!";
                    return response;
                }
                _context.Remove(barber);
                await _context.SaveChangesAsync();

                response.Dados = await _context.Barbers.ToListAsync();
                response.Message = "Colaborador excluido com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<BarberModel>> FindBarberById(int id)
        {
            ResponseModel<BarberModel> response = new ResponseModel<BarberModel>();

            try
            {
                var barber = await _context.Barbers.FirstOrDefaultAsync(bDatabase => bDatabase.Id == id);

                if (barber == null)
                {
                    response.Message = "Nenhum colaborador encontrado!";
                    return response;
                }

                response.Dados = barber;
                response.Message = "Colaborador Localizado!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<BarberModel>>> ListBarbers()
        {
            ResponseModel<List<BarberModel>> response = new ResponseModel<List<BarberModel>>();

            try
            {
                var barbers = await _context.Barbers.ToListAsync();
                response.Dados = barbers;
                response.Message = "Todos os colaboradores coletados!";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<BarberModel>>> UpdateBarber(UpdateBarberDto updateBarberDto)
        {
            ResponseModel<List<BarberModel>> response = new ResponseModel<List<BarberModel>>();

            try
            {
                var barber = await _context.Barbers.FirstOrDefaultAsync(b => b.Id == updateBarberDto.Id);
                if (barber == null)
                {
                    response.Message = "Colaborador não localizado!";
                    return response;
                }
                barber.Name = updateBarberDto.Name;

                _context.Update(barber);
                await _context.SaveChangesAsync();

                response.Dados = await _context.Barbers.ToListAsync();
                response.Message = "Dados do colaborador atualizados!";
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
