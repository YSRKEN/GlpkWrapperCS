using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlpkWrapperCS;

namespace GlpkSample {
	using org.gnu.glpk;
	class Program {
		static void Main(string[] args) {
			Console.WriteLine($"{GLPK.GLP_CV} {GLPK.GLP_IV} {GLPK.GLP_BV}");
			// GLPK for C#/CLIをそのまま使った場合
			Func1();
			Console.WriteLine("");
			// GlpkWrapperCSを使った場合 その1
			Func2();
			Console.WriteLine("");
			// GlpkWrapperCSを使った場合 その2
			Func3();
		}
		// GLPK for C#/CLIをそのまま使った場合
		static void Func1() {
			Console.WriteLine("【GLPK for C#/CLI】");
			using(var problem = GLPK.glp_create_prob()) {
				// 問題名
				GLPK.glp_set_prob_name(problem, "sample");
				// 最適化の方向
				GLPK.glp_set_obj_dir(problem, GLPK.GLP_MAX);
				// 制約式の数・名前・範囲
				GLPK.glp_add_rows(problem, 3);
				GLPK.glp_set_row_name(problem, 1, "p");
				GLPK.glp_set_row_bnds(problem, 1, GLPK.GLP_UP, 0.0, 100.0);
				GLPK.glp_set_row_name(problem, 2, "q");
				GLPK.glp_set_row_bnds(problem, 2, GLPK.GLP_UP, 0.0, 600.0);
				GLPK.glp_set_row_name(problem, 3, "r");
				GLPK.glp_set_row_bnds(problem, 3, GLPK.GLP_UP, 0.0, 300.0);
				// 変数の数・名前・範囲
				GLPK.glp_add_cols(problem, 3);
				GLPK.glp_set_col_name(problem, 1, "x1");
				GLPK.glp_set_col_bnds(problem, 1, GLPK.GLP_LO, 0.0, 0.0);
				GLPK.glp_set_col_name(problem, 2, "x2");
				GLPK.glp_set_col_bnds(problem, 2, GLPK.GLP_LO, 0.0, 0.0);
				GLPK.glp_set_col_name(problem, 3, "x3");
				GLPK.glp_set_col_bnds(problem, 3, GLPK.GLP_LO, 0.0, 0.0);
				// 目的関数の係数
				GLPK.glp_set_obj_coef(problem, 1, 10.0);
				GLPK.glp_set_obj_coef(problem, 2, 6.0);
				GLPK.glp_set_obj_coef(problem, 3, 4.0);
				// 制約式の係数
				var ia = GLPK.new_intArray(9 + 1);
				var ja = GLPK.new_intArray(9 + 1);
				var ar = GLPK.new_doubleArray(9 + 1);
				GLPK.intArray_setitem(ia, 1, 1); //行番号を代入
				GLPK.intArray_setitem(ia, 2, 1);
				GLPK.intArray_setitem(ia, 3, 1);
				GLPK.intArray_setitem(ia, 4, 2);
				GLPK.intArray_setitem(ia, 5, 2);
				GLPK.intArray_setitem(ia, 6, 2);
				GLPK.intArray_setitem(ia, 7, 3);
				GLPK.intArray_setitem(ia, 8, 3);
				GLPK.intArray_setitem(ia, 9, 3);
				GLPK.intArray_setitem(ja, 1, 1); //列番号を代入
				GLPK.intArray_setitem(ja, 2, 2);
				GLPK.intArray_setitem(ja, 3, 3);
				GLPK.intArray_setitem(ja, 4, 1);
				GLPK.intArray_setitem(ja, 5, 2);
				GLPK.intArray_setitem(ja, 6, 3);
				GLPK.intArray_setitem(ja, 7, 1);
				GLPK.intArray_setitem(ja, 8, 2);
				GLPK.intArray_setitem(ja, 9, 3);
				GLPK.doubleArray_setitem(ar, 1, 1.0); //係数を代入
				GLPK.doubleArray_setitem(ar, 2, 1.0);
				GLPK.doubleArray_setitem(ar, 3, 1.0);
				GLPK.doubleArray_setitem(ar, 4, 10.0);
				GLPK.doubleArray_setitem(ar, 5, 4.0);
				GLPK.doubleArray_setitem(ar, 6, 5.0);
				GLPK.doubleArray_setitem(ar, 7, 2.0);
				GLPK.doubleArray_setitem(ar, 8, 2.0);
				GLPK.doubleArray_setitem(ar, 9, 6.0);
				GLPK.glp_load_matrix(problem, 9, ia, ja, ar);
				// 最適化
				int result = GLPK.glp_simplex(problem, null);
				// 結果表示
				Console.WriteLine("");
				Console.Write($"z = {GLPK.glp_get_obj_val(problem)}");
				for(int i = 1; i <= GLPK.glp_get_num_cols(problem); ++i) {
					Console.Write($" {GLPK.glp_get_col_name(problem, i)} = {GLPK.glp_get_col_prim(problem, i)}");
				}
				Console.WriteLine("");
				// 後処理
				GLPK.delete_intArray(ia);
				GLPK.delete_intArray(ja);
				GLPK.delete_doubleArray(ar);
			}
		}
		// GlpkWrapperCSを使った場合
		static void Func2() {
			Console.WriteLine("【GlpkWrapperCS】");
			using(var problem = new MipProblem()) {
				// 問題名
				problem.Name = "sample";
				// 最適化の方向
				problem.ObjDir = ObjectDirection.Maximize;
				// 制約式の数・名前・範囲
				problem.AddRows(3);
				problem.RowName[0] = "p";
				problem.RowName[1] = "q";
				problem.RowName[2] = "r";
				problem.SetRowBounds(0, BoundsType.Upper, 0.0, 100.0);
				problem.SetRowBounds(1, BoundsType.Upper, 0.0, 600.0);
				problem.SetRowBounds(2, BoundsType.Upper, 0.0, 300.0);
				// 変数の数・名前・範囲
				problem.AddColumns(3);
				problem.ColumnName[0] = "x1";
				problem.ColumnName[1] = "x2";
				problem.ColumnName[2] = "x3";
				problem.SetColumnBounds(0, BoundsType.Lower, 0.0, 0.0);
				problem.SetColumnBounds(1, BoundsType.Lower, 0.0, 0.0);
				problem.SetColumnBounds(2, BoundsType.Lower, 0.0, 0.0);
				// 目的関数の係数
				problem.ObjCoef[0] = 10.0;
				problem.ObjCoef[1] = 6.0;
				problem.ObjCoef[2] = 4.0;
				// 制約式の係数
				var ia = new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 };
				var ja = new int[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 };
				var ar = new double[] { 1.0, 1.0, 1.0, 10.0, 4.0, 5.0, 2.0, 2.0, 6.0 };
				problem.LoadMatrix(ia, ja, ar);
				// 最適化
				var result = problem.Simplex();
				Console.WriteLine(result);
				// 結果表示
				Console.WriteLine("");
				Console.Write($"z = {problem.LpObjValue}");
				for(int i = 0; i < problem.ColumnsCount; ++i) {
					Console.Write($" {problem.ColumnName[i]} = {problem.LpColumnValue[i]}");
				}
				Console.WriteLine("");
			}
		}
		// GlpkWrapperCSを使った場合 その2
		static void Func3() {
			Console.WriteLine("【GlpkWrapperCS - 2】");
			using(var problem = new MipProblem()) {
				// 問題名
				problem.Name = "sample2";
				// 最適化の方向
				problem.ObjDir = ObjectDirection.Maximize;
				// 制約式の数・名前・範囲
				problem.AddRows(3);
				problem.RowName[0] = "p";
				problem.RowName[1] = "q";
				problem.RowName[2] = "r";
				problem.SetRowBounds(0, BoundsType.Upper, 0.0, 100.0);
				problem.SetRowBounds(1, BoundsType.Upper, 0.0, 600.0);
				problem.SetRowBounds(2, BoundsType.Upper, 0.0, 300.0);
				// 変数の数・名前・範囲
				problem.AddColumns(3);
				problem.ColumnName[0] = "x1";
				problem.ColumnName[1] = "x2";
				problem.ColumnName[2] = "x3";
				problem.SetColumnBounds(0, BoundsType.Lower, 0.0, 0.0);
				problem.SetColumnBounds(1, BoundsType.Lower, 0.0, 0.0);
				problem.SetColumnBounds(2, BoundsType.Lower, 0.0, 0.0);
				// 目的関数の係数
				problem.ObjCoef[0] = 10.0;
				problem.ObjCoef[1] = 6.0;
				problem.ObjCoef[2] = 4.0;
				// 制約式の係数
				var ia = new int[] { 0, 0, 0, 1, 1, 1, 2, 2, 2 };
				var ja = new int[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 };
				var ar = new double[] { 1.0, 1.0, 1.0, 10.0, 4.0, 5.0, 2.0, 2.0, 6.0 };
				problem.LoadMatrix(ia, ja, ar);
				// 変数条件
				problem.ColumnKind[0] = VariableKind.Integer;
				problem.ColumnKind[1] = VariableKind.Integer;
				problem.ColumnKind[2] = VariableKind.Integer;
				// 最適化
				var result = problem.BranchAndCut();
				Console.WriteLine(result);
				// 結果表示
				Console.WriteLine("");
				Console.Write($"z = {problem.MipObjValue}");
				for(int i = 0; i < problem.ColumnsCount; ++i) {
					Console.Write($" {problem.ColumnName[i]} = {problem.MipColumnValue[i]}");
				}
				Console.WriteLine("");
			}
		}
	}
}
