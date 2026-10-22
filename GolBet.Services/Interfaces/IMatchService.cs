// GolBet.Services/Interfaces/IMatchService.cs
using GolBet.Entities.Enums;
using GolBet.Services.DTOs;

namespace GolBet.Services.Interfaces;

public interface IMatchService
{
    /// <summary>Match board: all active matches ordered by date.</summary>
    //se agregar un parámetro de búsqueda para filtrar los partidos por nombre de equipo 
    Task<IEnumerable<MatchDto>> GetBoardAsync(MatchStatus? status = null, string? search = null); 

    Task<MatchDetailDto?> GetDetailAsync(int id);
    Task<MatchFormDto?> GetForEditAsync(int id);

    Task CreateAsync(MatchFormDto dto);

    Task UpdateAsync(MatchFormDto dto);

    Task DeactivateAsync(int id);
}



/*
 * // GolBet.Services/Interfaces/IMatchService.cs
using GolBet.Entities.Enums;
using GolBet.Services.DTOs;

namespace GolBet.Services.Interfaces;

public interface IMatchService
{
    /// <summary>Match board: all active matches ordered by date.</summary>
    Task<IEnumerable<MatchDto>> GetBoardAsync(MatchStatus? status = null);

    Task<MatchDetailDto?> GetDetailAsync(int id);
    Task<MatchFormDto?> GetForEditAsync(int id);

    Task CreateAsync(MatchFormDto dto);

    Task UpdateAsync(MatchFormDto dto);

    Task DeactivateAsync(int id);
}

*/