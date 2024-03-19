using Domain.Transport;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class TransportRepository(ApplicationDbContext context) : ITransportRepository
{
    public async Task<IEnumerable<TransportType>> GetTransportTypes()
    {
        return await context.TransportTypes.ToListAsync();
    }

    public async Task<TransportType> GetTransportTypeById(Guid id)
    {
        return await context.TransportTypes.FindAsync(id);
    }

    public async Task<TransportMake> GetTransportMakeById(Guid id)
    {
        return await context.TransportMakes.FindAsync(id);
    }

    public async Task<TransportModel> GetTransportModelById(Guid id)
    {
        return await context.TransportModels.FindAsync(id);
    }

    public async Task<TransportBodyType> GetTransportBodyTypeById(Guid id)
    {
        return await context.TransportBodyTypes.FindAsync(id);
    }
}