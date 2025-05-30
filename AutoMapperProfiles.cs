using AutoMapper;
using ShepherdsPie.Models;
using ShepherdsPie.Models.DTOs;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // CreateMap<mapfrom, mapto>();
        CreateMap<Order, OrderDTO>();
        CreateMap<Order, OrderDetailsDTO>();
        CreateMap<Pizza, PizzaDTO>();
    }
}