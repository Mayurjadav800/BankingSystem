using AutoMapper;
using PaymentMicroServices.Dto;
using PaymentMicroServices.Model;

namespace PaymentMicroServices.Mapper
{
    public class MapperConfiguration:Profile
    {
        public MapperConfiguration()
        {
            CreateMap<Compund, CompundDto>().ReverseMap();
        }
    }
}
