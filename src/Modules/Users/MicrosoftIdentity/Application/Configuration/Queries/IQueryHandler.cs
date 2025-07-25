using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using MediatR;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Queries;

public interface IQueryHandler<in TQuery, TResult> :
    IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
}