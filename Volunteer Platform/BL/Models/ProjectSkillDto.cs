using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BL.Models;

public class ProjectSkillDto
{
    [DisplayName("ProjectSkill Id")]
    public int? Id { get; set; }
    [Required]
    [DisplayName("Project Id")]
    public int ProjectId { get; set; }
    [Required]
    [DisplayName("Skill Id")]
    public int SkillId { get; set; }
    [DisplayName("Project Name")]
	public string? ProjectName { get; set; }
	[DisplayName("Project Name")]
	public string? SkillName { get; set; }


}