using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalog;

public class FeatureViewModel
{
    [Required]
    public int Duration { get; set; }
}