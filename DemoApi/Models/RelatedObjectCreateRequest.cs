using System.ComponentModel.DataAnnotations;

namespace Stio.WorkflowManager.DemoApi.Models;

public class RelatedObjectCreateRequest
{
    [Required]
    public Guid WorkflowId { get; set; }

    [Required]
    public string Name { get; set; } = null!;
}