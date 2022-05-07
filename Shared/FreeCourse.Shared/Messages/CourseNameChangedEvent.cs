namespace FreeCourse.Shared.Messages;

public class CourseNameChangedEvent
{
    public string CourseId { get; set; }
    public string UpdatedName { get; set; }
}