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

        public async Task<IEnumerable<Room>> GetAllRooms(RoomStatus? status, int? floorId)
        {
            var rooms = await _roomRepository.GetAllRooms(floorId, status);
            return rooms;
        }

        public async Task<Room?> GetRoomById(int id)
        {
            var room = await _roomRepository.GetRoomById(id);
            if (room == null)
                throw new AppNotFoundException($"Room with id '{id}' not found.");
            return room;
        }

        public async Task<Room> CreateRoom(int roomNumber, int capacity, int floorId)
        {
            // 1. check room number not duplicate
            if (await _roomRepository.RoomNumberExist(roomNumber))
                throw new AppConflictException($"Room number '{roomNumber}' already exists.");

            // 2. capacity must be at least 1
            if (capacity <= 0)
                throw new AppValidationException("Capacity must be at least 1.");

            var room = new Room
            {
                RoomNumber = roomNumber,
                Capacity = capacity,
                FloorId = floorId,
                Status = RoomStatus.Available
            };

            return await _roomRepository.CreateRoom(room);
        }

        public async Task<Room> UpdateRoom(int id, int roomNumber, int capacity, RoomStatus status)
        {
            var room = await _roomRepository.GetRoomById(id);
            if (room == null)
                throw new AppNotFoundException($"Room with id '{id}' not found.");

            // cannot reduce capacity below current occupancy
            if (capacity < room.Students.Count(s => s.IsActive))
                throw new AppValidationException("Cannot reduce capacity below current occupancy.");

            room.RoomNumber = roomNumber;
            room.Capacity = capacity;
            room.Status = status;

            await _roomRepository.UpdateRoom(room);
            return room;
        }

        public async Task<bool> DeleteRoom(int id)
        {
            var room = await _roomRepository.GetRoomById(id);
            if (room == null)
                throw new AppNotFoundException($"Room with id '{id}' not found.");

            // cannot delete room with active students
            if (room.Students.Any(s => s.IsActive))
                throw new AppValidationException("Cannot delete a room that has active students.");

            return await _roomRepository.DeleteRoom(room);
        }

        public async Task<bool> IsRoomAvailable(int roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            if (room == null)
                throw new AppNotFoundException($"Room with id '{roomId}' not found.");

            return room.Students.Count(s => s.IsActive) < room.Capacity;
        }

    }
}