using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Core.Behavior
{
	class BehaviorTree
	{
		private TreeNode Node { get; set; }

		class TreeNode
		{
			public IBehavior Behavior { get; set; }
			public HashSet<TreeNode> ChildNodes { get; set; }			
		}
	}
}