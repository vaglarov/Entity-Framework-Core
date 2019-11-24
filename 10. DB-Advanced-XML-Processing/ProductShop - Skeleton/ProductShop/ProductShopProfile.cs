using AutoMapper;
using ProductShop.Dtos.Import;
using ProductShop.Dtos.Export;
using ProductShop.Models;
using System.Linq;
using System.Collections.Generic;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUsersDto, User>();
            this.CreateMap<ImportProductsDto, Product>();
            this.CreateMap<ImportCategoriesDto, Category>();
            this.CreateMap<ImportMappingCategoriesProduct, CategoryProduct>();

            this.CreateMap<Product, ExportProductInRangeDto>()
                .ForMember(x=>x.BuyerName,y=>y.MapFrom(x=>x.Buyer.FirstName +" " +x.Buyer.LastName));

            this.CreateMap<User, ExportSoldProductDto>()
                 .ForMember(x => x.FirstName, y => y.MapFrom(x => x.FirstName))
                 .ForMember(x => x.LastName, y => y.MapFrom(x => x.LastName))
                 .ForMember(x => x.ProductsSold, y => y.MapFrom(x => x.ProductsSold.Select(p => new { p.Name, p.Price })));

            this.CreateMap<User, ExportSoldProductSecondDto>()
               .ForMember(x => x.Products,
                   y => y.MapFrom(s => Mapper.Map<List<ExportSoldProductSecondDto>>(s.ProductsSold)));

            this.CreateMap<User, ExportUsersWithProductsDto>()
            .ForMember(x => x.Count, y => y.MapFrom(x => x.ProductsSold.Count));
            

        }
    }
}
