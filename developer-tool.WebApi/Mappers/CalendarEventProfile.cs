using AutoMapper;
using Infrastructure.Models;
using WebApi.ViewModels;

public class CalendaEventProfile : Profile
{
	public CalendaEventProfile()
	{
		CreateMap<CalendarEvent, CalendarEventViewModel>();
	}
}