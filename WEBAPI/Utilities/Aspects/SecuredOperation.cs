using WEBAPI.Utilities.Extensions;
using WEBAPI.Utilities.IOC;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using WEBAPI.Constants;

namespace WEBAPI.Utilities.AutoFac
{
    public class SecuredOperation 
    {   //admin, edior
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }
      //removed protected override
            public  void OnBefore(IInvocation invocation)
            {
                var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
                foreach (var role in _roles)
                {
                    if (roleClaims.Contains(role))
                    {
                        return;//metodu çalıştırmaya devam et
                    }
                }
                throw new Exception(Messages.AuthorizationDenied);
            
        }
    }
}
