using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BiblioManager.API.DTOs.Request;
using BiblioManager.API.DTOs.Response;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Services;

namespace BiblioManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController : ControllerBase
{
    private readonly ILoanService _loanService;
    private readonly IMapper _mapper;

    public LoanController(
        ILoanService loanService,
        IMapper mapper)
    {
        _loanService = loanService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LoanResponseDTO>>> GetAll()
    {
        var loans = await _loanService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<LoanResponseDTO>>(loans));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LoanResponseDTO>> GetById(int id)
    {
        try
        {
            var loan = await _loanService.GetByIdAsync(id);
            return Ok(_mapper.Map<LoanResponseDTO>(loan));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<LoanResponseDTO>> Create(LoanRequestDTO dto)
    {
        try
        {
            var loan = _mapper.Map<Loan>(dto);
            var created = await _loanService.CreateAsync(loan);
            var loanWithDetails = await _loanService.GetByIdAsync(created.Id);
            var responseDto = _mapper.Map<LoanResponseDTO>(loanWithDetails);

            return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message }); 
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message }); 
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, LoanRequestDTO dto)
    {
        try
        {
            var loan = _mapper.Map<Loan>(dto);
            await _loanService.UpdateAsync(id, loan);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message }); 
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _loanService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message }); 
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult> UpdateStatus(int id, UpdateLoanStatusDTO dto)
    {
        try
        {
            await _loanService.UpdateStatusAsync(id, dto.Status);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message }); 
        }
    }
}