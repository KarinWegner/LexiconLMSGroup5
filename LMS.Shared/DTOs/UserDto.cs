using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs
{
	public class UserDto
	{
		public string Username { get; set; }
		public int UserId { get; set; }
		public string Course { get; set; }
		public DateTime Enrolled { get; set; }
		public string ProfilePictureLink { get; set; } = "https://tse4.mm.bing.net/th?id=OIP.4Q7-yMnrlnqwR4ORH7c06AHaHa&pid=Api&P=0&h=180";
	}
}
