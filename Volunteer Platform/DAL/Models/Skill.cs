#nullable disable
using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Skill
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<ProjectSkill> ProjectSkills { get; set; } = new List<ProjectSkill>();
}