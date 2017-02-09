﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlpkWrapperCS {
	using org.gnu.glpk;
	enum ObjectDirection { Minimize = 1, Maximize = 2 }
	enum BoundsType { Free = 1, Lower = 2, Upper = 3, Double = 4, Fixed = 5 }
	enum SolverResult {  }
	class MipProblem : IDisposable {
		glp_prob problem = GLPK.glp_create_prob();
		// コンストラクタ
		public MipProblem() {
			// 目的関数関係
			ObjCoef = new objCoef(this);
			// 制約式関係
			RowName = new rowName(this);
			RowType = new rowType(this);
			RowLB = new rowLb(this);
			RowUB = new rowUb(this);
			// 変数関係
			ColumnName = new columnName(this);
			ColumnType = new columnType(this);
			ColumnLB = new columnLb(this);
			ColumnUB = new columnUb(this);
			LpColumnValue = new lpColumnValue(this);
		}
		// Disposeメソッド
		public void Dispose() { }
		// 問題名
		public string Name {
			get { return GLPK.glp_get_prob_name(problem); }
			set { GLPK.glp_set_prob_name(problem, value); }
		}
		// 最適化処理
		public void Simplex() {
			GLPK.glp_simplex(problem, null);
		}
		#region 目的関数関係
		// 最適化の方向
		public ObjectDirection ObjDir {
			get { return (ObjectDirection)GLPK.glp_get_obj_dir(problem); }
			set { GLPK.glp_set_obj_dir(problem, (int)value); }
		}
		// 目的関数の係数
		public class objCoef {
			MipProblem mip;
			public objCoef(MipProblem mip) {
				this.mip = mip;
			}
			public double this[int index] {
				get { return GLPK.glp_get_obj_coef(mip.problem, index + 1); }
				set { GLPK.glp_set_obj_coef(mip.problem, index + 1, (int)value); }
			}
		}
		public objCoef ObjCoef;
		// 最適化後の値
		public double LpObjVal {
			get { return GLPK.glp_get_obj_val(problem); }
		}
		#endregion
		#region 制約式関係
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
		// 制約式の係数
		public void LoadMatrix(int size, int[] ia, int[] ja, double[] ar) {
			var ia_ = GLPK.new_intArray(size + 1);
			var ja_ = GLPK.new_intArray(size + 1);
			var ar_ = GLPK.new_doubleArray(size + 1);
			for(int i = 0; i < size; ++i) {
				GLPK.intArray_setitem(ia_, i + 1, ia[i] + 1);
				GLPK.intArray_setitem(ja_, i + 1, ja[i] + 1);
				GLPK.doubleArray_setitem(ar_, i + 1, ar[i]);
			}
			GLPK.glp_load_matrix(problem, size, ia_, ja_, ar_);
			GLPK.delete_intArray(ia_);
			GLPK.delete_intArray(ja_);
			GLPK.delete_doubleArray(ar_);
		}
		public void LoadMatrix(int[] ia, int[] ja, double[] ar) {
			LoadMatrix(ia.Count(), ia, ja, ar);
		}
		#endregion
		#region 変数関係
		// 変数を追加する
		public void AddColumns(int n) {
			GLPK.glp_add_cols(problem, n);
		}
		// 変数の数
		public int ColumnsCount {
			get { return GLPK.glp_get_num_cols(problem); }
		}
		// 変数の名前
		public class columnName {
			MipProblem mip;
			public columnName(MipProblem mip) {
				this.mip = mip;
			}
			public string this[int index] {
				get { return GLPK.glp_get_col_name(mip.problem, index + 1); }
				set { GLPK.glp_set_col_name(mip.problem, index + 1, value); }
			}
		}
		public columnName ColumnName;
		// 変数の範囲を設定する
		public void SetColumnBounds(int index, BoundsType type, double lowerBound, double upperBound) {
			GLPK.glp_set_col_bnds(problem, index + 1, (int)type, lowerBound, upperBound);
		}
		// 変数の種類
		public class columnType {
			MipProblem mip;
			public columnType(MipProblem mip) {
				this.mip = mip;
			}
			public BoundsType this[int index] {
				get { return (BoundsType)GLPK.glp_get_col_type(mip.problem, index + 1); }
			}
		}
		public columnType ColumnType;
		// 変数の下限
		public class columnLb {
			MipProblem mip;
			public columnLb(MipProblem mip) {
				this.mip = mip;
			}
			public double this[int index] {
				get { return GLPK.glp_get_col_lb(mip.problem, index + 1); }
			}
		}
		public columnLb ColumnLB;
		// 変数の上限
		public class columnUb {
			MipProblem mip;
			public columnUb(MipProblem mip) {
				this.mip = mip;
			}
			public double this[int index] {
				get { return GLPK.glp_get_col_ub(mip.problem, index + 1); }
			}
		}
		public columnUb ColumnUB;
		// 変数の値
		public class lpColumnValue {
			MipProblem mip;
			public lpColumnValue(MipProblem mip) {
				this.mip = mip;
			}
			public double this[int index] {
				get { return GLPK.glp_get_col_prim(mip.problem, index + 1); }
			}
		}
		public lpColumnValue LpColumnValue;
		#endregion
	}
}
