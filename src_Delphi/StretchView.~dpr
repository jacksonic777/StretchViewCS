program StretchView;

uses
  Forms,
  Windows,Dialogs,
  UfrmCap in 'UfrmCap.PAS' {frmCap},
  XRotateBitmap in 'XRotateBitmap.pas',
  UfrmHelp in 'UfrmHelp.pas' {frmHelp},
  uIniManager in 'uIniManager.pas',
  UVersion in 'UVersion.pas' {frmVersion},
  UfrmSetting in 'UfrmSetting.pas' {frmSetting},
  UfrmLisence in 'UfrmLisence.pas' {Form1};

{$R *.res}

var
  hMutex:THandle;
const
cnstLisence = 'BWEXPERP';
sAppTitle = 'StretchView';
begin
{*
  hMutex := OpenMutex(MUTEX_ALL_ACCESS,False,sAppTitle);
  //すでにMutexがあるかどうか調べる
  if hMutex <> 0 then
  begin  //あるとき
   // application.MessageBox('既に起動しています','');
    CloseHandle(hMutex);
    PostQuitMessage(0);
    exit;
  end
  else begin //ないとき
    hMutex := CreateMutex(nil,False,sAppTitle);
    // Mutexを作成しておく。MyProjectは、他のアプリと重複しないような
    // 文字列ならなんでもよい
  end;
 *}
 
  // シェアウェア機能
  // 起動回数が4回以上
  if IniManager.FRunCount >= 0 then begin
{*
    // ライセンスキーの確認
    if IniManager.FLisenceKey <> cnstLisence then begin
        // ライセンスキーの入力を促す
        IniManager.FLisenceKey := InputBox(sAppTitle + ' ライセンスキー入力', '本ソフトをご購入頂き大変ありがとうございます。'+#13+#10+'ご購入サイト(Vector様等)より配布のあったライセンスキーをご入力ください。'+#13+#10+'新規購入はVectorサイト様より可能です。','');
        // ライセンスキーが正しいかどうか
        if IniManager.FLisenceKey = '' then begin
            PostQuitMessage(0);
            exit;

        end;
        if IniManager.FLisenceKey <> cnstLisence then begin
            Application.MessageBox('一致致しませんでした。'+#13+#10+'終了致します。',sAppTitle);
            PostQuitMessage(0);
            exit;
        end;
    end else begin
       IniManager.FRunCount := 10;
    end;
*}
    
  end;

  Application.Title := 'StretchView';
  Application.Initialize;
  Application.CreateForm(TfrmCap, frmCap);
  Application.CreateForm(TfrmHelp, frmHelp);
  Application.CreateForm(TfrmVersion, frmVersion);
  Application.CreateForm(TfrmSetting, frmSetting);
  Application.CreateForm(TForm1, Form1);
  // スタートメニューへの登録
  if IniManager.FRunCount < 5 then begin
    if  frmSetting.ResisterStartMenu(true,true,sAppTitle) <> true then begin
        frmSetting.ResisterStartMenu(false,true,sAppTitle);
    end;
  end;

  Application.Run;

 // ReleaseMutex(hMutex);		//Mutexの解放

end.

