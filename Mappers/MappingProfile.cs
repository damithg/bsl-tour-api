using AutoMapper;
using BSLTours.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BSLTours.API.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Experience, ExperienceDto>()
                .ForMember(dest => dest.Highlights, opt => opt.MapFrom<HighlightResolver>())
                .ForMember(dest => dest.Inclusions, opt => opt.MapFrom<InclusionResolver>())
                .ForMember(dest => dest.WhatToBring, opt => opt.MapFrom<WhatToBringResolver>())
                .ForMember(dest => dest.GalleryImages,
                    opt => opt.MapFrom(src => src.GalleryImages ?? new List<GalleryImage>()))
                .ForMember(dest => dest.RelatedExperiences,
                    opt => opt.MapFrom(src => src.RelatedExperiences ?? new List<Experience>()))
                .ForMember(dest => dest.Location,
                    opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Seo,
                    opt => opt.MapFrom(src => src.Seo))
                .ForMember(dest => dest.Card,
                    opt => opt.MapFrom(src => src.Card));

            CreateMap<Experience, RelatedExperienceDto>()
                //.ForMember(dest => dest.Highlights,
                //    opt => opt.MapFrom<HighlightResolverForRelated>())
                .ForMember(dest => dest.GalleryImages,
                    opt => opt.MapFrom(src => src.GalleryImages ?? new List<GalleryImage>()))
                .ForMember(dest => dest.Seo,
                    opt => opt.MapFrom(src => src.Seo))
                .ForMember(dest => dest.Card,
                    opt => opt.MapFrom(src => src.Card));

            CreateMap<SeoMeta, SeoMetaDto>();

            CreateMap<GalleryImage, GalleryImageDto>();
            CreateMap<Location, LocationDto>();

            CreateMap<CardImage, CardImageDto>();

            CreateMap<CardData, CardDto>()
                .ForMember(dest => dest.Image,
                    opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Tags,
                    opt => opt.MapFrom(src => src.Tags.ToList() ?? new List<string>()));

        }
    }

    public class HighlightResolver : IValueResolver<Experience, ExperienceDto, List<string>>
    {
        public List<string> Resolve(Experience source, ExperienceDto dest, List<string> member, ResolutionContext context)
        {
            return source.Highlights?.Select(x => x.Text).ToList() ?? new List<string>();
        }
    }

    public class TextItemResolver<TDestination> : IValueResolver<Experience, TDestination, List<string>>
    {
        private readonly Func<Experience, List<TextItem>> _selector;

        public TextItemResolver(Func<Experience, List<TextItem>> selector)
        {
            _selector = selector;
        }

        public List<string> Resolve(Experience source, TDestination destination, List<string> destMember, ResolutionContext context)
        {
            return _selector(source)?.Select(x => x.Text).ToList() ?? new List<string>();
        }
    }

    public class CardHeaderResolver : IValueResolver<Experience, ExperienceDto, string>
    {
        public string Resolve(Experience source, ExperienceDto destination, string destMember, ResolutionContext context)
        {
            return source?.Card?.Header;
        }
    }

    public class GalleryImageUrlsResolver : IValueResolver<Experience, ExperienceDto, List<GalleryImageDto>>
    {
        public List<GalleryImageDto> Resolve(Experience source, ExperienceDto destination, List<GalleryImageDto> destMember, ResolutionContext context)
        {
            if (source?.GalleryImages == null)
                return new List<GalleryImageDto>();

            return source.GalleryImages.Select(img => new GalleryImageDto
            {
                PublicId = img.PublicId,
                Alt = img.Alt,
                Caption = img.Caption
            }).ToList();
        }
    }


    public class CardImageUrlResolver : IValueResolver<Experience, ExperienceDto, string>
    {
        public string Resolve(Experience source, ExperienceDto destination, string destMember, ResolutionContext context)
        {
            return source?.Card?.Image?.PublicId;
        }
    }

    //public class HighlightResolver : IValueResolver<Experience, ExperienceDto, List<string>>
    //{
    //    public List<string> Resolve(Experience source, ExperienceDto destination, List<string> destMember, ResolutionContext context)
    //    {
    //        return source.Highlights?.Select(h => h.Text).ToList() ?? new List<string>();
    //    }
    //}

    public class InclusionResolver : IValueResolver<Experience, ExperienceDto, List<string>>
    {
        public List<string> Resolve(Experience source, ExperienceDto destination, List<string> destMember, ResolutionContext context)
        {
            return source.Inclusions?.Select(i => i.Text).ToList() ?? new List<string>();
        }
    }

    public class WhatToBringResolver : IValueResolver<Experience, ExperienceDto, List<string>>
    {
        public List<string> Resolve(Experience source, ExperienceDto destination, List<string> destMember, ResolutionContext context)
        {
            return source.WhatToBring?.Select(w => w.Text).ToList() ?? new List<string>();
        }
    }

    public class HighlightResolverForRelated : IValueResolver<Experience, RelatedExperienceDto, List<string>>
    {
        public List<string> Resolve(Experience source, RelatedExperienceDto destination, List<string> destMember, ResolutionContext context)
        {
            return source.Highlights?.Select(h => h.Text).ToList() ?? new List<string>();
        }
    }


}
