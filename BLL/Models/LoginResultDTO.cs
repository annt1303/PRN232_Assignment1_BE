﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
	public class LoginResultDTO
	{
		public string Token { get; set; } = string.Empty;
		public SystemAccountDTO Account { get; set; } = null!;
	}

}
