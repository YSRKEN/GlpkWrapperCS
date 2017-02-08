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
				problem.Column.Add(3);
				Console.WriteLine(problem.Column.Count);
				// 制約式の数を設定
				problem.Row.Add(3);
				Console.WriteLine(problem.Row.Count);
				// 変数の範囲を設定
				problem.Column[0] = new Bounds(BoundsType.Lower, 0.0, 0.0);

				// 制約式の範囲を設定

			}
		}
	}
}
