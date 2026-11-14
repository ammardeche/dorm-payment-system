using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireBuildingAttribute : Attribute
    {
        
    }
}