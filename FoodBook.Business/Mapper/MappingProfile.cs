using AutoMapper;
using FoodBook.Common.DTOs.UserDTOs;
using FoodBook.DataAccess.Models;

namespace FoodBook.Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}