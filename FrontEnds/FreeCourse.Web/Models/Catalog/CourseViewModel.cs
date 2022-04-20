namespace FreeCourse.Web.Models.Catalog;

public class CourseViewModel
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string Picture { get; set; }
    
    public string UserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public FeatureViewModel Feature { get; set; }

    public string CategoryId { get; set; }

    public CategoryViewModel Category { get; set; }
}