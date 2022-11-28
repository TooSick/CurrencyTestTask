using AutoMapper;
using Currency.BLL.Models;

namespace Currency.BLL.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Models.Currency, CurrencyViewModel>();
        }
    }
}
