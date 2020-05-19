﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeaverCore.Helpers;
using WeaverCore.Interfaces;

namespace WeaverCore.Awaiters
{
	public class WaitForSeconds : IUAwaiter
	{
		float time;
		float elapsed = 0;

		public WaitForSeconds(float time)
		{
			this.time = time;
		}

		public bool KeepWaiting()
		{
			elapsed += URoutine.DT;
			return elapsed < time;
		}
	}
}
