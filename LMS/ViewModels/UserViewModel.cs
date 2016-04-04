using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.ViewModels
{
	public class UserViewModel
	{
		public int id { get; set; }
		public string role_id { get; set; }
		[Required]
		public string role_name { get; set; }
		[Required]
		public string fname { get; set; }
		[Required]
		public string lname { get; set; }
		[Required]
		public string email { get; set; }
		public string phone { get; set; }
		[Required]
		public string uname { get; set; }
		public string oldpassword { get; set; }
		[Required]
		public string password1 { get; set; }
		[Required]
		public string password2 { get; set; }
	}
}