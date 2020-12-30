using AutoMapper;
using StageBuilder.Models;
using StageBuilder.Dtos;

namespace StageBuilder.Profiles
{
  public class StageProfile : Profile
  {
    public StageProfile()
    {
      this.CreateMap<StageEntity, Stage>().ReverseMap()
          .ForMember(m => m.StageId, opt => opt.Ignore());
    }
  }
}
