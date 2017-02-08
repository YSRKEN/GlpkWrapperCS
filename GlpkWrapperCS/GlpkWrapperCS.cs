using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlpkWrapperCS {
	using org.gnu.glpk;
	enum ObjectDirection { Minimize = 1, Maximize = 2 }
	enum BoundsType { Free, Lower, Upper, Double, Fixed }
	class MipProblem : IDisposable {
		glp_prob problem = GLPK.glp_create_prob();
		// コンストラクタ
		public MipProblem() {
			RowName = new rowName(this);
			RowType = new rowType(this);
			RowLB = new rowLb(this);
			RowUB = new rowUb(this);
		}
		// Disposeメソッド
		public void Dispose() { }
		// 問題名
		public string Name {
			get { return GLPK.glp_get_prob_name(problem); }
			set { GLPK.glp_set_prob_name(problem, value); }
		}
		// 最適化の方向
		public ObjectDirection ObjDir {
			get { return (ObjectDirection)GLPK.glp_get_obj_dir(problem); }
			set { GLPK.glp_set_obj_dir(problem, (int)value); }
		}
		// 制約式を追加する
		public void AddRows(int n) {
			GLPK.glp_add_rows(problem, n);
		}
		// 制約式の数
		public int RowsCount {
			get { return GLPK.glp_get_num_rows(problem); }
		}
		// 制約式の名前
		public class rowName {
			MipProblem mip;
			public rowName(MipProblem mip) {
				this.mip = mip;
			}
			public string this[int index] {
				get { return GLPK.glp_get_row_name(mip.problem, index + 1); }
				set { GLPK.glp_set_row_name(mip.problem, index + 1, value); }
			}
		}
		public rowName RowName;
		// 制約式の範囲を設定する
		public void SetRowBounds(int index, BoundsType type, double lowerBound, double upperBound) {
			GLPK.glp_set_row_bnds(problem, index + 1, (int)type, lowerBound, upperBound);
		}
		// 制約式の種類
		public class rowType {
			MipProblem mip;
			public rowType(MipProblem mip) {
				this.mip = mip;
			}
			public BoundsType this[int index] {
				get { return (BoundsType)GLPK.glp_get_row_type(mip.problem, index + 1); }
			}
		}
		public rowType RowType;
		// 制約式の下限
		public class rowLb {
			MipProblem mip;
			public rowLb(MipProblem mip) {
				this.mip = mip;
			}
			public double this[int index] {
				get { return GLPK.glp_get_row_lb(mip.problem, index + 1); }
			}
		}
		public rowLb RowLB;
		// 制約式の上限
		public class rowUb {
			MipProblem mip;
			public rowUb(MipProblem mip) {
				this.mip = mip;
			}
			public double this[int index] {
				get { return GLPK.glp_get_row_ub(mip.problem, index + 1); }
			}
		}
		public rowUb RowUB;

	}
}
