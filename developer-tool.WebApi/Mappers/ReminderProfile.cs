using AutoMapper;
using Infrastructure.Models;
using Domain.Models;
using WebApi.ViewModels;

public class ReminderProfile : Profile
{
	public ReminderProfile()
	{
		CreateMap<Domain.Models.Reminder, ReminderViewModel>();
		CreateMap<Domain.Models.Reminder, Infrastructure.Models.Reminder>();
		CreateMap<Infrastructure.Models.Reminder, Domain.Models.Reminder>();
	}
}