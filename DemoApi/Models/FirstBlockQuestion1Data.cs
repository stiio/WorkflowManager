using System.ComponentModel.DataAnnotations;

namespace Stio.WorkflowManager.DemoApi.Models;

public class FirstBlockQuestion1Data
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    public bool Agree { get; set; }
}