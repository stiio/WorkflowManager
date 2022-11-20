using System.ComponentModel.DataAnnotations;

namespace Stio.WorkflowManager.DemoApi.Models;

public class SecondBlockQuestion3Data
{
    [Required]
    public string[] Checked { get; set; } = null!;
}