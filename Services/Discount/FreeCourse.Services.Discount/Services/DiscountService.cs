using System.Data;
using Dapper;
using FreeCourse.Shared.DTOs;
using Npgsql;

namespace FreeCourse.Services.Discount.Services;

public class DiscountService : IDiscountService
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _dbConnection;


    public DiscountService(IConfiguration configuration)
    {
        _configuration = configuration;
        _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
    }

    public async Task<Response<List<Models.Discount>>> GetAll()
    {
        var discounts = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount");

        return Response<List<Models.Discount>>.Success(discounts.ToList(), 200);
    }

    public async Task<Response<Models.Discount>> GetById(int id)
    {
        var discount =
            (await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where id=@Id", new { Id = id }))
            .SingleOrDefault();

        return discount == null
            ? Response<Models.Discount>.Fail("Discount not found", 404)
            : Response<Models.Discount>.Success(discount, 200);
    }

    public async Task<Response<NoContent>> Save(Models.Discount discount)
    {
        var status =
            await _dbConnection.ExecuteAsync("INSERT INTO discount (userId, rate, code) VALUES (@UserId, @Rate,@Code)",
                discount);

        return status > 0
            ? Response<NoContent>.Success(204)
            : Response<NoContent>.Fail("An error occurred while adding", 500);
    }

    public async Task<Response<NoContent>> Update(Models.Discount discount)
    {
        var status =
            await _dbConnection.ExecuteAsync("UPDATE discount set userId=@UserId, rate=@Rate, code=@Code where id=@Id",
                new { discount.Id, discount.UserId, discount.Rate, discount.Code });

        return status > 0
            ? Response<NoContent>.Success(204)
            : Response<NoContent>.Fail("Discount not found", 404);
    }

    public async Task<Response<NoContent>> Delete(int id)
    {
        var status =
            await _dbConnection.ExecuteAsync("DELETE from discount where id=@Id", new { Id = id });

        return status > 0
            ? Response<NoContent>.Success(204)
            : Response<NoContent>.Fail("Discount not found", 404);
    }

    public async Task<Response<Models.Discount>> GetByCodeAndUserid(string code, string userId)
    {
        var discount =
            await _dbConnection.QueryAsync<Models.Discount>(
                "Select * from discount where userId=@UserId and code=@Code", new { UserId = userId, Code = code });

        var hasDiscount = discount.FirstOrDefault();

        return hasDiscount != null
            ? Response<Models.Discount>.Success(hasDiscount, 200)
            : Response<Models.Discount>.Fail("Discount not found", 404);
    }
}