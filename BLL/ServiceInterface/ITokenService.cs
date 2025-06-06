﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterface
{
	public interface ITokenService
	{
		Task<string> GenerateToken(SystemAccount user);

		string GenerateRefreshToken();
	}
}
