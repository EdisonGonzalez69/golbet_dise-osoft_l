// GolBet.Repositories/Interfaces/IMatchRepository.cs
using GolBet.Entities;
using GolBet.Entities.Enums;

namespace GolBet.Repositories.Interfaces;

public interface IMatchRepository : IGenericRepository<Match>
{
    // se agregó el parámetro search para filtrar por nombre de equipo
    Task<IEnumerable<Match>> GetAllWithTeamsAsync(MatchStatus? status = null, string? search = null); 
    Task<Match?> GetByIdWithDetailsAsync(int id);
}







// codigo original de la interfaz IMatchRepository.cs

/*
 
// GolBet.Repositories/Interfaces/IMatchRepository.cs
using GolBet.Entities;
using GolBet.Entities.Enums;

namespace GolBet.Repositories.Interfaces;

public interface IMatchRepository : IGenericRepository<Match>
{
    Task<IEnumerable<Match>> GetAllWithTeamsAsync(MatchStatus? status = null);
    Task<Match?> GetByIdWithDetailsAsync(int id);
}

*/