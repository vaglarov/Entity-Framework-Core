using CarDealer.Models;
using CarDealer.Dtos.Import;
using CarDealer.Dtos.Export;

using AutoMapper;
using System.Linq;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSupplierDto, Supplier>();
            this.CreateMap<ImportPartDto, Part>();

            this.CreateMap<ImportCustomerDto, Customer>();
            this.CreateMap<ImportSaleDto, Sale>();

            this.CreateMap<Supplier, ExportLocalSupplierDto>();
            this.CreateMap<Car, ExportCarsWithDistanceDto>();
            this.CreateMap<Car, ExportCarsWithMakerBMWDto>();

            //CArWith their parts
            this.CreateMap<Part, ExportPartNameAndPriceDto>();
            this.CreateMap<Car, ExportCarWithPartsDto>()
                .ForMember(x => x.Parts, y => y.MapFrom(x => x.PartCars.Select(pc => pc.Part).OrderByDescending(pc => pc.Price)));

            this.CreateMap<Customer, ExportTotalSalecByCustomerDto>()
                .ForMember(x => x.Name, y => y.MapFrom(x => x.Name))
                .ForMember(x => x.BoughtCars, y => y.MapFrom(x => x.Sales.Count))
                .ForMember(x => x.SpentMoney, y => y.MapFrom(s => s.Sales.Sum(c => c.Car.PartCars.Sum(p => p.Part.Price))));

            this.CreateMap<Sale, ExportSalesWithAppliedDto>()
                .ForMember(x => x.Car, y => y.MapFrom(x => x.Car))
                .ForMember(x => x.CustomerName, y => y.MapFrom(x => x.Customer.Name))
                .ForMember(x => x.Price, y => y.MapFrom(s => s.Car.PartCars.Sum(p => p.Part.Price)))
                .ForMember(x => x.PriceDiscount, y => y.MapFrom(s => ( s.Car.PartCars.Sum(p => p.Part.Price) - s.Car.PartCars.Sum(p => p.Part.Price) * s.Discount / (decimal)100)));
        }
    }
}
