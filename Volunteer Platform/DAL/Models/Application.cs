#nullable disable
using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Application
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProjectId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Status { get; set; }

    public virtual Project Project { get; set; }

    public virtual User User { get; set; }
}