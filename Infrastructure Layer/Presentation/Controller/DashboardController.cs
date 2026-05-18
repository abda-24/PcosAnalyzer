using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs.Diagnosis;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDiagnosisService _diagnosisService;

    public DashboardController(IDiagnosisService diagnosisService)
    {
        _diagnosisService = diagnosisService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<DashboardDto>> GetDashboard()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var dashboard = await _diagnosisService.GetDashboardDataAsync(userId);
        return Ok(dashboard);
    }
}
