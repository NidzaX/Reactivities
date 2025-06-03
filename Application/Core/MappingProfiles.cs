using Application.Activities.DTOs;
using Application.Profiles.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        string? currentUserId = null;
        CreateMap<Activity, Activity>();
        CreateMap<CreateActivityDto, Activity>();
        CreateMap<EditActivityDto, Activity>();
        CreateMap<Activity, ActivityDto>()
            .AfterMap((src, dest) =>
            {
                var host = src.Attendees.FirstOrDefault(x => x.IsHost);
                dest.HostDisplayName = host?.User?.DisplayName ?? "";
                dest.HostId = host?.User?.Id ?? "";
            });
        CreateMap<ActivityAttendee, UserProfile>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User != null ? s.User.DisplayName : ""))
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.User != null ? s.User.Bio : ""))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User != null ? s.User.ImageUrl : ""))
            .ForMember(d => d.Id, o => o.MapFrom(s => s.User != null ? s.User.Id : ""))
            .ForMember(d => d.FollowerCount, o => o.MapFrom(s => s.User.Followers.Count))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.User.Followings.Count))
            .ForMember(d => d.Following, o => o.MapFrom(s =>
                s.User.Followers.Any(x => x.Observer.Id == currentUserId)));
        CreateMap<User, UserProfile>()
            .ForMember(d => d.FollowerCount, o => o.MapFrom(s => s.Followers.Count))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
            .ForMember(d => d.Following, o => o.MapFrom(s =>
                s.Followers.Any(x => x.Observer.Id == currentUserId)));
        CreateMap<Comment, CommentDto>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl));
    }
}