﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeaverCore.Interfaces
{
	public interface IHittable
	{
		void Hit(HitInfo hit);
	}
}
