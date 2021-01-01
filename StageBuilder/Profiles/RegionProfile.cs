using AutoMapper;
using StageBuilder.Models;
using StageBuilder.Dtos;

namespace StageBuilder.Profiles
{
  public class RegionProfile : Profile
  {
    public RegionProfile()
    {
      this.CreateMap<RegionEntity, Region>().ReverseMap()
          .ForMember(m => m.Id, opt => opt.Ignore());
    }
  }
}
