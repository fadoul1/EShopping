using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories;

public class DiscountRepository(IConfiguration configuration) : IDiscountRepository
{
    public async Task<Coupon> GetDiscount(string productName)
    {
        await using var connection = new NpgsqlConnection(
            configuration.GetValue<string>("DatabaseSettings:ConnectionString")
        );
        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
            "SELECT * FROM Coupon WHERE ProductName = @ProductName",
            new { ProductName = productName }
        );
        if (coupon == null)
            return new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Available"
            };
        return coupon;
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(
            configuration.GetValue<string>("DatabaseSettings:ConnectionString")
        );

        var affected = await connection.ExecuteAsync(
            "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
            new
            {
                coupon.ProductName,
                coupon.Description,
                coupon.Amount
            }
        );

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(
            configuration.GetValue<string>("DatabaseSettings:ConnectionString")
        );

        var affected = await connection.ExecuteAsync(
            "UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
            new
            {
                coupon.ProductName,
                coupon.Description,
                coupon.Amount,
                coupon.Id
            }
        );

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        await using var connection = new NpgsqlConnection(
            configuration.GetValue<string>("DatabaseSettings:ConnectionString")
        );

        var affected = await connection.ExecuteAsync(
            "DELETE FROM Coupon WHERE ProductName = @ProductName",
            new { ProductName = productName }
        );

        if (affected == 0)
            return false;

        return true;
    }
}
