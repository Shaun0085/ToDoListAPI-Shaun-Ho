using System.ComponentModel.DataAnnotations;

namespace ToDoListDomain{
    public class ToDoListItem{

        [Key]
        public int ItemId { get; set; }

        [Required]
        public string? ItemTitle { get; set; }

        public bool IsCompleted { get; set; }
    }
}
