#nullable disable
using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int TypeId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int? ImageId { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual Image Image { get; set; }

    public virtual ICollection<ProjectSkill> ProjectSkills { get; set; } = new List<ProjectSkill>();

    public virtual Type Type { get; set; }
}