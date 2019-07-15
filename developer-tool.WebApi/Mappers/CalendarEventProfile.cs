using AutoMapper;
using Domain.Models;
using WebApi.ViewModels;
using WebApi.InputModels;
using Domain.PersistenceModels;

public class CalendaEventProfile : Profile
{
	public CalendaEventProfile()
	{
		CreateMap<CalendarEvent, CalendarEventViewModel>();
		CreateMap<CalendarEventInputModel, CalendarEvent>()
			.ForMember(x => x.Id, opt => opt.Ignore());
		CreateMap<CalendarEventModel, CalendarEvent>();
		CreateMap<CalendarEvent, CalendarEventModel>()
			.ForMember(x => x.Id, opt => opt.Ignore());;
	}
}