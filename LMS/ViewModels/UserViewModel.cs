using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.ViewModels
{
	public class UserViewModel
	{
		public int id { get; set; }
		public string role_id { get; set; }
		public string fname { get; set; }
		public string lname { get; set; }
		public string email { get; set; }
		public string phone { get; set; }
		public string uname { get; set; }
		public string oldpassword { get; set; }
		public string password1 { get; set; }
		public string password2 { get; set; }
	}
}