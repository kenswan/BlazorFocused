using System.ComponentModel.DataAnnotations;

namespace Integration.Shared.Models
{
    public class ToDoItem
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public ToDoStatus Status { get; set; }
    }
}
