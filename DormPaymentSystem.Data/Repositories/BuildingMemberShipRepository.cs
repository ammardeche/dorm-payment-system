using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Repositories
{
    public class BuildingMemberShipRepository : IBuildingMemberShipRepository
    {

        private readonly DormPaymentDbContext _context;

        public BuildingMemberShipRepository(DormPaymentDbContext context)
        {
            _context = context;
        }

        public async Task AddMemberShip(string UserId, int buildingId)
        {
            await _context.BuildingMemberships.AddAsync(new BuildingMembership {UserId = UserId , BuildingId =  buildingId , });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> BuildingExist(int buildingId) => 
                await _context.Buildings.AnyAsync(m=>m.Id == buildingId);

        public async Task<List<Building>> GetBuildingsOfUser(string userId)=>
            await _context.Buildings.Where(
                b => _context.BuildingMemberships.Any(
                    m=> m.UserId == userId && m.BuildingId == b.Id)).ToListAsync();
        

        public async Task<bool> MemberShipExist(string userId, int buildingId)
        => await _context.BuildingMemberships.AnyAsync(m=>m.UserId == userId && m.BuildingId == buildingId );

        public async Task<bool> RemoveMembership(string userId, int buildingId)
        {
           var membership = await _context.BuildingMemberships.FirstOrDefaultAsync(
            m=>m.UserId == userId && m.BuildingId == buildingId
           );

           if(membership == null) return false; 

            _context.BuildingMemberships.Remove(membership);
           await _context.SaveChangesAsync();
           return true;
        }
    }
}