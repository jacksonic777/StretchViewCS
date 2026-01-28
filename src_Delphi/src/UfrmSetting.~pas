unit UfrmSetting;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, ComCtrls;
type
  TfrmSetting = class(TForm)
    btnOK: TButton;
    btnCancel: TButton;
    GroupBox1: TGroupBox;
    chkResistProgram: TCheckBox;
    procedure FormCreate(Sender: TObject);
    procedure btnCancelClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure btnOKClick(Sender: TObject);
    procedure FormShow(Sender: TObject);
  private
    { Private 宣言 }
  public
  
    function ResisterStartMenu(
      checkonly : boolean;
      onoff: boolean;
      strLinkName : string):Boolean;

    { Public 宣言 }
  end;

var
  frmSetting: TfrmSetting;
implementation
{$R *.dfm}
uses ShlObj, ActiveX, ComObj, Registry,UfrmCap;



procedure TfrmSetting.FormCreate(Sender: TObject);
begin

end;
//****************************************************************************
//
//                              スタートメニューに追加
//
function TfrmSetting.ResisterStartMenu(
      checkonly : boolean;
      onoff: boolean;
      strLinkName : string):Boolean;
var
  ShellLink: IShellLink;
  PersistFile: IPersistFile;
  FileName: String;
  StartMenuPath: array[0..MAX_PATH] of Char;
  pidlPath: PItemIDList;
  LinkFileName: string;
  WFileName: WideString;
  bExists : Boolean;
  flgResult :Boolean;

begin

   try
        { * ショートカットの設定}
        FileName:= ParamStr(0);  // : @ アプリケーション名

        ShellLink:= CreateComObject(CLSID_ShellLink) as IShellLink;   //ショートカット用COMオブジェクト作成
        PersistFile:= ShellLink as IPersistFile;                      //ショートカットファイル用オブジェクト

        with ShellLink do
        begin
         SetArguments('');      //ファイル名の後ろに指定するパラメータ
           SetPath(PChar(FileName));     //実行ファイルのフルパス
           SetWorkingDirectory(PChar(ExtractFileDir(FileName)));   //作業フォルダ
        end;

        {スタートアップパスの取得}
        SHGetSpecialFolderLocation(Application.Handle, CSIDL_PROGRAMS, pidlPath);
        SHGetPathFromIDList(pidlPath, StartMenuPath);

          {ショートカットファイルの名前決定}

         //LinkFileName:=Format('%s\%s',[StartMenuPath,ChangeFileExt(ExtractFilename(FileName),'.lnk')]);
         LinkFileName:=Format('%s\%s',[StartMenuPath, strLinkName + '.lnk']);

         // application.MessageBox(pchar(LinkFileName),'');
         WFileName := WideString(LinkFileName);
         bExists := FileExists(WFileName);

         // @ チェックだけかどうか
         if  checkonly = true then begin
            Result  := bExists;
            exit;
         end;
         // @ チェックだけでなければ
            if onoff = true then begin
            // @ 登録申請の場合
                try
                 // : 既に存在してなければ
                 if  not bExists then begin
                    // @ 作成する

                    OleCheck(PersistFile.Save(PWChar(WFileName),False));  // @ ファイル作成
                    flgResult :=true;

                 end;
                 except
                 end;
            end else begin
            // @ 削除申請
                try
                 if bExists then begin

                  //: 消す
                   deleteFile(WFileName);
                   flgResult :=false;
                 end;
                except
                end;

            end;
  except
     Result:=false;
     exit;
  end;
  Result:=true;



end;//proc
procedure TfrmSetting.btnCancelClick(Sender: TObject);
begin

        Self.Close;

end;

procedure TfrmSetting.FormClose(Sender: TObject; var Action: TCloseAction);
begin
        frmCap.bShowDlgBox := false;
        frmCap.switchTopmost(true);
end;

procedure TfrmSetting.btnOKClick(Sender: TObject);
begin
        ResisterStartMenu(false,chkResistProgram.Checked,sAppTitle);
        Self.Close;
end;

procedure TfrmSetting.FormShow(Sender: TObject);
begin
      chkResistProgram.Checked := ResisterStartMenu(true,false,sAppTitle);
end;

end.

