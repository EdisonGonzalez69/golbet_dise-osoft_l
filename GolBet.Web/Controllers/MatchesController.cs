
// GolBet.Web/Controllers/MatchesController.cs
using GolBet.Entities.Enums;
using GolBet.Repositories.Data;
using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GolBet.Web.Controllers;

public class MatchesController : Controller
{
    private readonly IMatchService _matchService;
    private readonly ITeamService _teamService;

    public MatchesController(IMatchService matchService, ITeamService teamService)
    {
        _matchService = matchService;
        _teamService = teamService;
    }

    //
    // MODIFICACIÓN 
    // 1. Se agrega el parámetro  'string? search' para recibir el término de búsqueda por equipo.
    // 2. Se almacena 'search' en ViewBag.CurrentSearch para mantener el valor en la caja de texto de la vista.
    // 3. Se pasa 'search' al método GetBoardAsync de la capa de servicios cumpliendo la arquitectura N-Capas.
    //
    // GET /Matches                      -> todos los partidos 
    // GET /Matches?status=Scheduled     -> filtrado por estado
    // GET /Matches?search=Nacional     -> filtrado por nombre de equipo
    public async Task<IActionResult> Index(MatchStatus? status = null, string? search = null) // CAMBIO 
    {
        // Se preservan los filtros en la vista para mantener la consistencia 
        ViewBag.CurrentStatus = status;
        ViewBag.CurrentSearch = search; // CAMBIO: Guarda la búsqueda actual para la vista

        // CAMBIO: Se envía 'search' junto con 'status' a la capa de servicios
        var board = await _matchService.GetBoardAsync(status, search);

        return View(board);
    }

    // GET /Matches/Detail/3 
    public async Task<IActionResult> Detail(int id)
    {
        var match = await _matchService.GetDetailAsync(id);

        if (match is null) return NotFound();   // HTTP 404 

        return View(match);
    }

    [Authorize(Roles = DbSeeder.AdminRole)]
    // GET /Matches/Create 
    public async Task<IActionResult> Create()
    {
        await LoadTeamsAsync();

        return View(new MatchFormDto());
    }

    [Authorize(Roles = DbSeeder.AdminRole)]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MatchFormDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadTeamsAsync();

            return View(dto);
        }

        try
        {
            await _matchService.CreateAsync(dto);

            TempData["Success"] = "Partido creado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)      // business rule violated 
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            await LoadTeamsAsync();

            return View(dto);
        }
    }

    [Authorize(Roles = DbSeeder.AdminRole)]
    // GET /Matches/Edit/5 
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _matchService.GetForEditAsync(id);

        if (dto is null) return NotFound();

        await LoadTeamsAsync(); // the form needs the dropdowns too 

        return View(dto);
    }

    [Authorize(Roles = DbSeeder.AdminRole)]
    // POST /Matches/Edit 
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MatchFormDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadTeamsAsync();

            return View(dto);
        }

        try
        {
            await _matchService.UpdateAsync(dto);

            TempData["Success"] = "Partido actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)      // business rule violated 
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            await LoadTeamsAsync();

            return View(dto);
        }
    }

    [Authorize(Roles = DbSeeder.AdminRole)]
    // POST /Matches/Deactivate/5 
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(int id)
    {
        await _matchService.DeactivateAsync(id);

        TempData["Success"] = "Partido desactivado.";

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadTeamsAsync()
    {
        var teams = await _teamService.GetAllAsync();

        ViewBag.Teams = new SelectList(teams, "Id", "Name");
    }
}


/*
 // GolBet.Web/Controllers/MatchesController.cs  (versión completa) 

using GolBet.Entities.Enums;
using GolBet.Repositories.Data;
using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GolBet.Web.Controllers;

public class MatchesController : Controller

{
    private readonly IMatchService _matchService;
    private readonly ITeamService _teamService;

    public MatchesController(IMatchService matchService, ITeamService teamService)

    {
        _matchService = matchService;
        _teamService = teamService;
    }


    // GET /Matches            -> all matches 

    // GET /Matches?status=Scheduled -> filtered board 

    public async Task<IActionResult> Index(MatchStatus? status)

    {

        ViewBag.CurrentStatus = status;

        var board = await _matchService.GetBoardAsync(status);

        return View(board);

    }

    // GET /Matches/Detail/3 

    public async Task<IActionResult> Detail(int id)

    {
        var match = await _matchService.GetDetailAsync(id);

        if (match is null) return NotFound();   // HTTP 404 

        return View(match);
    }



    [Authorize(Roles = DbSeeder.AdminRole)]
    // GET /Matches/Create 

    public async Task<IActionResult> Create()

    {

        await LoadTeamsAsync();

        return View( new MatchFormDto());

    }



    [Authorize(Roles = DbSeeder.AdminRole)]

    [HttpPost, ValidateAntiForgeryToken]

    public async Task<IActionResult> Create(MatchFormDto dto)

    {

        if (!ModelState.IsValid)

        {

            await LoadTeamsAsync();

            return View(dto);

        }



        try

        {

            await _matchService.CreateAsync(dto);

            TempData["Success"] = "Partido creado correctamente.";

            return RedirectToAction(nameof(Index));

        }

        catch (InvalidOperationException ex)      // business rule violated 

        {

            ModelState.AddModelError(string.Empty, ex.Message);

            await LoadTeamsAsync();

            return View(dto);

        }

    }



    [Authorize(Roles = DbSeeder.AdminRole)]

    // GET /Matches/Edit/5 

    public async Task<IActionResult> Edit(int id)

    {

        var dto = await _matchService.GetForEditAsync(id);

        if (dto is null) return NotFound();



        await LoadTeamsAsync(); // the form needs the dropdowns too 

        return View(dto);

    }



    [Authorize(Roles = DbSeeder.AdminRole)]

    // POST /Matches/Edit 

    [HttpPost, ValidateAntiForgeryToken]

    public async Task<IActionResult> Edit(MatchFormDto dto)

    {

        if (!ModelState.IsValid)

        {

            await LoadTeamsAsync();

            return View(dto);

        }



        try

        {

            await _matchService.UpdateAsync(dto);

            TempData["Success"] = "Partido actualizado correctamente.";

            return RedirectToAction(nameof(Index));

        }

        catch (InvalidOperationException ex)      // business rule violated 

        {

            ModelState.AddModelError(string.Empty, ex.Message);

            await LoadTeamsAsync();

            return View(dto);

        }

    }



    [Authorize(Roles = DbSeeder.AdminRole)]

    // POST /Matches/Deactivate/5 

    [HttpPost, ValidateAntiForgeryToken]

    public async Task<IActionResult> Deactivate(int id)

    {

        await _matchService.DeactivateAsync(id);

        TempData["Success"] = "Partido desactivado.";

        return RedirectToAction(nameof(Index));

    }



    private async Task LoadTeamsAsync()

    {

        var teams = await _teamService.GetAllAsync();

        ViewBag.Teams = new SelectList(teams, "Id", "Name");

    }

}
*/