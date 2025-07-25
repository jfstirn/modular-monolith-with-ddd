using CompanyName.MyMeetings.BuildingBlocks.Domain;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Domain;

public class ApplicationUser : IdentityUser<Guid>, IEntity
{
    private List<IDomainEvent>? _domainEvents;

    protected ApplicationUser(string userName)
    : base(userName)
    {
    }

    private ApplicationUser()
        : base()
    {
        // Only EF.
    }

    public static Result<ApplicationUser, Error> CreateUser(
            UserId userId,
            string login,
            Email? email,
            string? firstName,
            string? lastName,
            string? name)
    {
        if (string.IsNullOrEmpty(login))
        {
            return Errors.General.ValueIsRequired(nameof(login));
        }

        return new ApplicationUser(login)
        {
            Id = userId.Value,
            FirstName = firstName,
            LastName = lastName,
            Name = name,
            Email = email
        };
    }

    /// <summary>
    /// Domain events occurred.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    /// <summary>
    /// Add domain event.
    /// </summary>
    /// <param name="domainEvent">Domain event.</param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= new List<IDomainEvent>();

        _domainEvents.Add(domainEvent);
    }

    protected void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }

    public virtual string? Name { get; set; }

    public virtual string? FirstName { get; set; }

    public virtual string? LastName { get; set; }
}