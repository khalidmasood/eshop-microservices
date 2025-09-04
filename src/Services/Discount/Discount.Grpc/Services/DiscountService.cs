using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {

        public override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            // TODO: GetDiscount from Database
            return base.GetDiscount(request, context);
        }

        public override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            // TODO: AddDiscount to the Database
            return base.CreateDiscount(request, context);
        }

        // TODO: UpdateDiscount to the Database
        public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            return base.UpdateDiscount(request, context);
        }

        // TODO: DeleteDiscount from the Database
        public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            return base.DeleteDiscount(request, context);
        }
    }
}
 