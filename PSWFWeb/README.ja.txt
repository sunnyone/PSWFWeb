PSWFWeb - PowerShell Workflow WebConsole
=========================================

これは何？
-----------
PowerShell WorkflowをWebブラウザから実行したり、実行状況を見たりするためのアプリケーションです。

簡単な使い方
-------------
1. workflowsフォルダにWorkflowを含む.ps1ファイルを配置する。
このとき、スクリプトにはファイル名（拡張子を除く）と同じWorkflowを定義します（例えば、Sample.ps1ならSampleという名前のworkflow）。

具体的なスクリプトは、SampleWorkflow.ps1を参照ください。

2. PSWFWeb.exeを管理者で起動する。
コンソールにURLが表示されることを確認します。

3. ブラウザでアクセスし、Workflowsからワークフローを選択し、Startを実行する。
初期パスワードは、powershell / powershellになっています。

ちゃんとした使い方
--------------------
1. ポート番号やPasswordを変更する。
設定は、config.xmlに含まれています。パスワードはハッシュになっていますので、以下のコマンドで表示される出力を貼りつけてください。
  PSWFWeb /password YOUR-PASSWORD

2. urlaclの許可設定を行う。
PSWFWebがhttpをlistenしてもよいよう、netshコマンドで許可します。
  > netsh http add urlacl url=http://127.0.0.1:3579/ user=USERNAME

3. サービスの登録を行う。
PSWFWeb.exe /helpで表示されるコマンド（scコマンド）を実行し、サービスを登録します。

4. サービスを起動する。
「コンピュータの管理」画面などから、サービスを開始します。PowerShellでは、以下の通りです。
 PS> Start-Service PSWFWeb

