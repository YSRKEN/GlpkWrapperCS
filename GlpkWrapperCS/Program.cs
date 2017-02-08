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
			Func1();
			Console.WriteLine("");
			Func2();
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
				/*// 変数の数・名前・範囲
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
				// 制約式の係数*/

			}
		}
	}
}
