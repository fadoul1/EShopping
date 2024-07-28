using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly ISender _sender;
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(ISender sender, ILogger<DiscountService> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public override async Task<CouponModel> GetDiscount(
        GetDiscountRequest request,
        ServerCallContext context
    )
    {
        var query = new GetDiscountQuery(request.ProductName);
        var result = await _sender.Send(query);

        _logger.LogInformation(
            "Discount is retrieved for the Product Name: {productName} and Amount : {amount}",
            request.ProductName,
            result.Amount
        );

        return result;
    }

    public override async Task<CouponModel> CreateDiscount(
        CreateDiscountRequest request,
        ServerCallContext context
    )
    {
        var cmd = new CreateDiscountCommand
        {
            ProductName = request.Coupon.ProductName,
            Amount = request.Coupon.Amount,
            Description = request.Coupon.Description
        };

        var result = await _sender.Send(cmd);

        _logger.LogInformation(
            "Discount is successfully created for the Product Name: {productName}",
            result.ProductName
        );

        return result;
    }

    public override async Task<CouponModel> UpdateDiscount(
        UpdateDiscountRequest request,
        ServerCallContext context
    )
    {
        var cmd = new UpdateDiscountCommand
        {
            Id = request.Coupon.Id,
            ProductName = request.Coupon.ProductName,
            Amount = request.Coupon.Amount,
            Description = request.Coupon.Description
        };

        var result = await _sender.Send(cmd);

        _logger.LogInformation(
            "Discount is successfully updated Product Name: {productName}",
            result.ProductName
        );

        return result;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(
        DeleteDiscountRequest request,
        ServerCallContext context
    )
    {
        var cmd = new DeleteDiscountCommand(request.ProductName);
        var deleted = await _sender.Send(cmd);

        var response = new DeleteDiscountResponse { Success = deleted };

        return response;
    }
}
