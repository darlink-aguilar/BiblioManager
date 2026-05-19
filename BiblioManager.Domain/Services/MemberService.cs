using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Repositories;
using BiblioManager.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BiblioManager.Domain.Services;

public class MemberService : IMemberService
// SE REPITE EL METODO DE VALIDACION DE EMAIL 

{
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<MemberService> _logger;

    public MemberService(IMemberRepository memberRepository, ILogger<MemberService> logger)
    {
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all Members");
        return await _memberRepository.GetAllAsync();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving Member with ID: {MemberId}", id);
        var member = await _memberRepository.GetByIdAsync(id);

        if (member == null)
            _logger.LogWarning("Member with ID {MemberId} not found", id);

        return member;
    }

    public async Task<Member> CreateAsync(Member member)
    {
        // VALIDACIONES
        // Buscamos si ya existe un usuario con el mismo DNI
        var existingMember = await _memberRepository.GetByDniAsync(member.Dni);
        if (existingMember != null)
        {
            _logger.LogWarning("Attempted to create duplicate Member: {Dni} ", member.Dni);
            throw new InvalidOperationException(
                $"Ya existe un usuario registrado con el DNI '{member.Dni}'");
        }

        // Validamos que el correo tenga un formato valido
        var emailValidator = new EmailAddressAttribute(); // Clase de entity framework para validar formato de email

        if (string.IsNullOrWhiteSpace(member.Email) ||
            !emailValidator.IsValid(member.Email))
        {
            _logger.LogWarning("Invalid email format for member: {Email}", member.Email);
            throw new InvalidOperationException("El correo electrónico no tiene un formato válido");
        }

        member.IsActive = true; // Valor por defecto al crear un nuevo miembro
        _logger.LogInformation("Creating Member: {FullName}", member.FullName);
        return await _memberRepository.CreateAsync(member);
    }

    public async Task UpdateAsync(int id, Member member)
    {
        var existingMember = await _memberRepository.GetByIdAsync(id);
        if (existingMember == null)
        {
            _logger.LogWarning("Member with ID {MemberId} not found for update", id);
            throw new KeyNotFoundException(
                $"No se encontró el usuario con ID {id}");
        }

        // VALIDACIONES
        var repeatedMember = await _memberRepository.GetByDniAsync(member.Dni);

        if (repeatedMember != null && repeatedMember.Id != id)
        {
            _logger.LogWarning("Attempted to create duplicate Member: {Dni}", member.Dni);
            throw new InvalidOperationException(
                $"Ya existe un usuario registrado con el DNI '{member.Dni}'");
        }

        var emailValidator = new EmailAddressAttribute(); // Clase de entity framework para validar formato de email

        if (string.IsNullOrWhiteSpace(member.Email) ||
            !emailValidator.IsValid(member.Email))
        {
            _logger.LogWarning("Invalid email format for member: {Email}", member.Email);
            throw new InvalidOperationException("El correo electrónico no tiene un formato válido");
        }

        existingMember.Dni = member.Dni;
        existingMember.FullName = member.FullName;
        existingMember.Email = member.Email;
        existingMember.BirthDate = member.BirthDate;

        _logger.LogInformation("Updating Member with ID: {MemberId}", id);
        await _memberRepository.UpdateAsync(existingMember);
    }

    public async Task ActivateDeactivateAsync(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member == null)
        {
            _logger.LogWarning("Member with ID {MemberId} not found", id);
            throw new KeyNotFoundException($"No se encontró el usuario con ID {id}");
        }

        // Cambiamos el estado a Desactivado o Activado 
        member.IsActive = !member.IsActive;

        _logger.LogInformation("Member {MemberId} state changed to: {State}", id, member.IsActive);
        await _memberRepository.UpdateAsync(member);
    }
}