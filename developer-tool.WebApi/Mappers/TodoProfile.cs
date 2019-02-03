using AutoMapper;
using Infrastructure.Models;
using Domain.Models;
using WebApi.ViewModels;
using WebApi.InputModels;

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