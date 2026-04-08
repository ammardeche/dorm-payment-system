using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? StudentId { get; set; }
        public NotificationStudentResponse? Student { get; set; }

        public NotificationResponse(Notification n)
        {
            Id = n.Id;
            Title = n.Title;
            Message = n.Message;
            IsRead = n.IsRead;
            Type = n.Type;
            CreatedAt = n.CreatedAt;


            StudentId = n.StudentId;
            Student = n.Student != null ? new NotificationStudentResponse(n.Student) : null;
        }



        public class NotificationStudentResponse
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public int? RoomNumber { get; set; }

            public NotificationStudentResponse(Student student)
            {
                Id = student.Id;
                FullName = $"{student.FirstName} {student.LastName}";
                RoomNumber = student.Room?.RoomNumber;
            }
        }

    }
}