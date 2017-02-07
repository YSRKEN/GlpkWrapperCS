using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlpkSample {
	using GlpkWrapperCS;
	class Program {
		static void Main(string[] args) {
			using(var problem = new Problem()) {
				// 目的関数の方向を設定
				problem.ObjectDirection = ObjectDirection.Maximize;
				// 変数の数を設定
				problem.Column = 3;
				// 制約式の数を設定
				problem.Row = 3;

			}
		}
	}
}
