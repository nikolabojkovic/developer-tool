using System;
using Common.Enums;

namespace WebApi.ViewModels {
    public class ReminderViewModel {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public ReminderTimeOffset TimeOffset { get; set; }
    }
}