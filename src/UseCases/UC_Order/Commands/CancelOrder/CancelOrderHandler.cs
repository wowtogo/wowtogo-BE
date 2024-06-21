using Ardalis.Result;
using Domain.Enums;
using Domain.Interfaces.Data;
using Domain.Models;
using MediatR;

namespace UseCases.UC_Order.Commands.CancelOrder;
public class CancelOrderHandler(IUnitOfWork unitOfWork) : IRequestHandler<CancelOrderCommand, Result>
{
    public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        Order? checkingOrder = await unitOfWork.OrderRepository.FindAsync(o => o.Id == request.OrderId, trackChanges: true, cancellationToken: cancellationToken);
        if (checkingOrder == null)
            return Result.NotFound("Order is not found");
        if (checkingOrder.Status == OrderStatusEnum.Canceled)
            return Result.Error("Order is already canceled");
        if (checkingOrder.Status == OrderStatusEnum.Paid)
            return Result.Error("Order is already paid");
        if(checkingOrder.Status == OrderStatusEnum.Refunded)
            return Result.Error("Order is already refunded");
        checkingOrder.CancelOrder();
        if (!await unitOfWork.SaveChangesAsync(cancellationToken)) return Result.Error("Failed to cancel order");
        return Result.SuccessWithMessage("Order is canceled successfully");
    }
}