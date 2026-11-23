# Map: Guest, Invitation, Reservation

## 3 things, 3 questions
| Thing | Question | Table | Service |
|---|---|---|---|
| Guest | WHO is the person? | Guests | GuestService |
| Invitation | MAY he come? (student's request) | Invitations | InvitationService |
| Reservation | WHERE and WHEN does he sleep? | Reservations | ReservationService |

## Two stories

### Story A: guest invited by a student (free, no bed taken)
1. Student app    POST api/app/myinvitation/create
   -> InvitationService.CreateInvitationWithGuestAsync
      -> GuestService.RegisterGuestAsync        (finds or creates the GUEST)
      -> InvitationService.CreateInvitationAsync (creates the INVITATION, Pending)
2. Receptionist   GET api/admin/invitation/get-all        (sees the Pending invitation)
3. Receptionist   POST api/admin/invitation/approve/{id}  (TO BUILD: controller action)
   -> ReservationService.ApproveInvitationAsync
      - room = room of the inviter's active reservation
      - creates RESERVATION (InvitationStay, free)
      - invitation -> Used, ReservationId filled
4. Receptionist   POST api/admin/reservation/checkout/{id}
   -> ReservationService.CheckOutAsync  (must NOT free a bed for InvitationStay)

### Story B: stranger who rents a night (paid, takes a bed)
1. Receptionist   POST api/admin/guest/create             -> GuestService.RegisterGuestAsync
2. Receptionist   POST api/admin/reservation/guest        -> ReservationService.CreateGuestReservationAsync
      - checks room capacity, creates RESERVATION (GuestStay) + PAYMENT, occupancy +1
3. Receptionist   POST api/admin/reservation/checkout/{id} -> occupancy -1
(no invitation in this story)

## Which service creates what
- GuestService        -> Guest
- InvitationService   -> Invitation
- ReservationService  -> Reservation (+ Payment for GuestStay)

## Open items (your list)
- [ ] approve endpoint in InvitationController
- [ ] CheckOutAsync: skip occupancy change for InvitationStay
- [ ] delete CreateInvitationReservationAsync (service, interface, controller) + its request class
- [ ] Status filter in InvitationQuery
- [ ] admin InvitationController.create still calls the old CreateInvitationAsync(studentId, guestId): decide later (admin door)

## How to read the 3 services (one by one, 10 minutes each)
1. GuestService.RegisterGuestAsync: who calls it? (2 callers)
2. InvitationService.CreateInvitationWithGuestAsync: follow it into step 1 and into CreateInvitationAsync
3. ReservationService.ApproveInvitationAsync: find where each of the 3 tables is touched
After each one, close the file and write 3 lines: what goes in, what it creates, what can fail.
