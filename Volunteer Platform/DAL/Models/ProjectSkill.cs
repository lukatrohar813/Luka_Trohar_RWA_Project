#nullable disable


namespace DAL.Models;

public partial class ProjectSkill
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int SkillId { get; set; }

    public virtual Project Project { get; set; }

    public virtual Skill Skill { get; set; }
}