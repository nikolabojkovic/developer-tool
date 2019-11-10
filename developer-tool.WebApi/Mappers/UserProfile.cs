using AutoMapper;
using WebApi.InputModels;
using Application.Authentication.Commands;
using Domain.PersistenceModels;
using Domain.Models;

public class UserProfile : Profile
{
	public UserProfile()
	{
		CreateMap<RegisterInputModel, RegisterCommand>();
		CreateMap<User, UserModel>()
			.ForMember(x => x.Id, opt => opt.Ignore());
	}
}