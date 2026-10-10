using DenunciaPeAPI.Application.DTOs;
using DenunciaPeAPI.Application.Exceptions;
using DenunciaPeAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DenunciaPeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DenunciasController(IDenunciaService denunciaService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DenunciaResumenDto>>> GetAll()
        => Ok(await denunciaService.GetAllAsync());

    [HttpGet("denunciante/{nroDocumento}")]
    public async Task<ActionResult<IEnumerable<DenunciaResumenDto>>> GetByNroDocumento(string nroDocumento)
    {
        return Ok(await denunciaService.GetByNroDocumentoAsync(nroDocumento));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DenunciaDto>> GetById(int id)
    {
        var denuncia = await denunciaService.GetByIdAsync(id);
        return denuncia is null ? NotFound() : Ok(denuncia);
    }

    [HttpPost]
    public async Task<ActionResult<DenunciaDto>> Create(CreateDenunciaRequestDto dto)
    {
        try
        {
            var created = await denunciaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (DenunciaValidationException ex)
        {
            var problem = new ValidationProblemDetails
            {
                Title = "La solicitud de denuncia no cumple la estructura o formato requerido.",
                Status = StatusCodes.Status400BadRequest,
                Errors = { ["denuncia"] = [.. ex.Errores] }
            };
            return BadRequest(problem);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await denunciaService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
