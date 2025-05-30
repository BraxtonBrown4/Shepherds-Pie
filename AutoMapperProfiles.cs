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
        CreateMap<CreateOrderDTO, Order>();
        CreateMap<CreateOrderDTO, PizzaDTO>();
        CreateMap<Pizza, PizzaDTO>();
        CreateMap<PizzaDTO, Pizza>();
        CreateMap<PizzaToppingDTO, PizzaTopping>();
        CreateMap<PizzaTopping, PizzaToppingDTO>();
        CreateMap<Topping, ToppingDTO>();
    }
}