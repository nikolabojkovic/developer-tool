using System;
using Common.Enums;
using Infrastructure.Core;

namespace Infrastructure.Models 
{
    public class Event : Entity
    {
        public string Color { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public Reminder Reminder { get; set; }
    }
}