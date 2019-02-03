using AutoMapper;
using Infrastructure.Models;
using Domain.Models;
using WebApi.ViewModels;
using WebApi.InputModels;

public class CalendaEventProfile : Profile
{
	public CalendaEventProfile()
	{
		CreateMap<CalendarEvent, CalendarEventViewModel>();
		CreateMap<CalendarEventInputModel, CalendarEvent>()
			.ForMember(x => x.Id, opt => opt.Ignore());
		CreateMap<EventModel, CalendarEvent>();
		CreateMap<CalendarEvent, EventModel>()
			.ForMember(x => x.Id, opt => opt.Ignore());;
	}
}