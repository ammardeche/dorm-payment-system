using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingEntity = global::DormPaymentSystem.Core.Entities.Building;
namespace DormPaymentSystem.Core.Interfaces
{
    public interface IBuildingMemberShipRepository
    {
        
    Task<List<BuildingEntity>> GetBuildingsOfUser(string userId);
    Task<bool> BuildingExist(int buildingId);
    Task<bool> MemberShipExist(string userId , int buildingId);
    Task AddMemberShip(string UserId , int buildingId);
    Task<bool> RemoveMembership(string userId , int buildingId);

    }
}