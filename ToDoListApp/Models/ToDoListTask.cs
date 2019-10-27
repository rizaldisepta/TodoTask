using System;
using System.ComponentModel.DataAnnotations;


namespace ToDoListApp.Models
{
    public class ToDoListTask
    {
        public Guid Id { get; set; }

        public bool IsDone { get; set; }

        [Required]
        public string Title { get; set; }

        public string Details { get; set; }

        public DateTime DateModified { get; set; }

        public string UserId { get; set; }
    }
}