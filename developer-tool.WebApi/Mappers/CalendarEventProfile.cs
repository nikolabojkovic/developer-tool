using AutoMapper;
using Infrastructure.Models;
using Domain.Models;
using WebApi.ViewModels;

public class CalendaEventProfile : Profile
{
	public CalendaEventProfile()
	{
		CreateMap<CalendarEvent, CalendarEventViewModel>();
		CreateMap<Event, CalendarEvent>();
		CreateMap<CalendarEvent, Event>();
	}
}