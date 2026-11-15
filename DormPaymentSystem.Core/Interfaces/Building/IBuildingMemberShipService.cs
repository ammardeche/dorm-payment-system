using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingEntity = global::DormPaymentSystem.Core.Entities.Building;

namespace DormPaymentSystem.Core.Interfaces.Building
{
    public interface IBuildingMemberShipService
    {

        Task<List<BuildingEntity>> GetBuildingsOfUser(string userId);
        Task AssignAsync(string userId, int buildingId);
        Task RemoveAsync(string userId, int buildingId);

    }
}