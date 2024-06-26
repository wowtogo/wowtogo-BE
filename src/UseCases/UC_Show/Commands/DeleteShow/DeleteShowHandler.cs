using Ardalis.Result;
using Domain.Enums;
using Domain.Interfaces.Data;
using Domain.Models;
using MediatR;

namespace UseCases.UC_Show.Commands.DeleteShow;
public class DeleteShowHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteShowCommand, Result>
{
    public async Task<Result> Handle(DeleteShowCommand request, CancellationToken cancellationToken)
    {
        Show? checkingShow = await unitOfWork.ShowRepository.GetShowIncludingEventAsync(request.ShowId, cancellationToken: cancellationToken);
        if (checkingShow is null) return Result.NotFound("Show not found");
        if (checkingShow.Event?.Status == EventStatusEnum.Canceled) return Result.Error("Cannot delete show of a canceled event");
        if (checkingShow.Event?.Status == EventStatusEnum.Completed) return Result.Error("Cannot delete show of a completed event");
        if (checkingShow.Event?.Status == EventStatusEnum.Published) return Result.Error("Cannot delete show of a published event");
        if (checkingShow.TicketTypes.Any()) unitOfWork.TicketTypeRepository.RemoveRange(checkingShow.TicketTypes);
        unitOfWork.ShowRepository.Remove(checkingShow);
        if (!await unitOfWork.SaveChangesAsync(cancellationToken)) return Result.Error("Failed to delete show");
        return Result.SuccessWithMessage("Show is deleted successfully");
    }
}