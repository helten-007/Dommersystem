using System;
using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;
using NordicArenaDomainModels.Resources;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Areas.Public.ViewModels
{
	public class UserViewModel : _LayoutViewModel
    {
		public User User { get; set; }
		public List<Tournament> Tournaments { get; set; }

		public UserViewModel()
		{

		}
    }
}
