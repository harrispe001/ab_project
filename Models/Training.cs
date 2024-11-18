using System.ComponentModel.DataAnnotations;

namespace ab_project.Models
{
public class Training
{
    public int TrainingId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Author { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public Category Category { get; set; }
}

}
