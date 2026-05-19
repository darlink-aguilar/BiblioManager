using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BiblioManager.API.DTOs.Request;
using BiblioManager.API.DTOs.Response;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Services;

namespace BiblioManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly IMapper _mapper;
    private readonly ILogger<MemberController> _logger;

    public MemberController(
        IMemberService memberService,
        IMapper mapper,
        ILogger<MemberController> logger)
    {
        _memberService = memberService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponseDTO>>> GetAll()
    {
        var members = await _memberService.GetAllAsync();
        var membersDto = _mapper.Map<IEnumerable<MemberResponseDTO>>(members);
        return Ok(membersDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberResponseDTO>> GetById(int id)
    {
        var member = await _memberService.GetByIdAsync(id);

        if (member == null)
        {
            return NotFound(new { message = $"Usuario con ID {id} no encontrado" });
        }

        var memberDto = _mapper.Map<MemberResponseDTO>(member);
        return Ok(memberDto);
    }

    [HttpPost]
    public async Task<ActionResult<MemberResponseDTO>> Create(MemberRequestDTO dto)
    {
        try
        {
            var member = _mapper.Map<Member>(dto);
            var createdMember = await _memberService.CreateAsync(member);
            var responseDto = _mapper.Map<MemberResponseDTO>(createdMember);

            return CreatedAtAction(
                nameof(GetById),
                new { id = responseDto.Id },
                responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, MemberRequestDTO dto)
    {
        try
        {
            var member = _mapper.Map<Member>(dto);
            await _memberService.UpdateAsync(id, member);
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

    [HttpPatch("{id}/IsActive")] // HttpPatch porque no vamos a modificar toda la entidad
    public async Task<ActionResult> UpdateIsActive(int id)
    {
        try
        {
            await _memberService.ActivateDeactivateAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}