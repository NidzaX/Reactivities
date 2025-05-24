namespace Domain;

public class ActivityAttendee
{
    public  string? UserId { get; set; }
    public  User? User { get; set; }
    public  string? ActivityId { get; set; }
    public  Activity? Activity { get; set; }
    public bool IsHost { get; set; }
    public DateTime DateJoined { get; set; } = DateTime.UtcNow;

    public ActivityAttendee(
        string userId = "",
        User? user = null,
        string activityId = "",
        Activity? activity = null,
        bool isHost = false
    )
    {
        UserId = userId;
        User = user ?? throw new ArgumentNullException(nameof(user));
        ActivityId = activityId;
        Activity = activity ?? throw new ArgumentNullException(nameof(activity));
        IsHost = isHost;
    }

    protected ActivityAttendee() { }
}