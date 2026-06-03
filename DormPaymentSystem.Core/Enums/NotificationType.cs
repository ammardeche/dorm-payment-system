namespace DormPaymentSystem.Core.Enums
{
    public enum NotificationType
    {
        // admin → student (manual)
        GeneralAnnouncement,     // "clean your room"
        PaymentReminder,         // "your payment is due"
        CheckInReminder,         // "inspection on Feb 3"
        LeaveConfirmation,       // "confirm if you stay next semester"

        // system → admin (automatic)
        PaymentDueToday,         // "Ahmed has payment due today"
        OverduePayment,          // "3 students have overdue payments"
        StudentLeavingSoon       // "Ahmed is leaving in 3 days",
    }
}