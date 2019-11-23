using System.Linq;
using AutoMapper;
using CarDealer.DTO.Export;
using CarDealer.DTO.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<CarDto, Car>();

            this.CreateMap<Car, CarDto>();

            this.CreateMap<Part, PartDto>()
                .ForMember(x => x.Price, y => y.MapFrom(s => s.Price.ToString("F2")));

            this.CreateMap<Sale, SaleDto>()
                .ForMember(x => x.Car, y => y.MapFrom(s => Mapper.Map<CarDto>(s.Car)))
                .ForMember(x => x.CustomerName, y => y.MapFrom(s => s.Customer.Name))
                .ForMember(x => x.Discount, y => y.MapFrom(s => s.Discount.ToString("F2")))
                .ForMember(x => x.Price, y => y.MapFrom(s => (s.Car.PartCars.Sum(pc => pc.Part.Price)).ToString("F2")))
                .ForMember(x => x.PriceWithDiscount, y => y.MapFrom(s =>
                    ((s.Car.PartCars.Sum(pc => pc.Part.Price)) -
                     ((s.Car.PartCars.Sum(pc => pc.Part.Price)) * s.Discount / 100))
                    .ToString("F2")));
        }
    }
}