using System;
using Common.Enums;

namespace WebApi.InputModels 
{
    public class ReminderInputModel {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public ReminderTimeOffset TimeOffset { get; set; }
    }
}