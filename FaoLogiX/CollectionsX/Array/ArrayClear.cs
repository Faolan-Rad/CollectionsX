﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using CollectionsX.Objs;
namespace CollectionsX.Array
{
	[NodeName("Array Clear")]
	[Category(new string[] { "LogiX/Collections/Array" })]
	public class ArrayClear<T> : LogixNode, IChangeable, IWorldElement
	{
        public readonly Input<IList<T>> List;

		public readonly Impulse Cleared;

        [ImpulseTarget]
        public void Add()
        {
			IList<T> _listobj;
            _listobj = List.Evaluate();
            if (_listobj != null)
            {
                _listobj.Clear();
                this.Cleared.Trigger();
            }
        }

		protected override Type FindOverload(NodeTypes connectingTypes)
		{
			if (this.List.IsConnected)
			{
				return null;
			}
			Type overload;
			overload = LogixHelper.GetMatchingOverload(this.GetOverloadName(), connectingTypes);
			if (overload != null)
			{
				return overload;
			}
			if (connectingTypes.inputs.TryGetValue("List", out var type))
			{
				return typeof(ArrayClear<>).MakeGenericType(type.GetGenericArguments()[0]);
			}
			return null;
		}
		protected override void OnGenerateVisual(Slot root)
		{
			UIBuilder uIBuilder;
			uIBuilder = base.GenerateUI(root, 184f, 76f);
			VerticalLayout verticalLayout;
			verticalLayout = uIBuilder.VerticalLayout(4f);
			verticalLayout.PaddingLeft.Value = 8f;
			verticalLayout.PaddingRight.Value = 16f;
			uIBuilder.Style.MinHeight = 32f;
			LocaleString text;
			text = typeof(T).Name;
			uIBuilder.Text(in text);
		}
		protected override void NotifyOutputsOfChange()
		{
		}
	}

}
