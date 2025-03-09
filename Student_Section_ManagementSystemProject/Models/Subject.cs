using System.ComponentModel.DataAnnotations;

public class Subject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }
}
