using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence;

public class DbInitializer
{
    private static ActivityAttendee CreateAttendee(User user, bool isHost = false)
    {
        return new ActivityAttendee
        {
            UserId = user.Id,
            User = user,
            IsHost = isHost
        };
    }


    public static async Task SeedData(AppDbContext context, UserManager<User> userManager)
    {
        var users = new List<User>
    { 
            new() { Id = "bob-id", DisplayName = "Bob", UserName = "bob@test.com", Email = "bob@test.com" },
            new() { Id = "tom-id", DisplayName = "Tom", UserName = "tom@test.com", Email = "tom@test.com" },
            new() { Id = "jane-id", DisplayName = "Jane", UserName = "jane@test.com", Email = "jane@test.com" }
        };

        if (!userManager.Users.Any())
        {
            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }

        var bob = await userManager.FindByEmailAsync("bob@test.com");
        var tom = await userManager.FindByEmailAsync("tom@test.com");
        var jane = await userManager.FindByEmailAsync("jane@test.com");

        if (bob == null || tom == null || jane == null)
        {
            throw new Exception("Required seed users were not created properly.");
        }

        if (context.Activities.Any()) return;

        var activities = new List<(Activity activity, (User user, bool isHost)[] attendees)>
        {
            (new Activity { Title = "Past Activity 1", Date = DateTime.Now.AddMonths(-2), Description = "Activity 2 months ago", Category = "drinks", City = "London", Venue = "The Lamb and Flag, 33, Rose Street...", Latitude = 51.51171665, Longitude = -0.1256611 }, new[] { (bob, true), (tom, false) }),
            (new Activity { Title = "Past Activity 2", Date = DateTime.Now.AddMonths(-1), Description = "Activity 1 month ago", Category = "culture", City = "Paris", Venue = "Louvre Museum, Rue Saint-Honoré...", Latitude = 48.8611473, Longitude = 2.3380276 }, new[] { (tom, true), (jane, false), (bob, false) }),
            (new Activity { Title = "Future Activity 1", Date = DateTime.Now.AddMonths(1), Description = "Activity 1 month in future", Category = "culture", City = "London", Venue = "Natural History Museum", Latitude = 51.4965109, Longitude = -0.1760019 }, new[] { (jane, true) }),
            (new Activity { Title = "Future Activity 2", Date = DateTime.Now.AddMonths(2), Description = "Activity 2 months in future", Category = "music", City = "London", Venue = "The O2", Latitude = 51.5029366, Longitude = 0.0032029 }, new[] { (bob, true), (jane, false) }),
            (new Activity { Title = "Future Activity 3", Date = DateTime.Now.AddMonths(3), Description = "Activity 3 months in future", Category = "drinks", City = "London", Venue = "The Mayflower", Latitude = 51.501778, Longitude = -0.053577 }, new[] { (tom, false) }),
            (new Activity { Title = "Future Activity 4", Date = DateTime.Now.AddMonths(4), Description = "Activity 4 months in future", Category = "drinks", City = "London", Venue = "The Blackfriar", Latitude = 51.5121466, Longitude = -0.1036468 }, new[] { (jane, true), (bob, false) }),
            (new Activity { Title = "Future Activity 5", Date = DateTime.Now.AddMonths(5), Description = "Activity 5 months in future", Category = "culture", City = "London", Venue = "Sherlock Holmes Museum...", Latitude = 51.5237629, Longitude = -0.1584743 }, new[] { (bob, true) }),
            (new Activity { Title = "Future Activity 6", Date = DateTime.Now.AddMonths(6), Description = "Activity 6 months in future", Category = "music", City = "London", Venue = "Roundhouse, Chalk Farm Road...", Latitude = 51.5432505, Longitude = -0.1519761 }, new[] { (tom, true), (bob, false) }),
            (new Activity { Title = "Future Activity 7", Date = DateTime.Now.AddMonths(7), Description = "Activity 7 months in future", Category = "travel", City = "London", Venue = "River Thames, England...", Latitude = 51.5575525, Longitude = -0.781404 }, new[] { (jane, true), (tom, false) }),
            (new Activity { Title = "Future Activity 8", Date = DateTime.Now.AddMonths(8), Description = "Activity 8 months in future", Category = "film", City = "London", Venue = "Odeon Leicester Square", Latitude = 51.5575525, Longitude = -0.781404 }, new[] { (bob, true) })
        };

        foreach (var (activity, attendeeList) in activities)
        {
            context.Activities.Add(activity);
            await context.SaveChangesAsync();

            activity.Attendees = attendeeList.Select(a =>
                new ActivityAttendee(
                    userId: a.user.Id,
                    user: a.user,
                    activityId: activity.Id,
                    activity: activity,
                    isHost: a.isHost
                )
            ).ToList();

            context.Update(activity);
        }

        await context.SaveChangesAsync();
    }

}