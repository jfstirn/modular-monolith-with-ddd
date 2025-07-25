using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CSharpFunctionalExtensions;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.RefreshToken;

internal class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Contracts.Results.Result<TokenDto>>
{
    private readonly ITokenClaimsService _tokenService;

    public RefreshTokenCommandHandler(ITokenClaimsService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<Contracts.Results.Result<TokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _tokenService.GenerateNewTokensAsync(request.AccessToken, request.RefreshToken, cancellationToken)
            .Map(tokens => new TokenDto(tokens.AccessToken, tokens.RefreshToken));
    }
}