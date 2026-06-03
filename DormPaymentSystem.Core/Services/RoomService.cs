using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;
using DormPaymentSystem.Core.Exceptions;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Repositories;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.Core.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<(IEnumerable<Room> Items, int TotalCount)> GetAllRoomsAsync(
            RoomStatus? status = null,
            int? floorNumber = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            return await _roomRepository.GetAllRooms(
                status, floorNumber, pageIndex, pageSize);
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetRoomById(id);
            if (room == null)
                throw new AppNotFoundException($"Room with id '{id}' not found.");
            return room;
        }

        public async Task<Room> CreateRoomAsync(
            int roomNumber, int capacity, int floorNumber)
        {
            if (await _roomRepository.RoomNumberExist(roomNumber))
                throw new AppConflictException(
                    $"Room number '{roomNumber}' already exists.");

            if (capacity <= 0)
                throw new AppValidationException("Capacity must be at least 1.");

            if (floorNumber <= 0)
                throw new AppValidationException("Floor number must be at least 1.");

            var room = new Room
            {
                RoomNumber = roomNumber,
                Capacity = capacity,
                FloorNumber = floorNumber,  // simple int — no FK
                Status = RoomStatus.Available,
                CurrentOccupancy = 0
            };

            return await _roomRepository.CreateRoom(room);
        }

        public async Task<Room> UpdateRoomAsync(
            int id, int? roomNumber, int? capacity, RoomStatus? status)
        {
            var room = await _roomRepository.GetRoomById(id);
            if (room == null)
                throw new AppNotFoundException($"Room with id '{id}' not found.");

            if (capacity.HasValue)
            {
                // cannot reduce below active occupancy
                var activeCount = room.Students.Count(s => s.IsActive);
                if (capacity.Value < activeCount)
                    throw new AppValidationException(
                        $"Cannot reduce capacity below current occupancy ({activeCount}).");
                room.Capacity = capacity.Value;
            }

            if (roomNumber.HasValue && roomNumber.Value != room.RoomNumber)
            {
                if (await _roomRepository.RoomNumberExist(roomNumber.Value))
                    throw new AppConflictException(
                        $"Room number '{roomNumber}' already exists.");
                room.RoomNumber = roomNumber.Value;
            }

            if (status.HasValue)
                room.Status = status.Value;

            await _roomRepository.UpdateRoom(room);
            return room;
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _roomRepository.GetRoomById(id);
            if (room == null)
                throw new AppNotFoundException($"Room with id '{id}' not found.");

            if (room.Students.Any(s => s.IsActive))
                throw new AppValidationException(
                    "Cannot delete a room with active students.");

            var hasActiveReservation = room.Reservations
                .Any(r => r.Status == Core.Enums.ReservationStatus.Active);
            if (hasActiveReservation)
                throw new AppValidationException(
                    "Cannot delete a room with an active reservation.");

            return await _roomRepository.DeleteRoom(room);
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            if (room == null)
                throw new AppNotFoundException($"Room with id '{roomId}' not found.");

            return room.CurrentOccupancy < room.Capacity;
        }
    }
}
