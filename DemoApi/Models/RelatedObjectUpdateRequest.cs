using System.ComponentModel.DataAnnotations;

namespace Stio.WorkflowManager.DemoApi.Models;

public class RelatedObjectUpdateRequest
{
    [Required]
    public string Name { get; set; } = null!;
}