using System;
namespace BlazorFocused.Integration.Client.Models
{
    public class ToDoItem
    {
        public string Title { get; set; }

        public string Descriptions { get; set; }

        public ToDoStatus Status { get; set; }
    }
}
