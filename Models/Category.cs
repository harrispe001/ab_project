using System.Collections.Generic;

namespace ab_project.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Training> Trainings { get; set; }
    }
}
