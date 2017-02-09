# 概要
　この文章は、『[GLPK for C#/CLI]()』のラッパークラス「GlpkWrapperCS」についての解説です。  
　GLPKの全APIをサポートはしていないのでご注意ください。

※元のGLPKは添字が1オリジンですが、GlpkWrapperCSでは0オリジンに変更しています。間違わないように！

# 使い方(ざっくり)
- MipProblemクラスのインスタンスを作成(名前を仮にAとする)
- 変数についての設定
 - AddColumnsメソッドで変数を追加
 - SetColumnBoundsメソッドで範囲を設定
 - ColumnKindプロパティで実数・整数・0-1変数のどれかを選択
- 目的関数についての設定
 - ObjDirプロパティで最大化問題か最小化問題かを設定
 - ObjCoefプロパティで係数を設定
- 制約式についての設定
 - AddRowsメソッドで制約式を追加
 - SetRowBoundsメソッドで範囲を設定
 - LoadMatrixメソッドで係数を設定
- 最適化および結果表示
 - SimplexメソッドかBranchAndCutメソッドで計算
 - LpObjValue・MipObjValueプロパティで目的関数の最適値を返す
 - LpColumnValue・MipColumnValueプロパティで変数の最適値を返す

# 解説
## 初期化と破棄について
　MipProblemクラスのコンストラクタでは、メンバ変数であるglp_probクラスのインスタンスを作成しています。  
　どちらもIDisposableを継承しており、デストラクタおよびDisposeメソッドで安全に解放されます。
## 問題自体に関わるメソッド・プロパティ
### Nameプロパティ(string型)
　問題の名称を表します。
### Simplex(bool messageFlg = true)メソッド
　シンプレックス法により、線形計画問題を解きます。  
　ColumnKindプロパティの内容は無視されるのでご注意ください。  
(混合整数計画問題における緩和解を求めるということ)  
　messageFlgがtrueの場合、標準出力に解答状況を表示します。  
　このメソッドを使用した後でないと、LpObjValue・LpColumnValueプロパティは意味をなしません。  
　返り値はSolverResult型で与えられます。以下に一覧を示します。

|名称|意味|
|----|----|
|OK|正常に解けた|
|ErrorBadBasis|初期基底に誤りがあった|
|ErrorSingular|基底行列が特異になった|
|ErrorCondition|基底行列の条件数が大きすぎる|
|ErrorBound|変数の範囲設定に誤りがある|
|ErrorFail|ソルバーに障害が発生した|
|ErrorObjectLowerLimit|目的関数が下限に達した|
|ErrorObjectUpperLimit|目的関数が上限に達した|
|ErrorIterationLimit|反復回数の制限を超えた|
|ErrorTimeLimit|計算時間の制限を超えた|
|ErrorNoPrimalSolution|主問題の解が存在しない|
|ErrorNoDualSolution|双対問題の解が存在しない|
|ErrorRoot|初期解として緩和解が与えられなかった|
|ErrorStop|検索が強制的に終了させられた|
|ErrorMipGap|ギャップが公差に達したので早期に終了した|

### BranchAndCut(bool messageFlg = true)メソッド
　分枝カット法により、混合整数計画問題を解きます。  
　引数や返り値はSimplexメソッドと同じ意味です。  
　このメソッドを使用した後でないと、MipObjValue・MipColumnValueプロパティは意味をなしません。  
　なお、このメソッドを使用した後は、LpObjValue・LpColumnValueプロパティが緩和解を返します。
### ToLpStringメソッド
　問題をCPLEX LPファイルの形式でstring型として出力します。
## 目的関数に関わるメソッド・プロパティ
### ObjDirプロパティ(ObjectDirection型)
　目的関数を最適化する方向を表します。以下に一覧を示します。

|名称|意味|
|----|----|
|Minimize|最小化問題|
|Maximize|最大化問題|

### ObjCoef[int index]プロパティ(double型)
　目的関数の係数を表します。indexは変数の番号です。
### LpObjValueプロパティ(double型)　※getのみ
　線形計画問題における目的関数の最適値を返します。
### MipObjValueプロパティ(double型)　※getのみ
　混合整数計画問題における目的関数の最適値を返します。

## 制約式に関わるメソッド・プロパティ
### AddRows(int n)メソッド
　制約式をn個追加します。すでに追加済みの場合は、その後ろに追加します。
### RowsCountプロパティ(int型)　※getのみ
　制約式の数を返します。
### RowName[int index]プロパティ(string型)
　制約式の名称を表します。indexは制約式の番号です。
### SetRowBounds(int index, BoundsType type, double lowerBound, double upperBound)メソッド
　制約式の範囲を設定します。indexは制約式の番号です。  
　type(BoundsType型)の値により設定内容が異なります。以下に一覧を示します。

|種類|範囲|意味|
|----|----|----|
|Free|-∞≦X≦∞|自由変数|
|Lower|lowerBound≦X≦∞|下限のみ設定|
|Upper|-∞≦X≦upperBound|上限のみ設定|
|Double|lowerBound≦X≦upperBound|下限と上限を設定|
|Fixed|X=lowerBound=upperBound|下限＝上限|

### RowType[int index]プロパティ(BoundsType型)　※getのみ
　制約式の種類を返します。indexは制約式の番号です。
### RowLB[int index]プロパティ(double型)　※getのみ
　制約式の下限を返します。indexは制約式の番号です。
### RowUB[int index]プロパティ(double型)　※getのみ
　制約式の上限を返します。indexは制約式の番号です。
### LoadMatrix(int size, int[] ia, int[] ja, double[] ar)メソッド
　制約式の係数を設定します。sizeは代入する係数の数です。  
　i=0～(size - 1)とした際、ia[i]番目の制約式のja[i]番目の変数の係数にar[i]を設定します。
### LoadMatrix(int[] ia, int[] ja, double[] ar)メソッド
　1つ上のメソッドにおいて、size = ia.Count()としたものです。
### RowMatrix[int index]プロパティ(Dictionary<int, double>型)　※getのみ
　制約式の係数を返します。indexは制約式の番号です。
## 変数に関わるメソッド・プロパティ
### AddColumns(int n)メソッド
　変数をn個追加します。すでに追加済みの場合は、その後ろに追加します。
### ColumnsCountプロパティ(int型)　※getのみ
　変数の数を返します。
### ColumnName[int index]プロパティ(string型)
　変数の名称を表します。indexは変数の番号です。
### SetColumnBounds(int index, BoundsType type, double lowerBound, double upperBound)メソッド
　変数の範囲を設定します。indexは変数の番号です。
### ColumnType[int index]プロパティ(BoundsType型)　※getのみ
　変数の種類を返します。indexは変数の番号です。
### ColumnLB[int index]プロパティ(double型)　※getのみ
　変数の下限を返します。indexは変数の番号です。
### ColumnUB[int index]プロパティ(double型)　※getのみ
　変数の上限を返します。indexは変数の番号です。
### ColumnKind[int index]プロパティ(VariableKind型)
　変数の種類を返します。indexは変数の番号です。以下に一覧を示します。

|種類|意味|
|----|----|
|Continuous|実数変数|
|Integer|整数変数|
|Binary|0-1変数|

### LpColumnValueプロパティ(double型)　※getのみ
　線形計画問題における変数の最適値を返します。
### MipColumnValueプロパティ(double型)　※getのみ
　混合整数計画問題における変数の最適値を返します。
