using System;
using Domain.Enums;

namespace WebApi.InputModels 
{
    public class ReminderInputModel {
        public bool Active { get; set; }
        public DateTime Time { get; set; }
        public ReminderTimeOffset TimeOffset { get; set; }
    }
}