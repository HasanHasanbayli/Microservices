using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder
    .AddRedis("cache")
    .WithRedisInsight();

var mongoDb = builder
    .AddMongoDB("mongo")
    .AddDatabase("CatalogDb");


var basketService = builder
    .AddProject<FreeCourse_Services_Basket>("basket-service")
    .WithReference(cache)
    .WaitFor(cache);

var catalogService = builder
    .AddProject<FreeCourse_Services_Catalog>("catalog-service")
    .WithReference(mongoDb)
    .WaitFor(mongoDb);

// var discountService = builder.AddProject<FreeCourse_Services_Discount>("discount-service");
// var paymentService = builder.AddProject<FreeCourse_Services_FakePayment>("payment-service");
// var orderService = builder.AddProject<FreeCourse_Services_Order>("order-service");
// var photoStockService = builder.AddProject<FreeCourse_Services_PhotoStock>("photo-stock-service");
//
// var identityService = builder.AddProject<FreeCourse_IdentityServer>("identity-server");
// var webService = builder.AddProject<FreeCourse_Web>("web-service");
// var gatewayService = builder.AddProject<FreeCourse_Gateway>("gateway-service");


builder.Build().Run();