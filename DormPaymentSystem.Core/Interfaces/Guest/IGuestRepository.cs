using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IGuestRepository
    {
        Task<(IEnumerable<Guest> Items, int TotalCount)> GetGuests(
           string? documentNumber,
           DocumentType? documentType = null,
           string? fullName = null,
           int pageIndex = 1,
           int pageSize = 10);

        Task<Guest?> GetGuestById(int id);
        Task<Guest?> GetGuestByDocumentNumber(string documentNumber, DocumentType? documentType);

        Task<Guest> CreateGuest(Guest guest);
        Task<Guest> UpdateGuest(Guest guest);
        Task<bool> DeleteGuest(Guest guest);
        Task<bool> GuestExists(int id);
        Task SaveChanges();


    }
}