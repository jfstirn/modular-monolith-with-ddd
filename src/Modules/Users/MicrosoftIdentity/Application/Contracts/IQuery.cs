using MediatR;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}