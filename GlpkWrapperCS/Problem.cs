using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlpkWrapperCS {
	using org.gnu.glpk;
	#region 定数定義
	public static class ObjectDirection {
		/// <summary>
		/// 目的関数の方向
		/// </summary>
		public static int Minimize = GLPK.GLP_MIN; //最小化
		public static int Maximize = GLPK.GLP_MAX; //最大化
	}
	public static class BoundsType {
		/// <summary>
		/// 境界の種類
		/// </summary>
		public static int Free   = GLPK.GLP_FR; //(-∞, ∞)
		public static int Lower  = GLPK.GLP_LO; //(LB, ∞)
		public static int Upper  = GLPK.GLP_UP; //(-∞, UB)
		public static int Double = GLPK.GLP_DB; //(LB,UB)
		public static int Fixed  = GLPK.GLP_FX; //LB=UB
	}
	#endregion
	/// <summary>
	/// 境界クラス
	/// </summary>
	public class Bounds {
		public int Type { get; set; } //BoundsType型
		public double LowerBound { get; set; }
		public double UpperBound { get; set; }
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">境界の種類</param>
		/// <param name="lowerBound">下限</param>
		/// <param name="upperBound">上限</param>
		public Bounds(int type, double lowerBound, double upperBound) {
			Type = type;
			LowerBound = lowerBound;
			UpperBound = upperBound;
		}
	}
	/// <summary>
	/// 変数についてのクラス
	/// </summary>
	class column {
		glp_prob problem;
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="problem">対象とする問題クラス</param>
		public column(glp_prob problem) {
			this.problem = problem;
		}
		/// <summary>
		/// 変数の数を返す
		/// </summary>
		public int Count {
			get { return GLPK.glp_get_num_cols(problem); }
		}
		/// <summary>
		/// 変数を追加する
		/// </summary>
		/// <param name="n">追加する変数の数</param>
		public void Add(int n) {
			GLPK.glp_add_cols(problem, n);
		}
		/// <summary>
		/// 変数を追加する
		/// </summary>
		/// <param name="type">境界の種類(BoundsTypeクラス)</param>
		/// <param name="lowerBound">下限</param>
		/// <param name="upperBound">上限</param>
		public void Add(int type, double lowerBound, double upperBound) {
			GLPK.glp_add_cols(problem, 1);
			GLPK.glp_set_col_bnds(problem, Count, type, lowerBound, upperBound);
		}
		/// <summary>
		/// 境界条件をインデックスアクセスする
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Bounds this[int index] {
			get {
				return new Bounds(
					GLPK.glp_get_col_type(problem, index + 1),
					GLPK.glp_get_col_lb(problem, index + 1),
					GLPK.glp_get_col_ub(problem, index + 1)
				);
			}
			set {
				GLPK.glp_set_col_bnds(problem, index + 1, value.Type, value.LowerBound, value.UpperBound);
			}
		}
	}
	/// <summary>
	/// 制約式についてのクラス
	/// </summary>
	class row {
		glp_prob problem;
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="problem">対象とする問題クラス</param>
		public row(glp_prob problem) {
			this.problem = problem;
		}
		/// <summary>
		/// 制約式の数を返す
		/// </summary>
		public int Count {
			get { return GLPK.glp_get_num_rows(problem); }
		}
		/// <summary>
		/// 制約式を追加する
		/// </summary>
		/// <param name="n">追加する制約式の数</param>
		public void Add(int n) {
			GLPK.glp_add_rows(problem, n);
		}
		/// <summary>
		/// 制約式を追加する
		/// </summary>
		/// <param name="type">境界の種類(BoundsTypeクラス)</param>
		/// <param name="lowerBound">下限</param>
		/// <param name="upperBound">上限</param>
		public void Add(int type, double lowerBound, double upperBound) {
			GLPK.glp_add_rows(problem, 1);
			GLPK.glp_set_row_bnds(problem, Count, type, lowerBound, upperBound);
		}
		/// <summary>
		/// 境界条件をインデックスアクセスする
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Bounds this[int index] {
			get {
				return new Bounds(
					GLPK.glp_get_row_type(problem, index + 1),
					GLPK.glp_get_row_lb(problem, index + 1),
					GLPK.glp_get_row_ub(problem, index + 1)
				);
			}
			set {
				GLPK.glp_set_row_bnds(problem, index + 1, value.Type, value.LowerBound, value.UpperBound);
			}
		}
	}
	/// <summary>
	/// 問題クラス
	/// </summary>
	class Problem : glp_prob {
		glp_prob problem;
		public column Column;
		public row Row;
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Problem() {
			problem = GLPK.glp_create_prob();
			Column = new column(problem);
			Row = new row(problem);
		}
		/// <summary>
		/// デスストラクタ
		/// </summary>
		~Problem() {
			Dispose();
		}
		/// <summary>
		/// 目的関数の方向
		/// </summary>
		public int ObjectDirection {
			get { return GLPK.glp_get_obj_dir(problem); }
			set { GLPK.glp_set_obj_dir(problem, value); }
		}
	}
}
