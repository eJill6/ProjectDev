using FluentValidation;
using MS.Core.MMModel.Models.AdminUserManager;

namespace MMService.Validators
{
	public class AdminUserManager
	{
	}

	public class AdminUserManagerIdentityApplyParamValidator : AbstractValidator<AdminUserManagerIdentityApplyParam>, IFluentValidation
    {
		public AdminUserManagerIdentityApplyParamValidator()
		{
			RuleFor(u => u.UserId).NotEmpty().WithMessage("会员ID错误");
			RuleFor(u => u.EarnestMoney).GreaterThanOrEqualTo(0).WithMessage("编辑保证金仅允许正整数或0");
			RuleFor(u => u.ExtraPostCount).GreaterThanOrEqualTo(0).WithMessage("增加发贴次数仅允许正整数或0");
        }
	}
    
    public class AdminUserManagerIdentityAuditParamValidator : AbstractValidator<AdminUserManagerIdentityAuditParam>, IFluentValidation
    {
        public AdminUserManagerIdentityAuditParamValidator()
        {
            RuleFor(u => u.EarnestMoney).GreaterThanOrEqualTo(0).WithMessage("编辑保证金仅允许正整数或0");
            RuleFor(u => u.ExtraPostCount).GreaterThanOrEqualTo(0).WithMessage("增加发贴次数仅允许正整数或0");
        }
    }
}
