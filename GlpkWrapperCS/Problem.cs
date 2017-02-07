using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlpkWrapperCS {
	using org.gnu.glpk;
	// 定数定義
	public static class ObjectDirection {
		// 目的関数の方向
		public static int Minimize = GLPK.GLP_MIN;
		public static int Maximize = GLPK.GLP_MAX;
	}
	// 問題クラス
	public class Problem : glp_prob {
		glp_prob problem = GLPK.glp_create_prob();
		// 目的関数の方向
		public int ObjectDirection {
			get { return GLPK.glp_get_obj_dir(problem); }
			set { GLPK.glp_set_obj_dir(problem, value); }
		}
		// 変数の数
		public int Column {
			get { return GLPK.glp_get_num_cols(problem); }
			set { GLPK.glp_add_cols(problem, value); }
		}
		// 制約式の数
		public int Row {
			get { return GLPK.glp_get_num_rows(problem); }
			set { GLPK.glp_add_rows(problem, value); }
		}
	}
}
