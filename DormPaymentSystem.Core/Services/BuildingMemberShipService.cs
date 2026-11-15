using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Core.Interfaces.Building;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.Core.Services
{
    public class BuildingMemberShipService : IBuildingMemberShipService
    {

        private readonly IBuildingMemberShipRepository _repo;
        private readonly IUserService _userService;
        public BuildingMemberShipService(IBuildingMemberShipRepository repo,
        IUserService userService
        )
        {
            _repo = repo;
            _userService = userService;
        }

        public async Task AssignAsync(string userId, int buildingId)
        {
            await _userService.GetUserById(userId);

            if (!await _repo.BuildingExist(buildingId))
            {
                throw new AppNotFoundException($"Building '{buildingId}' not found ");
            }
            if (await _repo.MemberShipExist(userId, buildingId))
            {
                throw new AppConflictException(" this user already belongs to this building . ");
            }

            await _repo.AddMemberShip(userId, buildingId);
        }

        public async Task<List<Building>> GetBuildingsOfUser(string userId)
        {
            return await _repo.GetBuildingsOfUser(userId);
        }

        public async Task RemoveAsync(string userId, int buildingId)
        {
            if (!await _repo.RemoveMembership(userId, buildingId))
                throw new AppNotFoundException("this user does not belong to this building");
        }
    }
}