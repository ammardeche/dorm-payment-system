using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IGuestService
    {


        Task<(IEnumerable<Guest> Items, int TotalCount)> GetAllGuestsAsync(
                  string? documentNumber = null,
                  DocumentType? documentType = null,
                  string? fullName = null,
                  int pageIndex = 1,
                  int pageSize = 10);

        Task<Guest?> GetGuestByIdAsync(int id);
        Task<Guest?> GetGuestByDocumentNumberAsync(string documentNumber, DocumentType documentType);

        // just registers guest identity — room/dates/payment handled by ReservationService
        Task<Guest> RegisterGuestAsync(string fullName, string documentNumber, DocumentType documentType);

        Task<Guest> UpdateGuestAsync(int id, string? fullName, string? documentNumber, DocumentType? documentType);


        Task<bool> DeleteGuestAsync(int id);

    }
}