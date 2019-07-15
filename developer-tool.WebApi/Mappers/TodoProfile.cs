using AutoMapper;
using Domain.Models;
using WebApi.ViewModels;
using WebApi.InputModels;
using Domain.PersistenceModels;

public class TodoProfile : Profile
{
	public TodoProfile()
	{
		CreateMap<Todo, TodoViewModel>();
		CreateMap<TodoInputModel, Todo>()
			.ForMember(x => x.Id, opt => opt.Ignore());
		CreateMap<TodoModel, Todo>();
		CreateMap<Todo, TodoModel>()
			.ForMember(x => x.Id, opt => opt.Ignore());
	}
}