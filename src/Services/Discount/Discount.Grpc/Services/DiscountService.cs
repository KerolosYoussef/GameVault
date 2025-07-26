namespace Discount.Grpc.Services
{
    public class DiscountService : Grpc.DiscountService.DiscountServiceBase
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly DiscountContext _context;

        public DiscountService(ILogger<DiscountService> logger, DiscountContext context)
        {
            _logger = logger;
            _context = context;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.ProductName == request.ProductName, context.CancellationToken);

            // If coupon is null, create a default one
            coupon ??= new Coupon { ProductName = "No Discount", Description = "No Discount Description", Amount = 0 };

            // Log the retrieved coupon
            _logger.LogInformation("Retrieved coupon: {@Coupon}", coupon);

            var couponModel = coupon.Adapt<CouponModel>();

            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            // map coupon model to coupon
            var coupon = request.Coupon.Adapt<Coupon>();

            // if coupon is null, throw an exception
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

            // add coupon to the database
            var result = await _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync(context.CancellationToken);

            // log the created coupon
            _logger.LogInformation("Created coupon: {@Coupon}", result.Entity);

            // map the result to CouponModel
            var couponModel = result.Entity.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            // map coupon model to coupon
            var coupon = request.Coupon.Adapt<Coupon>();

            // if coupon is null, throw an exception
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

            _context.Coupons.Update(coupon);
            await _context.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("Discount is successfully updated, {@Coupon}", coupon);

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully deleted, {@Coupon}", coupon);

            return new DeleteDiscountResponse()
            {
                Success = true
            };
        }
    }
}
