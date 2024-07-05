
namespace BL.Models
{
	public class PaginatedResponse<ProjectDto>
	{
		public IEnumerable<ProjectDto> Items { get; set; }
		public int TotalCount { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
	}
}
