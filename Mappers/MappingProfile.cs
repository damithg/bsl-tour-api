using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BSLTours.API.Models;

namespace BSLTours.API.Mappers
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Experience, ExperienceDto>()
                .ForMember(dest => dest.Location,
                    opt => opt.MapFrom<LocationNameResolver>())

                .ForMember(dest => dest.Highlights,
                    opt => opt.MapFrom(src => src.Highlights != null
                        ? src.Highlights.Select(h => h.Text).ToList()
                        : new List<string>()))

                .ForMember(dest => dest.Inclusions,
                    opt => opt.MapFrom(src => src.Inclusions != null
                        ? src.Inclusions.Select(i => i.Text).ToList()
                        : new List<string>()))

                .ForMember(dest => dest.WhatToBring,
                    opt => opt.MapFrom(src => src.WhatToBring != null
                        ? src.WhatToBring.Select(w => w.Text).ToList()
                        : new List<string>()))

                .ForMember(dest => dest.Seo,
                    opt => opt.MapFrom(src => src.Seo))

                // Use resolver for GalleryImageUrls
                .ForMember(dest => dest.GalleryImages,
                    opt => opt.MapFrom<GalleryImageUrlsResolver>());

            CreateMap<SeoMeta, SeoMetaDto>();
        }

        private static string GetLocationName(Experience src)
        {
            return src?.Location?.Data?.Attributes?.Name;
        }
    }

    public class LocationNameResolver : IValueResolver<Experience, ExperienceDto, string>
    {
        public string Resolve(Experience source, ExperienceDto destination, string destMember, ResolutionContext context)
        {
            return source?.Location?.Data?.Attributes?.Name;
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
}
