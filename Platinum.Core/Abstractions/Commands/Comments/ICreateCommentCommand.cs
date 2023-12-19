namespace Platinum.Core.Abstractions.Commands.Comments
{
    public interface ICreateCommentCommand<TReturn> :
        ICommandHandler<TReturn>, ICommandHandlerAsync<TReturn>
    {
    }
}
