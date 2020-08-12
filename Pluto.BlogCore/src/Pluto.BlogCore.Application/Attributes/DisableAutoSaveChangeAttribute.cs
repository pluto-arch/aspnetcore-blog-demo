﻿using System;

namespace Pluto.BlogCore.Application.Attributes
{
	/// <summary>
	/// 禁用自动SaveChange
	/// </summary>
	/// <remarks>如果禁用了，需要手动添加unitofwork的SaveChange</remarks>
	[AttributeUsage(AttributeTargets.Class)]
	public class DisableAutoSaveChangeAttribute: Attribute
	{
	}
}