unit UfrmCap;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Menus,  ExtCtrls, ComCtrls, ToolWin, ImgList,UfrmSetting,
  StdCtrls, ShellAPI, ExtDlgs, Clipbrd, Printers, Buttons;

type
  TfrmCap = class(TForm)
    tim: TTimer;
    mMainMenu: TMainMenu;
    mmRate: TMenuItem;
    mmUP: TMenuItem;
    mmDown: TMenuItem;
    mmFix: TMenuItem;
    mmRate10: TMenuItem;
    mmRate15: TMenuItem;
    mmRate20: TMenuItem;
    mmFlipH: TMenuItem;
    mmFlipV: TMenuItem;
    mmFlexRotate: TMenuItem;
    mmRate25: TMenuItem;
    mmRate30: TMenuItem;
    mm40: TMenuItem;
    mm50: TMenuItem;
    mmRate60: TMenuItem;
    mmHelp: TMenuItem;
    mmGrid: TMenuItem;
    mmRate100: TMenuItem;
    mmRate160: TMenuItem;
    mmLeft: TMenuItem;
    mmRight: TMenuItem;
    mmUpper: TMenuItem;
    mmDowner: TMenuItem;
    N5: TMenuItem;
    mmCatchEchat: TMenuItem;
    eChat2: TMenuItem;
    mmGraph: TMenuItem;
    N6: TMenuItem;
    mmRedrawDesk: TMenuItem;
    N7: TMenuItem;
    Exit2: TMenuItem;
    mmWndSize: TMenuItem;
    N8: TMenuItem;
    mm300x300: TMenuItem;
    mm200x200: TMenuItem;
    N400x3001: TMenuItem;
    N400x4001: TMenuItem;
    mm300x400: TMenuItem;
    mmSampRate: TMenuItem;
    Help1: TMenuItem;
    mmWndInfo: TMenuItem;
    mmClassName: TMenuItem;
    mmWindowText: TMenuItem;
    mmGraph30: TMenuItem;
    mmGraph40: TMenuItem;
    mmGraph50: TMenuItem;
    mmGraph60: TMenuItem;
    mmGraphFlex: TMenuItem;
    mmAtariMode: TMenuItem;
    mmBltAtari: TMenuItem;
    mmClearAtari: TMenuItem;
    mmAtaris: TMenuItem;
    mmTopMost: TMenuItem;
    mmRate70: TMenuItem;
    mmRate80: TMenuItem;
    mmRate90: TMenuItem;
    sbMain: TStatusBar;
    About1: TMenuItem;
    mmRange: TMenuItem;
    mAtari: TMenuItem;
    N1: TMenuItem;
    N2: TMenuItem;
    ImageList1: TImageList;
    mmOption: TMenuItem;
    timRepeat: TTimer;
    N3: TMenuItem;
    N4: TMenuItem;
    mmFixView: TMenuItem;
    mmFixViewSw: TMenuItem;
    mmSaveViewFile: TMenuItem;
    mmCopyToClipBoard: TMenuItem;
    PrintDialog1: TPrintDialog;
    mmPrint: TMenuItem;
    N9: TMenuItem;
    mmSrcNot: TMenuItem;
    SavePictureDialog1: TSavePictureDialog;
    controlBar1: TControlBar;
    toolbarAdvanced: TToolBar;
    tbAtariMode: TSpeedButton;
    tbAtariVisible: TSpeedButton;
    tbGrid: TSpeedButton;
    tbFixSimu: TSpeedButton;
    toolbarAtari: TToolBar;
    pnAtariWidth: TPanel;
    lblAtariWidth: TLabel;
    clBoxAtari: TColorBox;
    udAtariWidth: TUpDown;
    InfoBar1: TStatusBar;
    Help2: TMenuItem;
    toolbarMain: TToolBar;
    tbZoomUp: TSpeedButton;
    tbZoomOut: TSpeedButton;
    tbFlipH: TSpeedButton;
    tbFlipV: TSpeedButton;
    tbLRotate: TSpeedButton;
    tbReset: TSpeedButton;
    tbRRotate: TSpeedButton;

    procedure Exit1Click(Sender: TObject);
    procedure FormMouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure timTimer(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure FormMouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure FormMouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure NUPClick(Sender: TObject);
    procedure mHFlipClick(Sender: TObject);
    procedure mVFlipClick(Sender: TObject);
    procedure mFlexFlipClick(Sender: TObject);
    procedure NDOWNClick(Sender: TObject);
    procedure mHelpClick(Sender: TObject);

    function flipHV(iHV: integer):Boolean;

    procedure mLeftClick(Sender: TObject);
    procedure mRightClick(Sender: TObject);
    procedure mGridClick(Sender: TObject);
    procedure FormResize(Sender: TObject);
    procedure ChgWindowSize(iSizeW : Integer; iSizeH : Integer);
    procedure mChgSizeClick(Sender: TObject);
    procedure mmRate15Click(Sender: TObject);
    procedure mmRate10Click(Sender: TObject);
    procedure mmRate20Click(Sender: TObject);
    procedure mmRate25Click(Sender: TObject);
    procedure mmRate30Click(Sender: TObject);
    procedure mm40Click(Sender: TObject);
    procedure mm50Click(Sender: TObject);
    procedure mmRate60Click(Sender: TObject);
    procedure mmFlipHClick(Sender: TObject);
    procedure mmFlipVClick(Sender: TObject);
    procedure mmUPClick(Sender: TObject);
    procedure mmDownClick(Sender: TObject);
    procedure mmFlexRotateClick(Sender: TObject);
    procedure mmRate100Click(Sender: TObject);
    procedure mmRate160Click(Sender: TObject);
    procedure mmGridClick(Sender: TObject);
    procedure mmLeftClick(Sender: TObject);
    procedure mmRightClick(Sender: TObject);
    procedure mmUpperClick(Sender: TObject);
    procedure mmDownerClick(Sender: TObject);
    procedure CreateWnd; override;
    procedure DestroyWnd; override;
    procedure mmFixClick(Sender: TObject);
    procedure mmRedrawDeskClick(Sender: TObject);
    procedure mm300x300Click(Sender: TObject);
    procedure mm200x200Click(Sender: TObject);
    procedure N400x3001Click(Sender: TObject);
    procedure N400x4001Click(Sender: TObject);
    procedure mm300x400Click(Sender: TObject);
    procedure Help1Click(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure mmSampRateClick(Sender: TObject);
    procedure mmGraph30Click(Sender: TObject);
    procedure mmGraph40Click(Sender: TObject);
    procedure mmGraph50Click(Sender: TObject);
    procedure mmGraph60Click(Sender: TObject);
    procedure mmGraphFlexClick(Sender: TObject);
    procedure mmAtariModeClick(Sender: TObject);
    procedure mmflipClick(Sender: TObject);
    procedure mmBltAtariClick(Sender: TObject);
    procedure mmClearAtariClick(Sender: TObject);
    procedure mmAtarisClick(Sender: TObject);
    procedure Exit2Click(Sender: TObject);
    procedure switchTopMost(onoff:Boolean);
    procedure mmTopMostClick(Sender: TObject);
    procedure mmRate70Click(Sender: TObject);
    procedure mmRate90Click(Sender: TObject);
    procedure mmRate80Click(Sender: TObject);
    procedure About1Click(Sender: TObject);
    procedure mmRangeClick(Sender: TObject);
    procedure mAtariClick(Sender: TObject);
    procedure rotateAngle(iAngle:integer);
    procedure tbFlipHClick(Sender: TObject);
    procedure tbRRotateMouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure tbFlipVClick(Sender: TObject);
    procedure tbZoomOutClick(Sender: TObject);
    procedure tbLRotateMouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure tbGridMouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure tbFixSimuClick(Sender: TObject);
    procedure tbAtariModeClick(Sender: TObject);
    procedure tbAtariVisibleClick(Sender: TObject);
    procedure tbResetClick(Sender: TObject);

    procedure SetAbsoluteForegroundWindow(hwnd :HWND);
    procedure mmOptionClick(Sender: TObject);
    procedure FormActivate(Sender: TObject);
    procedure clBoxAtariChange(Sender: TObject);
    procedure timRepeatTimer(Sender: TObject);
    procedure tbRRotateMouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure tbLRotateMouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure tbZoomUpMouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure tbZoomOutMouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure tbZoomUpMouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure tbZoomOutMouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure tbClearClick(Sender: TObject);
    procedure udAtariWidthClick(Sender: TObject; Button: TUDBtnType);
    procedure mmFixViewSwClick(Sender: TObject);
    procedure mmFixViewClick(Sender: TObject);
    procedure mmSaveViewFileClick(Sender: TObject);
    procedure mmCopyToClipBoardClick(Sender: TObject);
    procedure mmPrintClick(Sender: TObject);
    procedure mmSrcNotClick(Sender: TObject);
    procedure mmRateClick(Sender: TObject);
    procedure Help2Click(Sender: TObject);

  private
    { Private 宣言 }
    procedure WMHotKey(var Msg: TWMHotKey); message WM_HOTKEY;
    procedure chgCapRate(iRate:Single);
    procedure translateXY(x,y:integer;Shift:TShiftState;iWM:integer);
    procedure moveRange(direction:String);
    procedure drawOptions(hBack:HWND;mmx,mmy,rLeft,rTop:integer);
    procedure updateCaption;
    procedure startMyCapture;
    function fixMode(flg:Boolean):Boolean;
    function fixModeByKey:Boolean;
    procedure atariMode(onoff:Boolean);
    procedure registerMyHotkeys();
    procedure unRegisterMyHotkeys();

  public
    { Public 宣言 }

      bShowDlgBox : Boolean;        // @ ダイアログを出している途中かどうか
    procedure myOnMinimize(Sender: TObject);
    procedure myOnRestore(Sender: TObject);
    function getTargetWindow(pt:TPoint): integer;
    function FixView(onoff:Boolean; x:integer; y:integer):Boolean;

    procedure changeUIMode(iMode : Integer);


  end;
const
     crHandCursor = 7;

type
      TENM_MyHK = (   MHK_ZOOMUP   =201,
                      MHK_ZOOMDOWN =202,
                      MHK_MLeft = 211, MHK_MRight = 212,MHK_MUp = 213, MHK_MDown = 214,
                      MHK_FLIPV,
                      MHK_FLIPH,
                      MHK_FixMode ,
                      MHK_AtariMode,
                      MHK_AtariBlt,

                      MHK_FreeRotate,

                      MHK_Grid   ,

                      MHK_RRotate = 301,
                      MHK_LRotate = 302

                    );


var
  frmCap: TfrmCap;              // フォーム
  // ----------------------------------------------------------
  capRate : Single;             // @ 倍率
  bGrid : boolean;              // @ ｸﾞﾘｯﾄﾞフラグ
  // -------------------------------------------------------------
  bMdown : boolean;             // @ ドラッグ移動モード制御フラグ
  disX,disY  : Integer;         // @ ドラッグ移動用
  capSizeW,capSizeH: Integer;   // @ キャプチャサイズ
  bVFlip,bHFlip:Boolean;        // @ 反転フラグ
  transX,transY,transW,transH : integer;      // @ 拡大後の大きさ

  repeatMode : Integer;

  bFixed:boolean;               // @ 投影モードフラグ
  bFixedView : boolean;         //   固定ビューモード
  gAngle : integer;             // @ 回転角度
  baseX,baseY,
  clientBaseX,clientBaseY
   :integer;                    // @  固定モードのストレッチベースとクライアントベース
  hTarget:HWND;                 // @ ターゲットウィンドウハンドル
  rW : TRECT;                   // @ 矩形描画用レクト
  bMouseCap : Boolean;       // @ マウスキャプチャモードかどうか
  bStateUpdate : Boolean;       // @
  rcLastDraw :TRect;
  pStart,pEnd:TPoint;           // @ マウスキャプチャ
  bmpBackUp : TBitmap;          // @ バックアップビットマップ
  bmpAtari : TBitmap;            // @ アタリレイヤー

  bDrawedRect : Boolean;         // @ 矩形描画フラグ
  FFormShowing:Boolean;
  bGraph:Boolean;               // @ 方眼フラグ
  iCutPixel : integer;          // @ 方眼単位
  bBltAtari: Boolean;           // @ アタリを転送するかどうか
  bAtariMode : Boolean;         // @ アタリ描画モード

  bPenDown : Boolean;           // @ ペンがダウンしてるかどうか
  pCStart : TPoint ;            // @ クライアント押下ポイント
  bAtariBmpCreate : Boolean;    // @ アタリｂｍｐを作ったかどうか
  pOverPoint : TPoint;          // @ オーバーした時の座標
  bOverDrug : Boolean;
  bVirtualRDownEvent : Boolean; // @
  bVirtualLDownEvent : Boolean; // @
  strWindowText, strClassName : string;
  bRegistedMHK : boolean;
  //---------------------------------------------------------------
  iIncAngle:integer;
  sgIncCapRate : single;
  MyAtariColor : TColor;

  iRepeatAngle : integer;
  sgRepeatCapRate : single;
  iAtariLineWidth : integer;
  MHK : TENM_myHK ;
  hLastTargetWindow: integer;
  bForFixViewCap : Boolean;
  bSrcNot : Boolean;
const
  sAppTitle = 'StretchView';

implementation

{$R *.dfm}

{$R icon.res icon.rc}
uses XRotateBitmap,uFrmHelp,Math,uIniManager,UVersion;
const
  RM_ZOOM = 110;
  RM_ROTATE = 220;
  crMyHand = 15;
  crMyZoomIn = 16;
  crMyZoomOut = 17;
//****************************************************
//
//
procedure TfrmCap.myOnMinimize(Sender: TObject);
begin
           tim.Enabled := false;
           unRegisterMyHotkeys();

end;
//****************************************************
//
//        UIモードを変更
//
procedure TfrmCap.changeUIMode(iMode:Integer);
begin


end;
//****************************************************
//
//
procedure TfrmCap.myOnRestore(Sender: TObject);
begin
           tim.Enabled := true;
           RegisterMyHotkeys();

end;
//************************************************************************
//
//                              フォームクリエイト
//
//
procedure TfrmCap.CreateWnd;

begin
  inherited CreateWnd;
  // MOD_ALT      Alt  キー
  // MOD_CONTROL  Ctrl キー
  // MOD_SHIFT    Shiftキー
  Screen.Cursors[crHandCursor] := LoadCursor(hInstance, 'HAND');

  iIncAngle    := 10;
  sgIncCapRate := 0.1;
  iAtariLineWidth := 1;

    // @ 初期設定
  RegisterMyHotkeys();

  gAngle := 0;
  bFixed := false;
  bDrawedRect := false;
  iCutPixel := 60;
  hLastTargetWindow:=0;
  // @ 初期倍率
  capRate := iniManager.capRate;
  //application.MessageBox(pchar(inttostr(inimanager.ScaleHeight)),'');
  chgWindowSize(iniManager.scaleWidth,iniManager.ScaleHeight );


  // @ 初期縦横サイズ
  //application.MessageBox(pchar(inttostr(inimanager.BoundsRect.Left )),'');


end;
//***********************************************************************
//
//
//                      ホットキーの解除
//
procedure TfrmCap.unRegisterMyHotkeys();
begin
        exit;
        try

            if ( bRegistedMHK = false) then exit;

            UnRegisterHotKey(Handle,Integer(MHK_ZOOMUP));
            UnRegisterHotKey(Handle,Integer(MHK_ZOOMDOWN));
            UnRegisterHotKey(Handle,Integer(MHK_MLeft));
            UnRegisterHotKey(Handle,Integer(MHK_MRight));
            UnRegisterHotKey(Handle,Integer(MHK_MUp));
            UnRegisterHotKey(Handle,Integer(MHK_MDown));


            UnRegisterHotKey(Handle,Integer(MHK_FreeRotate));
            UnRegisterHotKey(Handle,Integer(MHK_FliPV));
            UnRegisterHotKey(Handle,Integer(MHK_FlipH));
            UnRegisterHotKey(Handle,Integer(MHK_Grid));
            UnRegisterHotKey(Handle,Integer(MHK_AtariMode));
            UnRegisterHotKey(Handle,Integer(MHK_AtariBlt));

            UnRegisterHotKey(Handle,Integer(MHK_RRotate));
            UnRegisterHotKey(Handle,Integer(MHK_LRotate));
            bRegistedMHK := false;

        except
        end;

end;
//***********************************************************************
//
//
//                              ホットキーの登録
//
procedure TfrmCap.registerMyHotkeys();
var
  sErrkey : string;
begin

  //
  exit;

  sErrkey := '';
  if bRegistedMHK = true then exit;
  try

  // Ctrl + F1 を登録する   ↓IDを123にしてみました
  if RegisterHotKey(Handle, Integer(MHK_ZOOMUP), MOD_CONTROL, $41 ) = False then
    sErrkey := sErrKey +  ' Ctrl+A:"拡大"';
  if RegisterHotKey(Handle, Integer(MHK_ZOOMDOWN), MOD_CONTROL, $53) = False then
    sErrkey := sErrKey +  ' Ctrl+S:"縮小"';
  if RegisterHotKey(Handle, Integer(MHK_FLIPH), MOD_CONTROL, $44) = False then // @ 左右反転 $83 = S
    sErrkey := sErrKey +  ' Ctrl+D:"左右反転"';
  if RegisterHotKey(Handle, Integer(MHK_FLIPV), MOD_CONTROL, $46 ) = False then
    sErrkey := sErrKey +  ' Ctrl+F:"上下反転"';
  if RegisterHotKey(Handle, Integer(MHK_AtariMode), MOD_CONTROL, VK_F2 ) = False then  // Q 51
    sErrkey := sErrKey +  ' Ctrl+F2:"アタリモード切替"';
  if RegisterHotKey(Handle, Integer(MHK_AtariBlt), MOD_CONTROL, VK_F3 ) = False then  // Q 51
    sErrkey := sErrKey +  ' Ctrl+F3:"アタリ可視/不可視"';
  if RegisterHotKey(Handle, INteger(MHK_FixMode), MOD_CONTROL, $45 ) = False then
    sErrkey := sErrKey +  ' Ctrl+E:"固定モード切替"';
  if RegisterHotKey(Handle, Integer(MHK_MLeft), MOD_CONTROL, VK_LEFT ) = False then
    sErrkey := sErrKey +  ' Ctrl+←';
  if RegisterHotKey(Handle, Integer(MHK_MRight), MOD_CONTROL, VK_RIGHT ) = False then
    sErrkey := sErrKey +  ' Ctrl+→';
  if RegisterHotKey(Handle, Integer(MHK_MUp), MOD_CONTROL, VK_UP ) = False then
    sErrkey := sErrKey +  ' Ctrl+↑';
  if RegisterHotKey(Handle, Integer(MHK_MDown), MOD_CONTROL, VK_DOWN ) = False then
    sErrkey := sErrKey +  ' Ctrl+↓';
  if RegisterHotKey(Handle, Integer(MHK_FreeRotate), MOD_CONTROL, $54 ) = False then
    sErrkey := sErrKey +  ' Ctrl+T:"自由角度回転"';
  if RegisterHotKey(Handle, Integer(MHK_Grid), MOD_CONTROL, $47 ) = False then
    sErrkey := sErrKey +  ' Ctrl+G:"方眼紙"';
  if RegisterHotKey(Handle, INteger(MHK_LRotate), MOD_SHIFT, VK_LEFT ) = False then // Z
    sErrkey := sErrKey +  ' Ctrl+Z:反時計周り回転';
  if RegisterHotKey(Handle, Integer(MHK_RRotate), MOD_SHIFT, VK_RIGHT ) = False then //X
    sErrkey := sErrKey +  ' Ctrl+X:時計周り回転';

  if sErrKey <> '' then begin end;
  bRegistedMHK := true;


  except

  end;




end;
//**********************************************************************
//
//
//
//
procedure TfrmCap.DestroyWnd;

begin

  UnregisterMyHotKeys();

  // @ 解放
  bmpBackup.Free ;
  if bAtariBmpCreate then bmpAtari.Free ;

  invalidateRect(0,0,true);

  inherited DestroyWnd;

end;
//************************************************************************
//
//                             ホットキーハンドラ
//
//
procedure TfrmCap.WMHotKey(var Msg: TWMHotKey);
begin
  //               ↓ここでIDをチェックします
  if Msg.HotKey = Integer(MHK_ZOOMUP) then begin
    chgCapRate(capRate+sgIncCapRate);

  end else if  Msg.HotKey = Integer(MHK_ZOOMDown) then begin
      chgCapRate(capRate-sgIncCapRate);

  end else if  Msg.HotKey = Integer(MHK_LRotate) then begin

      gAngle := gAngle+ iIncAngle;
      rotateAngle(iIncAngle);

  end else if Msg.HotKey = Integer(MHK_RRotate) then begin
      gAngle := gAngle - iIncAngle;
      rotateAngle(iIncAngle);
      updatecaption;



  end else if Msg.HotKey = Integer(MHK_FLIPH) then begin
      mHFlipClick(Self);

  end else if  Msg.HotKey = Integer(MHK_FLIPV) then begin
        mVFlipClick(Self);

  end else if  Msg.HotKey = Integer(MHK_AtariBlt) then begin

      mmBltAtari.Checked := bBltAtari;
      tbAtariVisible.Down := bBltAtari;

      mmBltAtariClick(Self);

      updatecaption();

  end else if  Msg.HotKey = Integer(MHK_AtariMode) then begin

      mmAtariMode.Checked := bAtariMode;
      tbAtariMode.Down := bAtariMode;

      mmAtariModeClick(Self);
      updatecaption;


  end else if  Msg.HotKey =Integer(MHK_FixMode) then begin

      fixModeByKey;
    
  end else if  Msg.HotKey = Integer(MHK_FreeRotate) then begin

      mFlexFlipClick(Self);

  end else if  Msg.HotKey = Integer(MHK_Grid) then begin

      bGraph := not bGraph;
      tbGrid.Down := bGraph;

  end;
  if bFixed then begin
    if Msg.HotKey = Integer(MHK_MLeft) then moveRange('left') ;
    if Msg.HotKey = Integer(MHK_MRight) then moveRange('right') ;
    if Msg.HotKey = Integer(MHK_MUp) then moveRange('up') ;
    if Msg.HotKey = Integer(MHK_MDown) then moveRange('down') ;
  end;
  Msg.Result := 0; //これは不要かもしれません
end;
//*****************************************************
//
//              moveRange()
//
//
procedure TfrmCap.moveRange(direction:String);
begin

        if direction = 'left' then begin
              baseX := baseX - 10

        end else if direction = 'right' then begin
              baseX := baseX + 10
        end else if direction = 'up' then begin
             baseY := baseY - 10
        end else if direction = 'down' then begin
              baseY := baseY + 10
        end;
        // @ 範囲チェック
        if baseX < Screen.DesktopRect.Left  then baseX := Screen.DesktopRect.Left ;
        if baseY < Screen.DesktopRect.Top  then baseY := Screen.DesktopRect.Top ;

        if baseX + capsizeW > Screen.DesktopRect.Right   then baseX := Screen.DesktopRect.Right - capsizeW;
        if baseY + capsizeH > Screen.DesktopRect.Bottom   then baseY := Screen.DesktopRect.Bottom  - capsizeH;

        // @ クライアント基準座標を更新
        clientBaseX := baseX - rW.Left  ;
        clientBaseY := baseY - rW.Top  ;


end;
//************************************************************************
//
//                              終了
//
//
procedure TfrmCap.Exit1Click(Sender: TObject);
begin
        Application.MainForm.Close();
end;
//************************************************************************
//
//                             キーによる切替
//
//
function TfrmCap.fixModeByKey;
begin
        mmFix.Checked := not mmFix.Checked;
        if mmFix.Checked then begin
           tbFixSimu.Down:= true;
           fixMode(true);
        end else begin
           fixMode(false);
           tbFixSimu.Down:= false;

        end;



end;
//************************************************************************
//
//
//                      絶対に前に持ってくる

procedure TfrmCap.SetAbsoluteForegroundWindow(hwnd :HWND);
var
  nTargetID,nForegroundID: integer;
  sp_time: DWORD;
  set_time: DWORD;
begin

    // フォアグラウンドウィンドウを作成したスレッドのIDを取得
    nForegroundID := GetWindowThreadProcessId(GetForegroundWindow(), nil);
    // 目的のウィンドウを作成したスレッドのIDを取得
    nTargetID     := GetWindowThreadProcessId(hWnd, nil );
    set_time := 1000;
    // スレッドのインプット状態を結び付ける
    AttachThreadInput(nTargetID, nForegroundID, TRUE );  // TRUE で結び付け

    // 現在の設定を sp_time に保存
    SystemParametersInfo( SPI_GETFOREGROUNDLOCKTIMEOUT,0,@sp_time,0);
    // ウィンドウの切り替え時間を 0ms にする
    SystemParametersInfo( SPI_SETFOREGROUNDLOCKTIMEOUT,0,TObject(0),0);

    // ウィンドウをフォアグラウンドに持ってくる
    SetForegroundWindow(hWnd);

    // 設定を元に戻す
    SystemParametersInfo( SPI_SETFOREGROUNDLOCKTIMEOUT,0,TObject(sp_time),0);

    // スレッドのインプット状態を切り離す
    AttachThreadInput(nTargetID, nForegroundID, FALSE );  // FALSE で切り離し




end;
{void SetAbsoluteForegroundWindow(HWND hWnd)

    int nTargetID, nForegroundID;
    DWORD sp_time;

    // フォアグラウンドウィンドウを作成したスレッドのIDを取得
    nForegroundID = GetWindowThreadProcessId(GetForegroundWindow(), NULL);
    // 目的のウィンドウを作成したスレッドのIDを取得
    nTargetID = GetWindowThreadProcessId(hWnd, NULL );

    // スレッドのインプット状態を結び付ける
    AttachThreadInput(nTargetID, nForegroundID, TRUE );  // TRUE で結び付け

    // 現在の設定を sp_time に保存
    SystemParametersInfo( SPI_GETFOREGROUNDLOCKTIMEOUT,0,&sp_time,0);
    // ウィンドウの切り替え時間を 0ms にする
    SystemParametersInfo( SPI_SETFOREGROUNDLOCKTIMEOUT,0,(LPVOID)0,0);

    // ウィンドウをフォアグラウンドに持ってくる
    SetForegroundWindow(hWnd);

    // 設定を元に戻す
    SystemParametersInfo( SPI_SETFOREGROUNDLOCKTIMEOUT,0,sp_time,0);

    // スレッドのインプット状態を切り離す
    AttachThreadInput(nTargetID, nForegroundID, FALSE );  // FALSE で切り離し

}


//************************************************************************
//
//                            @ タイマー
//
//
procedure TfrmCap.timTimer(Sender: TObject);
var
   DesktopDC: HDc;
   // @ ビットマップ
   bmpTmp : TBitmap;
   bmpRotated : TBitmap;
   bFlipped: Boolean;
   bmpFlipped : TBitmap;
   //:  integer
   rleft , rtop,
   tmpMx,tmpMy,
   mmx,mmy,
   halfCapW,halfCapH:integer;
   argHFlip : Boolean;
   // rec
   tmpRect : TRect;
   cvDesk : TCanvas;
   bRotated : Boolean;
   rcFrm : TRect;
   bpp : integer;
   RastaOp : integer;

begin



  if bShowDlgBox then exit;
  // @ ウィンドウ内に入っているとき
  {if (not bFixed) and (not bStateUpdate) and (not bAtariMode) then  begin

   tmpRect.Left   := frmcap.Left;
   tmpRect.Top    := frmcap.Top;
   tmpRect.right  := frmcap.Left + frmcap.Width;
   tmpRect.bottom := frmcap.top + frmcap.height;

   if ptinrect(tmpRect,mouse.CursorPos) then exit;


  end;
   }
  bStateUpdate := false;

  try

    // @  マウス座標の処理
    tmpMx  := mouse.CursorPos.x;
    tmpMy  := mouse.CursorPos.y;

    mmx    := tmpMx;
    mmy    := tmpMy;


    // 画面座標の 表示
    if not (bFixed or bAtariMode) then begin
      rcFrm.Left  := frmCap.Left;
      rcFrm.top   := frmCap.top;
      rcFrm.Right := frmCap.Left + frmCap.Width;
      rcFrm.bottom:= frmCap.top + frmCap.Height;
      if not ptinRect(rcFrm,Mouse.CursorPos) then begin
         sbMain.Panels[2].Text := format('マウス座標(%4d,%4d)' ,[tmpMx,tmpmy]);
      end else begin

      end;
    end;

    // :  アクティブ化処理
    if bFixed and  (not frmCap.Active) and  ptinRect(rcFrm,Mouse.CursorPos) and
        (frmCap.WindowState = wsNormal)  then begin
        SetAbsoluteForegroundWindow(Self.Handle );
    end;
    // @ デスクトップのハンドル取得
    DesktopDC := GetWindowDC(GetDeskTopWindow);

    if capRate = 0 then capRate := 1;
    // @ キャプチャサイズを逆算して算出
    capSizeW :=Trunc( transW / capRate);
    capSizeH :=Trunc( transH / capRate);
    halfCapW := Trunc(capsizeW/2);
    halfCapH := Trunc(capsizeH/2);
    // ※  DesktopRectを使わなければならない
    if (tmpMx <= (Screen.DesktopRect.Left + halfCapW ))  then tmpMx := Screen.DesktopRect.Left + halfCapW;
    if (tmpMy <= halfCapH ) then tmpMy := halfCapH;
    if (tmpMx+halfCapW >= (Screen.DesktopRect.Right ) )  then tmpMx := Screen.DesktopRect.Right - halfCapW;
    if (tmpMy+halfCapH >= (Screen.DesktopRect.Bottom ) ) then tmpMy := Screen.DesktopRect.Bottom - halfCapH;
    // @ 左上の座標を算出
    rleft := tmpMx - halfCapW;
    rtop  := tmpMy - halfCapH;

    // 2007
    // 固定表示モード
    if bFixedView then begin
         rLeft := baseX;
         rTop  := baseY;
    end else if bFixed then begin     // @ 投影モードならベース座標計算
         getWindowRect(hTarget,rW);
         rLeft := baseX;
         rTop  := baseY;
    end;
    try
      try
        bRotated := false;
        bFlipped := false;
        // ① @ バックサーフェスを作る
        bmpTmp        := TBitmap.Create;

  // 2006.12.28 デスクトップの色数を取得
  bpp := GetDeviceCaps( GetDC(GetDeskTopWindow()) , BITSPIXEL);
  if bpp = 32 then begin
    bmpTmp.PixelFormat := pf32bit;
  end;
  if bpp = 24 then begin
    bmpTmp.PixelFormat := pf24bit;
  end;
  if bpp = 15 then begin
    bmpTmp.PixelFormat := pf15bit;
  end;
  if bpp = 16 then begin
    bmpTmp.PixelFormat := pf16bit;
  end;
  if bpp = 8 then begin
    bmpTmp.PixelFormat := pf8bit;
  end;
  //frmCap.Caption := IntToStr(bpp);
        if bSrcNot then begin
            RastaOp := NotSrcCopy;
        end else begin
            RastaOp := SrcCopy;
        end;

        // @ パフォーマンス維持用 処理分け
        // @ 反転/回転/処理がまったく無い場合  ---------------------------------------
        if not ( bHFlip or bVFlip or ( gAngle <> 0 ) ) then  begin
             //
             bmpTmp.Width  := capSizeW;
             bmpTmp.Height := capSizeH;

               if true then begin

                 // @ バッファにデスクトップから等倍でコピー
                 BitBlt (bmpTmp.Canvas.Handle,
                            0,0,capSizeW,capSizeH,
                         DeskTopDC,
                           rLeft, rTop,
                         RastaOp);

                 // @ アタリ転送があれば
                 if bBltAtari then begin
                   // @ バッファにアタリキャンバスからストレッチAnd転送
                   StretchBlt(bmpTmp.Canvas.Handle,
                                 0,0,capSizeW,capSizeH,
                              bmpAtari.Canvas.handle,
                                 0,0,transW,transH,
                             SrcAnd );
                 end;
               end;
               // @ 何もなければ
               bmpTmp.Width  := transW;
               bmptmp.Height := transH;

               // @ バッファにバッファ自身を拡大コピー
               StretchBlt(bmpTmp.Canvas.Handle,
                             0,0, transW,transH,
                          bmpTmp.Canvas.Handle,
                             0,0, capSizeW,capSizeH,
                          SrcCopy);

                // @ インジケータと転送のみ
                drawOptions(bmpTmp.canvas.Handle,mmx,mmy,rLeft,rTop );


                // @ 投影ウィンドウにバッファからコピー
                BitBlt (frmCap.Canvas.Handle,
                          transX,transY,transW,transH,
                        bmpTmp.Canvas.handle,
                          0,0,
                        SrcCopy );
         end else begin  // @ -----反転/回転処理有り------------------------------


           if ((bHFlip or bVFlip) and (gAngle = 0)) then begin // @ 反転処理のみ


              bmpTmp.Width  := capSizeW;
              bmpTmp.Height := capSizeH;
              // @ 拡大せずにバックバッファに転送
              BitBlt (bmpTmp.Canvas.Handle,
                     0,0,
                     capSizeW,capSizeH,
                     DeskTopDC,
                     rLeft, rTop,
                     RastaOp);
              // @ 縮小転送
              if bBltAtari then begin
                  StretchBlt(bmpTmp.Canvas.Handle,
                    0,0,capSizeW,capSizeH,
                    bmpAtari.Canvas.handle,
                    0,0,transW,transH,SrcAnd
                    );

              end;

              // @ 左右反転は上下反転のときもやらなければならないため or を取る
              argHFlip := (bHFlip or bVFlip);
              // @ 上下反転なら180度回転扱い
              if bVFlip then begin

                  bmpRotated := RotateBitmapX(bmpTmp ,1800,argHFlip,false);
              end else begin

                  bmpRotated := RotateBitmapX(bmpTmp ,gAngle,argHFlip,false);
              end;


             bmpTmp.Width := transW;
             bmpTmp.Height := transH;


             // @ プライマリにストレッチ転送
             StretchBlt(bmptmp.Canvas.Handle,
                            0,0, transW,transH,
                        bmpRotated.Canvas.Handle,
                            0,0, capSizeW,capSizeH,
                        SrcCopy);

              //end;
              drawOptions(bmptmp.Canvas.Handle,mmx,mmy,rLeft,rTop );


              BitBlt (frmCap.Canvas.Handle,
                        transX,transY,transW,transH,
                      bmpTmp.Canvas.handle,
                        0,0,
                      SrcCopy);
               bRotated := true;

           end else begin

               if (gAngle <> 0) then  begin   // @ 任意角度回転-----------------


                      bmpTmp.Width  := transW;
                      bmptmp.Height := transH;
                      // @ バックバッファに拡大転送
                      StretchBlt(bmptmp.Canvas.Handle,
                                0,0,
                                transW,transH,
                                DesktopDC, rleft,rtop,
                                capSizeW,capSizeH,
                                RastaOp);
                      // @ アタリの転送
                     if bBltAtari then begin
                         BitBlt (bmpTmp.Canvas.Handle,
                                 0,0,transW,transH,
                                 bmpAtari.Canvas.handle,
                                 0,0,SrcAnd
                                );
                      end;

                    drawOptions(bmptmp.Canvas.Handle,mmx,mmy,rLeft,rTop );

                    // @ 左右反転は上下反転のときもやらなければならないため or を取る
                    // @ 上下反転なら180度回転扱い
                    if( bVFlip or bHFlip) then begin
                       argHFlip := (bHFlip or bVFlip);
                       if bVFlip then begin

                           bmpFlipped := RotateBitmapX(bmpTmp ,1800,argHFlip,false);
                       end else begin

                           bmpFlipped := RotateBitmapX(bmpTmp ,0,argHFlip,false);

                        end;
                        bFlipped := true;
                        // @　回転実行
                        bmpRotated := RotateBitmapX(bmpFlipped ,gAngle,false,false);

                     end else begin

                        // @　回転実行
                        bmpRotated := RotateBitmapX(bmpTmp ,gAngle,false,false);
                     end;

                     bRotated := true;

                     // @  プライマリに転送
                     BitBlt (frmCap.Canvas.Handle,
                             transX,transY,transW,transH,
                             bmpRotated.Canvas.handle,
                             0,0,SrcCopy
                             );

                  end; // @回転か


           end;// @ 反転か

         end;// @ 回転反転ありかどうか


        except // @ 例外発生

        end;
      finally
         // 開放
         bmpTmp.Free;
         if bRotated = true then bmpRotated.Free;
         if bFlipped = true then bmpFlipped.Free;
      end;



   finally
     {デバイスコンテキストハンドルを開放}
     ReleaseDC(GetDesktopWindow, DesktopDC);
   end;
end;
procedure atariVisible(bE:Boolean);
begin


end;
//****************************************************************
//
//                      キャプションのアップデート
//
procedure TfrmCap.updateCaption;
var
     sState : array[0..5] of string;

begin
      // @----------------------------------------------------------------
      sState[0] := ''; sState[1] := '';  sState[2] := '';
      sState[3] := '';
      sState[4] := '';
      sState[5] := '';


      if bHFlip then  sState[0]   := '反転(左右)';
      if bVFlip then  sState[0]   := '反転(上下)';
      if not (gAngle =0) then begin
        sState[0] := '' + IntToStr(math.Floor( gAngle / 10) )+'°回転';
      end else begin
        sState[0] := '';
      end;
      if bFixed then begin
        sState[2]    := '表示固定 & 操作投影モードです';
        sState[5]    := ':' + strClassName;
      end;
      if bAtariMode then begin
        sState[2] := '表面レイヤ描き込みモードです';
        sState[4] := 'Ctrl+ドラッグ：消しゴム';
      end;
      if ( not bFixed ) and ( not bAtariMode) and ( not bMouseCap) then begin
        sState[2]:= '表示モード';
      end;
      if bMouseCap then begin
         if bForFixViewCap then begin
           sState[2] := '表示を固定する位置を選択してください：右クリックでキャンセル';
         end else begin
           sState[2] := '投影先を選択してください：右クリックでキャンセル';
         end;
      end;
      if bBltAtari then sState[3] := 'on'
      else sState[3] := 'off';

     try

      sbMain.Panels[0].Text := '表面レイヤ：' + sState[3];

      sbMain.Panels[1].text := sState[0];
       //format('%2.1fx %s ',[caprate,sState[2]]);

      sbMain.Panels[3].Text := sState[4];

      // @ 出力
      frmCap.Caption := format( 'マルチ拡大鏡 StretchView ' + '[倍率:%2.1f倍 回転:%3d度 状態:%s]%s',[capRate,gAngle,sState[2],sState[5]]);
     except

     end;
end;

//***********************************************************************
//
//                      グリッドやポインタなどの描画
//
//
procedure TfrmCap.drawOptions(hBack:HWND;mmx,mmy,rLeft,rTop:integer);
var
 tX,tY : integer;
 cvTmp: TCanvas;
 dX,dy : integer;

 idx,iWnum , iHnum : integer;
 iPsize,halfPsize:integer;
begin

   if (not (bGraph or bGrid) ) and ( bFixed or bAtariMode ) then exit;

   try


     cvTmp := TCanvas.Create;
     cvTmp.Handle := hBack;
         // Draw
     with cvTmp do  begin
         // Draw Cross
        Pen.Style := psDot;
        Pen.Mode := pmNotXor;

        // @ 中心線　--------------------------------
        if bGrid then begin
          dx := Trunc(transW / 2);
          dy := Trunc(transH/ 2);
          MoveTo(dx, 0);
          LineTo(dx, transH);

          MoveTo(0, dy);
          LineTo(transW, dy);
        end;
        // @ 方眼処理
        if bGraph then begin
           iWnum := Trunc(transW / iCutpixel);
           iHnum := Trunc(transH / iCutpixel);
           for idx := 1 to iWnum do begin
              dx := idx * iCutPixel;
              MoveTo(dx, 0);
              LineTo(dx, transH);
           end;
           for idx := 1 to iHnum do begin
              dy := idx * iCutPixel;
              MoveTo(0, dy);
              LineTo(transW, dy);
           end;
         end;

         // @ ポインタの処理 --------------------------------
         // @ 固定モードかどうか
         if (not bFixed) and ( not bAtariMode) then begin
            // @ 座標計算
            // @ 反転されているかどうか
            if bHFlip and
               (mmx < (screen.DesktopRect.Left + Trunc(capsizeW / 2)) ) then begin
               tx := screen.DesktopRect.Left + Trunc(capsizeW / 2) - mmx;
            end else begin
               tx := Trunc((mmx - rleft)*caprate) ;
            end;

            ty :=Trunc((mmy - rtop)*caprate);

            // @ ポインタ転送
            iPsize := Trunc(1*caprate);
            halfpSize:= Trunc(iPSize/2);

            BitBlt(hBack,tx, ty,
                   iPSize,iPSize,
                   hBack,
                   tx,ty ,DstInvert);
          end;

       end; //with


    finally
        cvTmp.free;
    end;

end;
//**************************************************************
//
//                      フォームクリエイト
//
//
procedure TfrmCap.FormCreate(Sender: TObject);
begin
    Application.OnMinimize := myOnMinimize;
    Application.OnRestore := myOnRestore;

    switchtopmost(true);
    updateCaption();
    // @ ビットマップ生成
    clBoxAtari.Selected := clBlue;
    myAtariColor := clBlue;
    
    bmpBackUp := TBitmap.Create;
    bAtariBmpCreate:=true;
    bmpAtari  := TBitmap.Create;
    // @ タイマーインターバル

    tim.Interval := inimanager.SamplingRate;
    frmcap.BoundsRect := IniManager.BoundsRect;
   // mmSetForeground.Checked := true;
    {
    Screen.Cursors[crMyHand] := LoadCursor(HInstance, 'HAND');
    Screen.Cursors[crMyZoomIn] := LoadCursor(HInstance, 'ZOOMIN');
    Screen.Cursors[crMyZoomOut] := LoadCursor(HInstance, 'ZOOMOUT');
     }
     if IniManager.FFixView then begin

       FixView(true, IniManager.FFixViewX,IniManager.FFixViewY);

     end;

end;
//procedure
function TfrmCap.FixView(onoff:Boolean; x:integer; y:integer):Boolean;

begin
    if onoff then begin

      bFixedView := true;
      capSizeW :=Trunc( transW / capRate);
      capSizeH :=Trunc( transH / capRate);

      // @ ベース座標を取得
      baseX := x - Trunc(capsizeW / 2) ;
      baseY := y - Trunc(capsizeH / 2) ;
      // @ はみ出し補正
      if baseX < screen.DesktopRect.Left  then baseX := 0;
      if baseY < screen.DesktopRect.top then baseY := 0;
      if baseX+capSizeW > screen.DesktopRect.Right then baseX := screen.DesktopRect.Right - capSizeW;
      if baseY+capSizeH > screen.DesktopRect.Bottom then baseY := screen.DesktopRect.bottom - capSizeH;
      IniManager.FFixViewX := x;
      IniManager.FFixViewY := y;
    end else begin
      bFixedView:=false;

    end;

    IniManager.FFixView := bFixedView;

end;

 ////////////////////////////////////////////////////////////
//
//
//
procedure calculateXY(var x:integer;var y :integer);
var
  tmpx,tmpy:integer;
  rTheta:single;
  sw:boolean;
begin

        // 任意角度回転
        if gAngle <> 0 then begin
            // @ 中心をゼロとした座標に変換
            x := x - Trunc(transW / 2);
            y := y - Trunc(transH / 2);
            tmpx:= x ; tmpy := y;
            // @ ラジアンにする
            rTheta := (gAngle /10) * pi  / 180;
            // @ 座標回転計算
            x := Trunc( tmpx*cos(rTheta) - tmpy*sin(rTheta) );
            y := Trunc( tmpx*sin(rTheta) + tmpy*cos(rTheta) );
            x := Trunc( transW / 2) + x;
            y := Trunc( transH / 2) + y;

          { if x<0 then begin  sw := true; x := 0 ; end;
           if y<0 then begin sw:= true; y := 0 ;end;
           if x> transW  then begin sw:= true; x := transH; end;
           if y> transW then begin sw := true ; y := transH; end;
           }

        end;

        // 左右反転なら反転させる
        if bHFlip then begin
              x := Trunc(transW / 2 )- (X - Trunc(transW / 2));
        end;
        if bVFlip then begin
           //   x := Trunc((transW / 2 )- (X - Trunc((transW / 2));
              y := Trunc(transH / 2 )- (Y - Trunc(transH / 2));
        end;



end;
//************************************************************
//
//                              座標変換
//
procedure TfrmCap.translateXY(X:Integer;Y:integer;Shift:TShiftState;iWM:integer);
var
  iShift : integer;
  cx,cy:integer;
  sw:Boolean;
  pt:TPoint;
begin
    iShift := 0;
    // : @ ダウン時の処理
     // @ リリース状態では、こちらのマウスアップハンドラより
     //  他のアプリがメッセージを先に受け取って描画してしまう　
     //  SendMessageでダウンが送られた後にリリースになると、
     //   ウィンドウがメッセージを受け取って描画が起きてしまう
     // リリースを防ぐには
     //  左か右のドラッグ状態でないとリリースされてしまう
     //  右の押下をエミュレートして、左が離されてもリリースされないようにする

     //   こっちのMouseイベントよりも早く相手が受け取ってしまうので、ここで受け取っていては遅い

      X := X + transX;
      Y := Y - transY;
        //
        if (ssLeft in Shift) and ( not bVirtualLDownEvent) then iShift := MK_LBUTTON;
        if (ssRight in Shift) and  ( not bVirtualRDownEvent) then iShift := MK_RBUTTON;
        if (ssMiddle in Shift) then iShift := MK_MBUTTON;
        if ssCtrl in Shift then iShift := iShift or MK_CONTROL;
        if ssShift in Shift then iShift := iShift or MK_SHIFT;


        
       if           (not bVirtualLDownEvent) and (not bVirtualRDownEvent) and(iWM = WM_LBUTTONDOWN) then begin

         // : @ 領域外左ドラッグ用
         // @ リリース予防
         //SendMessage(hTarget, WM_RBUTTONDOWN, transX,transY);

         mouse_event(MOUSEEVENTF_RIGHTDOWN,0,0,0,GetMessageExtraInfo());
         bVirtualRDownEvent:= true;
       end else if  (not bVirtualLDownEvent) and bVirtualRDownEvent and(iWM = WM_LBUTTONUP) then begin;
         // @ リリース予防
         //SendMessage(hTarget, WM_RBUTTONUP, transX,transY);
          mouse_event(MOUSEEVENTF_RIGHTUP,0,0,0,GetMessageExtraInfo());
          // @ リリース予防　解除

          bVirtualRDownEvent:= false;

      end else if (not (bVirtualRDownEvent)) and (not bVirtualLDownEvent) and (iWM = WM_RBUTTONDOWN) then begin

           // : @  領域外右ドラッグ用

          // @ リリース予防
          //SendMessage(hTarget, WM_LBUTTONDOWN, transX,transY);
           mouse_event(MOUSEEVENTF_LEFTDOWN,0,0,0,GetMessageExtraInfo());
           // @ リリース予防
           bVirtualLDownEvent:= true;
        end else if (not (bVirtualRDownEvent))and bVirtualLDownEvent and (iWM = WM_RBUTTONUP) then begin;

           // @ リリース予防　解除
           //SendMessage(hTarget, WM_LBUTTONUP, transX,transY);

           mouse_event(MOUSEEVENTF_LEFTUP,0,0,0,GetMessageExtraInfo());
           bVirtualLDownEvent:= false;
        end;

        // @ 共通の計算を行う
        calculatexy(x,y);

        // @ 倍率が高くなると、矢印の先頭にいきにくくなるので、y座標を少し引く
         y := y- math.floor(caprate / 2);
        // @ 四捨五入だと右に偏るので左に寄せる
         x := x- math.Floor(caprate/2);

        // @　送信出力補正
        if  strClassName = 'MSAWT_Comp_Class'  then begin
            // @ takamingは左に寄るので補正
             x := x+Trunc(1*caprate);
            // mmUp.Caption :=  'a';

        end;
        // @ クライアント座標に変換

        cx := clientbaseX + Trunc( X / capRate);
        cy := clientbaseY + Trunc( Y / capRate);

        // @ タイトルバーをクリックさせない
       { if (iWM= WM_LBUTTONDOWN) or (iWM = WM_RBUTTONDOWN) or
          (iWM = WM_LBUTTONUP) or (iWM = WM_RBUTTONUP) then begin

         if cy <0 then exit;
        end;
        }
        // @ ウィンドウメッセージ転送
        try

          if false then begin
            // << 2007.01.05  T.Yoshino
            pt.X := baseX + Trunc(X/capRate);
            pt.Y := baseY + Trunc(Y/capRate);
            hTarget := getTargetWindow( pt);
            // @ takamin対策
            if hTarget <> 0 then begin
              if  (iWM = WM_RBUTTONUP) then begin
                if cy <0 then exit;
              end;
              // >>
               SendMessage( hTarget, iWM, iShift,makelparam(cx,cy));
            end;

          end else begin

            if hTarget <> 0 then begin
                // @ takamin対策
                if  (iWM = WM_RBUTTONUP) then begin
                  if cy <0 then exit;
                end;
                // >>
                SendMessage( hTarget, iWM, iShift,makelparam(cx,cy));

               // PostMessage( hTarget, iWM, iShift,makelparam(cx,cy));
             end;
           end;

        except

        end;
end;
//**************************************************************
//
//                                      キャプチャ開始
//
procedure TfrmCap.startMyCapture;
begin

      if bAtariMode then bAtariMode:= false;
      bMouseCap := true;
      updatecaption();
      SetCapture(frmCap.Handle );
      // @ リリース予防
      mouse_event(MOUSEEVENTF_RIGHTDOWN,0,0,0,GetMessageExtraInfo());

end;

//************************************************************************
//
//                              フォームマウスダウン
//
//
procedure TfrmCap.FormMouseDown(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
var
  iWM : integer;
begin
       // frmcap.Caption := '111';
       // @ アタリモードかどうか
       if bAtariMode then begin
           x:= x - transX;
           y:= y - transY;

           calculatexy(x,y);
           bPenDown := true;
           pCStart.x := x;
           pCStart.y := y;

       // @ マウスキャプチャモードかどうか
       end else if bMouseCap then begin

           pStart := mouse.CursorPos;
       end else begin

         // @ 擬似的に起こしたイベントかどうか
         if (button = mbLeft) and  bVirtualLDownEvent then exit;
         if (Button = mbRight) and bVirtualRDownEvent then exit;

         if bFixed then begin
         // @ 固定モードなら
             if not frmcap.Active then begin
                frmcap.Activate;
                //exit;
             end;

              if Button = mbLeft then iWM := WM_LBUTTONDOWN;
              if Button = mbRight then iWM := WM_RBUTTONDOWN;


              translateXY(X,Y,Shift,iWM);
         end else begin // @ 非固定モード
               bMdown := true;
               disX := mouse.CursorPos.X - frmcap.left ;
               disY := mouse.CursorPos.Y - frmcap.top ;
         end;

       end;
end;
//************************************************************************
//
//                              フォームマウスムーブ
//
//
procedure TfrmCap.FormMouseMove(Sender: TObject; Shift: TShiftState; X,
  Y: Integer);
var
  cvDesk : TCanvas;
  DeskTopDC : HDC;
  mmx,mmy : integer;
  halfCapW,halfCapH : integer;
  rTmp : TRect;
  hTmp : integer;
begin


       frmcap.Cursor := crArrow;
       // : @ アタリモードの時
       if bAtariMode and bPenDown then begin

               // @ 座標計算
               x:= x - transX;
               y:= y - transY;

               calculatexy(x,y);
               with bmpAtari.Canvas do begin

                 if (ssLeft in Shift) and not (ssCtrl in Shift) then begin
                   // @ 左ドラッグ
                   pen.Width := iAtariLineWidth;
                   Pen.Color := myAtariColor;
                   pen.Mode := pmCopy;
                   frmcap.Cursor := crArrow;

                   MoveTo(pCStart.X,pCStart.Y);
                   LineTo(x,y);
                   pCStart.X:=x;
                   pCStart.Y:=y;
                 end else if (ssLeft in Shift) and (ssCtrl in Shift) then begin
                   // @ 消しゴム
                   pen.Width := Math.Floor(iAtariLineWidth*4);
                   Pen.Color := clWhite;
                   pen.Mode := pmWhite;
                   frmcap.Cursor := crArrow;

                   MoveTo(pCStart.X,pCStart.Y);
                   LineTo(x,y);
                   pCStart.X:=x;
                   pCStart.Y:=y;
                 end else if (ssRight in Shift) and not (ssCtrl in Shift) then begin
                   // @ 消しゴム
                   pen.Width := 5;
                   Pen.Color := clWhite;
                   pen.Mode := pmWhite;
                   frmcap.Cursor := crArrow;

                   MoveTo(pCStart.X,pCStart.Y);
                   LineTo(x,y);
                   pCStart.X:=x;
                   pCStart.Y:=y;

                 end;
               end;

       end else if bMouseCap then  begin // @ マウスキャプチャモード

        try
         // @  --------------------------------------------
         cvDesk := TCanvas.Create;
         mmx := Mouse.CursorPos.X;
         mmy := mouse.CursorPos.Y ;


 {*
         // 2007.01.05
         if (( mmx > frmCap.Left ) and (mmx < frmCap.Left+20))  then begin
           mmx := frmCap.Left;
           SetCursorPos(mmx,mmy);
         end;
         if ( mmx > frmCap.Left +frmCap.Width - 20 ) and ( mmx < frmCap.Left +frmCap.Width) then begin
           mmx := frmCap.Left + frmCap.Width;
           SetCursorPos(mmx,mmy);
          end;
         if ( mmy > frmCap.Top ) and (mmy < frmCap.Top+20) then begin
           mmy := frmCap.Top;
           SetCursorPos(mmx,mmy);
          end;
         if ( mmy > frmCap.Top+frmCap.Height - 20 ) and ( mmy < frmCap.Top + frmCap.Height) then begin
           mmy := frmCap.Top + frmCap.Height;
           SetCursorPos(mmx,mmy);
         end;
  *}

         with cvDesk do begin
           // @ デスクトップDCを取得
           DeskTopDC :=GetWindowDC(GetDeskTopWindow);
           Handle    := DeskTopDC ;
           //Pen.Mode  := pmNot;
           Pen.Style := psDash;
           Brush.Style := bsClear;
           halfCapW  := Trunc(capSizeW / 2);
           halfCapH  := Trunc(capSizeH / 2);


           with rcLastDraw do begin


             // 2007

             if not bForFixViewCap then begin
               // << 2007.01.05
               // 前のウィンドウを再描画
               //InvalidateRect( 0,0,false);
               GetWindowRect( hLastTargetWindow, rTmp);
               Pen.Style := psSolid;
               Pen.Width := 2;
               Pen.Color := clRed;

               //Windows.RedrawWindow( hLastTargetWindow, nil,0, RDW_INVALIDATE or RDW_ERASE or RDW_UPDATENOW or RDW_ALLCHILDREN);

               //InvalidateRect( hLastTargetWindow,nil, true);
               //UpdateWindow( hLastTargetWindow);


               // @ 再描画
               // @ 前回の領域を戻す
               if bDrawedRect then begin
                   BitBlt(cvDesk.Handle,
                          rTmp.Left,rTmp.Top,rTmp.Right-rTmp.Left+4,rTmp.Bottom-rTmp.Top+4,
                          bmpBackup.Canvas.Handle,0,0,SRCCOPY);

               end;

               //
               hTmp := getTargetWindow(Mouse.CursorPos );
               if hTmp <> 0 then begin

                 getWindowRect( hTmp , rTmp);
                 // 前回情報を残す
                 hLastTargetWindow := hTmp;

                bmpBackup.Width := rTmp.Right - rTmp.Left;
                bmpBackup.Height := rTmp.Bottom - rTmp.Top;
                // @ 今回描く領域をバックアップ
                BitBlt(bmpBackUp.Canvas.Handle,
                      0,0,rTmp.Right-rTmp.Left+4,rTmp.Bottom-rTmp.Top+4,
                      cvDesk.Handle,rTmp.Left,rTmp.Top,SRCCOPY);

                 rTmp.Top  := rTmp.Top + 2;
                 rTmp.Left := rTmp.Left + 2;
                 rTmp.Bottom:= rTmp.Bottom -4;
                 rTmp.Right := rTmp.Right - 4;

                 Rectangle( rTmp );

               end;
               frmCap.Update;
             end else begin


               if Mouse.CursorPos.X < Screen.DesktopRect.Left + halfCapW then SetCurSorPos(Screen.DesktopRect.Left + halfCapW,Mouse.CursorPos.Y);
               if Mouse.CursorPos.Y < halfCapH then SetCursorPos(Mouse.CursorPos.X, Screen.DesktopRect.Top + halfCapH);
               if Mouse.CursorPos.X > Screen.DesktopRect.Right - halfCapW then SetCursorPos(Screen.DesktopRect.Right - halfCapW,Mouse.CursorPos.Y);
               if Mouse.CursorPos.Y > Screen.DesktopRect.Bottom - halfCapH then SetCursorPos(Mouse.CursorPos.X,Screen.DesktopRect.Bottom - halfCapH);


               bmpBackup.Width := capSizeW+4;
               bmpBackup.Height := capSizeH+4;

               // @ 前回の領域を戻す
               if bDrawedRect then begin
                   BitBlt(cvDesk.Handle,
                          Left,Top,Right-Left+4,Bottom-Top+4,
                          bmpBackup.Canvas.Handle,0,0,SRCCOPY);

               end;




               // @ 新しい矩形をセット
               Left := mmx-halfCapW;
               Top := mmy-halfCapH;
               Right:=  mmx+halfCapW;
               Bottom := mmy+halfCapH;

               // @ 今回描く領域をバックアップ
               BitBlt(bmpBackUp.Canvas.Handle,
                      0,0,Right-Left+4,Bottom-Top+4,
                      cvDesk.Handle,Left,Top,SRCCOPY);

               // @ 矩形を描く
               Rectangle(Left,top,
                         Right,Bottom
                        );
             end;
             bDrawedRect := true;
            end;

            // invalidateRect(0,@rclastDraw,false);

           end;

          // ----------------------------------------------------
         finally
            cvDesk.Free;
            ReleaseDC(GetDesktopWindow, DesktopDC);

         end;


       end else if bFixed then begin       // @ こっちで描画モード　なら　
           //@ マウスアイコンを変える

           translateXY(x,y,Shift,WM_MOUSEMOVE);
       end else if  bAtariMode then begin
          frmCap.Cursor := crArrow;



       end else begin
          //frmCap.Cursor := crHandPoint;
          if frmcap.Cursor  <> crHandCursor then frmcap.Cursor := crHandCursor;

          //frmCap.Cursor := crMyHand;
          if bMdown then begin
               frmcap.left := mouse.CursorPos.X - disx;
               frmcap.top := mouse.CursorPos.y - disy;
           end;

       end;
       //application.ProcessMessages();
end;
//************************************************************************
//
//                              フォームマウスアップ
//
procedure TfrmCap.FormMouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
var
  iWM : integer;
begin



      if bAtariMode then begin
      // @ アタリモードの場合

            bPenDown := false;

      end else if bMouseCap then begin
      // @ キャプチャモードの場合

         with rclastDraw do begin
           bmpBackup.Width := capSizeW;
           bmpBackup.Height := capSizeH;

           // 2007.01
           if not bForFixViewCap then begin
             InvalidateRect(0,0,true);

           end else begin
             // @ 前回の領域を戻す
             BitBlt(GetWindowDC(GetDeskTopWindow),
                    Left,Top,Right-Left+4,Bottom-Top+4,
                    bmpBackup.Canvas.Handle,0,0,SRCCOPY);
           end;
           bDrawedRect := false;

          end;

          // 固定ビューモードのためのマウスキャプチャか
          if bForFixViewCap then begin

            if Button = mbLeft then begin

               bForFixViewCap:= false;
               FixView(true, Mouse.CursorPos.X, Mouse.CursorPos.Y );

               // @ リリース予防右ダウン解除
               mouse_event(MOUSEEVENTF_RIGHTUP,0,0,0,GetMessageExtraInfo());

               ReleaseCapture();
               bMouseCap:=false;

            end else begin
               ReleaseCapture();
               bMouseCap:=false;

            end;

          end else begin

            // @ 領域の決定
            if Button = mbLeft then begin

               tbFixSimu.Down:= true;
               mmfix.Checked := true;

               fixMode(true);
               // @ リリース予防右ダウン解除
               mouse_event(MOUSEEVENTF_RIGHTUP,0,0,0,GetMessageExtraInfo());
               pEnd := Mouse.CursorPos ;
               ReleaseCapture();
              //ClientToScreen(@pStart);
               //ClientToScreen(@pEnd);
               bMouseCap := false;
            end else begin // @ 右

               tbFixSimu.Down:= false;
               mmfix.Checked := false;

               // @ 固定キャンセル
               ReleaseCapture();
               bMouseCap:=false;
            end;

          end;

          toolbarmain.Refresh;
          updatecaption();

      end else begin  //: 通常モードの場合

          if (button = mbLeft) and  bVirtualLDownEvent then exit;
          if (Button = mbRight) and bVirtualRDownEvent then exit;


          if bFixed then  begin // @ 固定モードなら
               if Button = mbLeft then iWM := WM_LBUTTONUP;
               if Button = mbRight then iWM := WM_RBUTTONUP;
               translateXY(x,y,Shift,iWM);
          end;
          updatecaption();
          bMdown := false;

      end;

end;

//***************************************************
//
//                       ズームアップ
//
//
procedure TfrmCap.chgCapRate(iRate:Single);
begin
   // @  1 ～ 16倍

   if (iRate <= 1.0)  and (iRate >= 0.95 ) then iRate := 1;
   if (iRate >= 1.0 ) and (iRate <= 16) then begin


       // @ 固定モードなら
       if bFixed then begin

         baseX := baseX + Trunc(capsizeW / 2) ;
         baseY := baseY + Trunc(capsizeH / 2) ;

         capRate := iRate;

         capSizeW := Trunc(transW / capRate);
         capSizeH := Trunc(transH / capRate);

         baseX := baseX - Trunc(capsizeW / 2) ;
         baseY := baseY - Trunc(capsizeH /2 ) ;
         clientBaseX := baseX - rW.Left  ;
         clientBaseY := baseY - rW.Top ;


       end else begin
         // @ レートを上げる
         //application.MessageBox('test',pchar(inttostr(integer(math.Floor(iRate*1000)))));
         capRate := iRate;

       end;
       bStateUpdate:= true;
       updatecaption();
    end else begin

    end;


end;

procedure TfrmCap.NUPClick(Sender: TObject);
begin
        chgCapRate(capRate+1);

end;
//****************************************************
//
//                      ズームアウト
//
procedure TfrmCap.NDOWNClick(Sender: TObject);
begin
        chgCapRate(capRate-1);
end;

procedure TfrmCap.mVFlipClick(Sender: TObject);
begin
        flipHV(2);
end;

procedure TfrmCap.rotateAngle(iAngle:integer);
begin
         //gAngle := iAngle*10;
         mmFlipV.Checked := false;
         mmFlipH.Checked := false;
         bHFlip := false ;
         bVFlip := false ;
         mmFlexRotate.Checked := true;
         sbmain.Panels[1].text := IntToStr(math.Floor(gAngle/10))+'°回転';
         updatecaption;
         if bVFlip then mmFlipVClick(Self);
         if bHFlip then mmFlipHClick(Self);
end;



//******************************************************************
//
//                              任意角度回転
//
//
procedure TfrmCap.mFlexFlipClick(Sender: TObject);
var
        instr : string;
        iAngle : integer;
begin

   bShowDlgBox := true;
   switchTopmost(false);

    if mmFlexRotate.Checked then
    begin
        gAngle := 0;
        mmFlexRotate.Checked:= false;

    end else begin

       try
         instr := InputBox('任意角度回転','角度を入力してください(1～359)','');
         iAngle := strtoint(instr);

         if (instr <> '') and (iAngle > 0) and (iAngle < 360) then
         begin
                gAngle := iAngle * 10;
                mmFlipV.Checked := false;
                mmFlipH.Checked := false;
                bHFlip := false ;
                bVFlip := false ;
                mmFlexRotate.Checked := true;
         end;

         if bVFlip then mmFlipVClick(Self);
         if bHFlip then mmFlipHClick(Self);

       except
           application.MessageBox('不正な数値です','');
       end;

    end;
   switchtopmost(true);
   bShowDlgBox := false;

   timTimer(Self);
end;

   {
//*****************************************************************
//
//                      こっちで書き込み
//

procedure TfrmCap.mFixedClick(Sender: TObject);
begin


        mFixed.Checked := not mFixed.Checked;
       // fixMode(mFixced.checked);
        startmyCapture;
        exit;
end;}


function TfrmCap.getTargetWindow(pt:TPoint): integer;
var
  hRes : integer;
  hCwnd : HWND;
  pMouse: TPOint;
 bufWT : array[0..70] of char;                    //ウィンドウタイトル用のバッファ
 bufCN : array[0..70] of char;                    //ウィンドウタイトル用のバッファ

begin
        // @ WindowFromPointを実行
        hRes := WindowFromPoint(pt);
        // @ 値があるなら子ウィンドウを探す
        if hRes = 0 then  begin
           Result := 0;
           exit;
        end else begin // @ ハンドルがゼロ以外

           // 子ウィンドウを探す
           // @ クライアント座標系に変換
           Windows.ScreenToClient(hRes,pt);
	   hCWnd := ChildWindowFromPoint(hRes, pt);
	   if (hCWnd <> 0) then begin
              	hRes := hCWnd;
	   end;
           getWindowText(hRes,bufWT,71);
           getClassName(hRes,bufCN,71);

           strClassName := string(bufCN);
           strWindowText := string(bufWT);
           // 自分自身を選択しているかどうか
           if (strClassName = 'TfrmCap') or ( strClassName = 'toolbarMain') then begin
               // @ 自分自身を選択している
               Result:= 0;
               exit;
           end;
           //mmClassName.Caption := '<クラス名>"' + string(bufCN) + '"';
           //mmWindowText.Caption := '<ウィンドウテキスト>"' + string(bufWT) + '"';
           // @ 対象の矩形をゲットする
           getWindowRect(hRes,rW);
           Result:= hRes;

        end;


end;


//**************************************************************************
//
//                      固定モードに以降する
//

function TfrmCap.fixMode(flg:Boolean):Boolean;
var
 bufWT : array[0..70] of char;                    //ウィンドウタイトル用のバッファ
 bufCN : array[0..70] of char;                    //ウィンドウタイトル用のバッファ
 pt:TPoint;
 hCwnd : HWND;
 pMouse: TPOint;


begin

    if bAtariMode then bAtariMode:= false;
    // @ 初期化
    strClassName:= '';
    strWindowText:= '';

    bFixed := flg;
    mmFix.Checked := bFixed;
    if bFixed then begin
      try
        strClassName := '';
        strWindowText:= '';
        pMouse := mouse.CursorPos;
        // @ カーソルを得る
        GetCursorPos(pt);
        // @ WindowFromPointを実行
        hTarget := WindowFromPoint(pt);
        // @ 値があるなら子ウィンドウを探す
        if hTarget = 0 then  begin

           mmFix.Checked := false;
           bFixed := false;
           bShowDlgBox:= true;
           application.MessageBox(pchar('対象が取得できません ' + inttostr(hTarget)),'対象取得');
           bShowDlgBox:= false;
           exit;
        end else begin // @ ハンドルがゼロ以外

            // @ クライアント座標系に変換
           Windows.ScreenToClient(hTarget,pt);
	   hCWnd := ChildWindowFromPoint(hTarget, pt);
	   if (hCWnd <> 0) then begin
              	hTarget := hCWnd;
	   end;
           getWindowText(htarget,bufWT,71);
           getClassName(hTarget,bufCN,71);

           strClassName := string(bufCN);
           strWindowText := string(bufWT);

           if strClassName = 'TfrmCap' then begin
               // @ 自分自身を選択している
               mmFix.Checked := false;
               bFixed := false;
               bShowDlgBox:= true;
               application.MessageBox('自分自身を選択しています','対象取得');
               bShowDlgBox:= false;
               exit;
           end;
           mmClassName.Caption := '<クラス名>"' + string(bufCN) + '"';
           mmWindowText.Caption := '<ウィンドウテキスト>"' + string(bufWT) + '"';
           // @ 矩形をゲットする
           getWindowRect(hTarget,rW);
           // @ ベース座標を取得
           baseX := mouse.CursorPos.X - Trunc(capsizeW / 2) ;
           baseY := mouse.CursorPos.Y - Trunc(capsizeH / 2) ;
           // @ はみ出し補正
           if baseX < screen.DesktopRect.Left  then baseX := 0;
           if baseY < screen.DesktopRect.top then baseY := 0;
           if baseX+capSizeW > screen.DesktopRect.Right then baseX := screen.DesktopRect.Right - capSizeW;
           if baseY+capSizeH > screen.DesktopRect.Bottom then baseY := screen.DesktopRect.bottom - capSizeH;
           // @ クライアント座標
           clientBaseX := baseX - rW.Left  ;
           clientBaseY := baseY - rW.Top ;

        end;



            {if (  = 'eChat') and ( bufCN <> 'TScrollBox') then begin
              bFixed :=false;
              mFixed.Checked := false;

              application.MessageBox('取得失敗' , bufCN );
              exit;

            end;
             }
       except


       end;
    end;// bFixed?
    updatecaption();


end;

procedure TfrmCap.mHelpClick(Sender: TObject);
begin
        frmHelp.showModal();
end;

procedure TfrmCap.mLeftClick(Sender: TObject);
begin
        if bFixed then begin
            baseX := baseX + 10 ;
            clientBaseX := baseX - rW.Left  ;
        end;
end;

procedure TfrmCap.mRightClick(Sender: TObject);
begin
        if bFixed then begin
            baseX := baseX - 10 ;
            clientBaseX := baseX - rW.Left  ;
        end;

end;

procedure TfrmCap.mGridClick(Sender: TObject);
begin
        mmGrid.Checked := not mmGrid.Checked;

        bGrid := mmGrid.Checked;
end;

//**************************************************************
//
//                      窓のサイズ変更
//
//
//
procedure TfrmCap.chgWindowSize(iSizeW : Integer; iSizeH : Integer);
var
  maxW , maxH : integer;
begin


        maxH := Screen.Height ;
        maxW := screen.Width ;

        try
         if ( iSizeW >= 10 )and (iSizeW <= maxW)
            and ( iSizeH >= 10 )and (iSizeH <= maxH )
         then
         begin
          frmCap.ClientWidth := iSizeW;
          frmCap.ClientHeight := iSizeH;

          transX := 0;
          transY := 61;//InfoBar1.Height + ControlBar1.Height  ; // 21 高さ

          transW := iSizeW ;

          transH := iSizeH - (sbMain.Height + transY ) ;

          //@ サイズあわせ

          if bAtaribmpCreate then begin

            bmpAtari.Width := iSizeW;
            bmpAtari.Height := iSizeH;
          end;
         end else begin

          application.MessageBox('値が大きすぎるか小さすぎます','メッセージ');

         end;

        except
          application.MessageBox('値が大きすぎるか小さすぎます','メッセージ');
        end;
        bStateUpdate:= true;

end;
//**********************************************************
//
//                      リサイズ時
//
procedure TfrmCap.FormResize(Sender: TObject);
begin
//
        if frmcap.WindowState = wsNormal then begin
          RegisterMyHotkeys();
          chgwindowsize(frmcap.ClientWidth , frmcap.ClientHeight);

        end;


end;
//**********************************************************
//
//                      サイズ変更
//

procedure TfrmCap.mChgSizeClick(Sender: TObject);
var
iSizeW :integer;
iSizeH :integer;
begin
      iSizeW := StrToInt(InputBox( '','横サイズを入力してください(50～400)','0'));
       iSizeH := StrToInt(InputBox( '','縦サイズを入力してください(50～400)','0'));

        chgWindowSize(iSizeW,iSizeH);

end;

procedure TfrmCap.mmRate15Click(Sender: TObject);
begin
        chgCapRate(1.5);
end;

procedure TfrmCap.mmRate10Click(Sender: TObject);
begin
        chgCapRate(1);
end;

procedure TfrmCap.mmRate20Click(Sender: TObject);
begin
        chgCapRate(2);
end;

procedure TfrmCap.mmRate25Click(Sender: TObject);
begin
        chgCapRate(2.5);

end;

procedure TfrmCap.mmRate30Click(Sender: TObject);
begin
        chgCapRate(3);

end;

procedure TfrmCap.mm40Click(Sender: TObject);
begin
        chgCapRate(4);

end;

procedure TfrmCap.mm50Click(Sender: TObject);
begin
        chgCapRate(5);

end;

procedure TfrmCap.mmRate60Click(Sender: TObject);
begin
        chgCapRate(6);

end;
 procedure TfrmCap.mHFlipClick(Sender: TObject );
begin
        flipHV(1);
end;
//***************************************************
//
//               反転
//
function TfrmCap.flipHV(iHV: integer):Boolean;
begin


        //gAngle := 0;
        if iHV = 1 then begin   // @ 左右
          mmFlipH.Checked := not mmFlipH.Checked;
          tbFlipH.Down := mmFlipH.Checked;

          bHFlip := mmFlipH.Checked ;

          if bHFlip then begin // @ もう片方をオフに
             mmFlipV.Checked := false;
             tbFlipV.Down := mmFlipV.Checked;

             bVFlip := false;
          end;
        end else begin // @ 上下
          mmFlipV.Checked := not mmFlipV.Checked;
          bVFlip := mmFlipV.Checked;
          tbFlipV.Down := mmFlipV.Checked;

          // @ もう片方をオフに
          if bVFlip then begin
             mmFlipH.Checked := false;
             tbFlipH.Down := mmFlipH.Checked;

             bHFlip := false;
          end;
        end;
        bStateUpdate := true;

        timTimer(Self);
        updatecaption();
        Result:=true;
end;

procedure TfrmCap.mmFlipHClick(Sender: TObject);
begin
        flipHV(1);

end;

procedure TfrmCap.mmFlipVClick(Sender: TObject);
begin
        flipHV(2);   
end;


procedure TfrmCap.mmUPClick(Sender: TObject);
begin
           chgCapRate(capRate+1);

end;

procedure TfrmCap.mmDownClick(Sender: TObject);
begin
        chgCapRate(capRate-1);

end;

procedure TfrmCap.mmFlexRotateClick(Sender: TObject);
begin

     mFlexFlipClick(Sender);
     updatecaption;
end;

procedure TfrmCap.mmRate100Click(Sender: TObject);
begin
        chgCapRate(10);
end;

procedure TfrmCap.mmRate160Click(Sender: TObject);
begin
        chgCapRate(16);
end;

procedure TfrmCap.mmGridClick(Sender: TObject);
begin
        mGridClick(Sender);
end;

procedure TfrmCap.mmLeftClick(Sender: TObject);
begin
        moveRange('left');
end;

procedure TfrmCap.mmRightClick(Sender: TObject);
begin
        moveRange('right');

end;

procedure TfrmCap.mmUpperClick(Sender: TObject);
begin
        moveRange('up');

end;

procedure TfrmCap.mmDownerClick(Sender: TObject);
begin
        moveRange('down');

end;

procedure TfrmCap.mmFixClick(Sender: TObject);
begin

        mmFix.Checked := not mmFix.Checked;
        if mmFix.Checked then begin
          // application.MessageBox('投影先のキャンバス/オブジェクトを選択してください。(右クリックでキャンセル)','操作投影先オブジェクト選択');
           startMyCapture;
           SetCursorPos(frmCap.Left,frmCap.Top);

        end else begin
           fixMode(false);
        end;
end;

procedure TfrmCap.mmRedrawDeskClick(Sender: TObject);
begin
        invalidateRect(0,0,true);

end;

procedure TfrmCap.mm300x300Click(Sender: TObject);
begin
        chgWindowSize(300,300);
end;

procedure TfrmCap.mm200x200Click(Sender: TObject);
begin
        chgWindowSize(200,200);

end;

procedure TfrmCap.N400x3001Click(Sender: TObject);
begin
        chgWindowSize(400,300);

end;

procedure TfrmCap.N400x4001Click(Sender: TObject);
begin
        chgWindowSize(400,400);

end;

procedure TfrmCap.mm300x400Click(Sender: TObject);
begin
         chgWindowSize(300,400);

end;

procedure TfrmCap.Help1Click(Sender: TObject);
begin

  SetWindowPos(Self.Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE);
  mmTopMost.Checked := false;

  //2006.12.30
  //application.HelpFile := 'ScreenTools.hlp';
  //Application.HelpCommand(HELP_FINDER,0);


 // Web ヘルプを表示
  // uses ShellAPI が必要
  ShellExecute(0, nil, 'http://f29.aaa.livedoor.jp/~morg/wiki/index.php?StretchView%2FHelp', nil, nil, SW_NORMAL);

end;

procedure TfrmCap.FormDestroy(Sender: TObject);
var
  rRect : TRect;
begin
         iniManager.CapRate  := capRate;
         iniManager.BoundsRect := frmCap.BoundsRect ;

end;

procedure TfrmCap.mmSampRateClick(Sender: TObject);
var
  tmpStr: String;
  tmpInt :integer;
begin
  bShowDlgBox := true;
  switchTopMost(false);
  tmpStr := IntToStr(IniManager.SamplingRate);

  FFormShowing := True;
  try
    if (InputQuery('サンプリングレート', 'サンプリングレートを指定してください' + #13
          + '(単位:ms デフォルト 100  範囲 10～400)', tmpStr)) then

      tmpInt := StrToIntDef(tmpStr, IniManager.SamplingRate);

      if (tmpInt >= 10) and (tmpInt <= 400) then begin

         IniManager.SamplingRate := tmpInt;

         tim.Interval := IniManager.SamplingRate;

      end else begin
         application.MessageBox('範囲外の数値が入力されました。','');
      end;
  finally
    FFormShowing := False;
  end;
  switchTopMost(true);
  bShowDlgBox := false;

end;

procedure TfrmCap.mmGraph30Click(Sender: TObject);
begin
        mmGraph30.Checked := not mmGraph30.Checked;
        bGraph := mmGraph30.Checked;
        if  mmGraph30.Checked then  begin
            mmGraph40.Checked := false;
            mmGraph50.Checked := false;
            mmGraph60.Checked := false;

            iCutPixel := 30;

        end;
        mmGraph.Checked := bGraph;
        bStateUpdate := true;

end;

procedure TfrmCap.mmGraph40Click(Sender: TObject);
begin
        mmGraph40.Checked := not mmGraph40.Checked;
        bGraph := mmGraph40.Checked;
        if mmGraph40.Checked then  begin
            mmGraph30.Checked := false;
            mmGraph60.Checked := false;
            mmGraph50.Checked := false;

            iCutPixel := 40;

        end;

        mmGraph.Checked := bGraph;
        bStateUpdate := true;

end;

procedure TfrmCap.mmGraph50Click(Sender: TObject);
begin
        mmGraph50.Checked := not mmGraph50.Checked;
        bGraph := mmGraph50.Checked;
        if mmGraph50.Checked then  begin
            mmGraph30.Checked := false;
            mmGraph40.Checked := false;
            mmGraph60.Checked := false;

            iCutPixel := 50;

        end;
        mmGraph.Checked := bGraph;
        bStateUpdate := true;

end;

procedure TfrmCap.mmGraph60Click(Sender: TObject);
begin
         mmGraph60.Checked := not mmGraph60.Checked;
        bGraph := mmGraph60.Checked;
        if mmGraph60.Checked then  begin
            mmGraph30.Checked := false;
            mmGraph40.Checked := false;
            mmGraph50.Checked := false;

            iCutPixel := 60;

        end;
        mmGraph.Checked := bGraph;
        bStateUpdate := true;

end;

procedure TfrmCap.mmGraphFlexClick(Sender: TObject);
var
  tmpStr :string;
  tmpInt : integer;
begin
  bShowDlgBox := true;
  switchTopmost(false);

  if mmGraphFlex.Checked then begin
     bGraph := false;
     exit;
  end;
  bGraph := true;

  try

    if (InputQuery('方眼単位', '単位を入力してください。(ピクセル)', tmpStr)) then
      tmpint := StrToInt(tmpStr);

      iCutPixel := tmpint;

  finally

  end;
  bShowDlgBox := false;
  switchTopmost(true);

end;

procedure TfrmCap.mmAtariModeClick(Sender: TObject);
begin
        mmAtariMode.Checked :=not mmAtariMode.Checked ;
        tbAtarimode.Down := mmAtariMode.Checked;

        atariMode(mmAtariMode.checked);
        bStateUpdate:= true;
        updatecaption();

end;
procedure TfrmCap.mmflipClick(Sender: TObject);
begin
        //
        mmFlipV.Checked := bVFlip;
        mmFlipH.Checked := bHFlip;

end;
procedure TfrmCap.atariMode(onoff:Boolean);
begin
     bAtariMode := onoff;
     if bAtariMode = true then begin
       bBltAtari := true;
       tbAtariVisible.Down := true;
       bmpAtari.Width := transW;
       bmpAtari.Height := transH;
     end;




end;
procedure TfrmCap.mmBltAtariClick(Sender: TObject);
begin
        mmBltAtari.Checked := not mmBltAtari.Checked ;
        tbAtariVisible.Down := mmBltAtari.Checked;
        bBltAtari := mmBltAtari.Checked;
        bStateUpdate:= true;
        updatecaption();

end;
//***************************************************************
//
//                      最前面切り替え
//
//
procedure TfrmCap.switchTopMost(onoff: Boolean);
begin
     if onoff = true then begin

         SetWindowPos(Self.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE);
         mmTopMost.Checked := true;
     end else begin

         // APIで最前面表示
         SetWindowPos(Self.Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE);
         mmTopMost.Checked := false;
     end;
end;

procedure TfrmCap.mmClearAtariClick(Sender: TObject);
var
        fRect : TRect;
begin

        fRect.Left := 0;
        fRect.top := 0;
        fRect.bottom := transH;
        fRect.right := transW;

        bmpAtari.Canvas.FillRect(fRect);

end;
procedure TfrmCap.mmAtarisClick(Sender: TObject);
begin

       mmWndInfo.Enabled := bFixed;
       mmFix.Enabled := not bAtariMode;

end;

procedure TfrmCap.Exit2Click(Sender: TObject);
begin
        application.MainForm.Close();
end;

procedure TfrmCap.mmTopMostClick(Sender: TObject);
begin
         mmTopMost.Checked := not mmTopMost.Checked;
         switchtopmost(mmTopMost.Checked);

end;

procedure TfrmCap.mmRate70Click(Sender: TObject);
begin
        chgCapRate(7);        
end;

procedure TfrmCap.mmRate90Click(Sender: TObject);
begin
        chgCapRate(9);
end;

procedure TfrmCap.mmRate80Click(Sender: TObject);
begin
        chgCapRate(8);
end;

procedure TfrmCap.About1Click(Sender: TObject);
begin
        bShowDlgBox:= true;
        frmVersion.ShowModal;
        bShowDlgBox:= false;

end;

procedure TfrmCap.mmRangeClick(Sender: TObject);
begin
    mmLeft.Enabled := bFixed;
    mmRight.Enabled := bFixed;
    mmUpper.Enabled := bFixed;
    mmDowner.Enabled := bFixed;

    mmGraph.Checked := bGraph;
    mmGraph30.Checked := false;
    mmGraph40.Checked := false;
    mmGraph50.Checked := false;
    mmGraph60.Checked := false;
    mmGraphFlex.Checked := false;

    if bGraph then begin
     if iCutPixel = 30 then mmGraph30.Checked := true
     else if iCutPixel = 40 then mmGraph40.Checked := true
     else if iCutPixel = 50 then mmGraph50.Checked := true
     else if iCutPixel = 60 then mmGraph60.Checked := true
     else mmGraphFlex.Checked :=true;
    end;


end;

procedure TfrmCap.mAtariClick(Sender: TObject);
begin
       mmBltAtari.Checked := bBltAtari;
       mmAtariMode.Checked := bAtariMode;

end;

procedure TfrmCap.tbFlipHClick(Sender: TObject);
begin
            flipHV(1);

end;


procedure TfrmCap.tbRRotateMouseDown(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin

//         gAngle := gAngle+iIncAngle;
  //       rotateAngle(iIncAngle);

         repeatMode := RM_ROTATE;
         iRepeatAngle := iIncAngle;
         timRepeat.Enabled := true;

end;

procedure TfrmCap.tbFlipVClick(Sender: TObject);
begin
        flipHV(2);
end;

//**********************************************************
//
//
//
procedure TfrmCap.tbZoomOutClick(Sender: TObject);
begin


end;

//*****************************************************************
//
//              ボタン回転
//
procedure TfrmCap.tbLRotateMouseDown(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
//         gAngle := gAngle-iIncAngle;
//         rotateAngle(iIncAngle);

         repeatMode := RM_ROTATE;
         iRepeatAngle := -iIncAngle;
         timRepeat.Enabled := true;

end;

procedure TfrmCap.tbGridMouseDown(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
     bGraph := not bGraph;
end;

procedure TfrmCap.tbFixSimuClick(Sender: TObject);
begin
      mmFixClick(Self);

end;

procedure TfrmCap.tbAtariModeClick(Sender: TObject);
begin
      mmAtariModeClick(Self);

end;

procedure TfrmCap.tbAtariVisibleClick(Sender: TObject);
begin
        mmBltAtariClick(self);

end;

procedure TfrmCap.tbResetClick(Sender: TObject);
begin
        gAngle := 0;
        updatecaption;
end;

procedure TfrmCap.mmOptionClick(Sender: TObject);
begin
        bShowDlgBox := true;
        switchTopmost(false);
        frmSetting.showmodal;
end;

procedure TfrmCap.FormActivate(Sender: TObject);
begin

        RegisterMyHotkeys();
end;

procedure TfrmCap.clBoxAtariChange(Sender: TObject);
begin
        myAtariColor := clBoxAtari.Selected;
       // lblAtariWidth.Color := myAtariColor;
end;

//*****************************************************
//
//
//                     リピートタイマー
//
//
procedure TfrmCap.timRepeatTimer(Sender: TObject);
begin


        if repeatMode = RM_ZOOM then begin
               chgCapRate(capRate+sgRepeatCapRate);

        end else if repeatMode = RM_Rotate then begin
              gAngle := gAngle+ ( iRepeatAngle);
              rotateAngle(iRepeatAngle);


        end;
end;

procedure TfrmCap.tbRRotateMouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
        timRepeat.Enabled :=false;

end;

procedure TfrmCap.tbLRotateMouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
        timRepeat.Enabled := false;

end;

procedure TfrmCap.tbZoomUpMouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
        timRepeat.Enabled := false;

end;

procedure TfrmCap.tbZoomOutMouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
        timRepeat.Enabled := false;

end;

procedure TfrmCap.tbZoomUpMouseDown(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
         repeatMode := RM_ZOOM;
         sgRepeatCapRate := sgIncCapRate;
         timRepeat.Enabled := true;
end;

procedure TfrmCap.tbZoomOutMouseDown(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
          repeatMode := RM_ZOOM;

          sgRepeatCapRate := -sgIncCapRate;

          timRepeat.Enabled := true;
end;

procedure TfrmCap.tbClearClick(Sender: TObject);
begin
        mmClearAtariClick(Self);

end;

procedure TfrmCap.udAtariWidthClick(Sender: TObject; Button: TUDBtnType);
var
  iWidth : integer;
begin

      try

       iWidth := integer(udAtariWidth.Position);
       lblAtariWidth.Height := iWidth;
       lblAtariWidth.Top := 11 - (iWidth div 2);
       iAtariLineWidth := iWidth;
      except
      end;
end;

procedure TfrmCap.mmFixViewSwClick(Sender: TObject);
begin


        if not bFixedView then begin

          bForFixViewCap := true;
          StartMyCapture;
        end else begin
          // 固定ビューモード解除
          FixView(false,0,0);


        end;


end;

procedure TfrmCap.mmFixViewClick(Sender: TObject);
begin
        mmFixViewSw.Checked := bFixedView;
        mmLeft.Enabled  := bFixed or bFixedView;
        mmRight.Enabled  := bFixed or bFixedView;
        mmUpper.Enabled  := bFixed or bFixedView;
        mmDowner.Enabled  := bFixed or bFixedView;

end;
// 画像の保存
procedure TfrmCap.mmSaveViewFileClick(Sender: TObject);
var
  bmpClip: TBitmap ;
  sFile : string;
begin
     try
        SavePictureDialog1.Filter := 'ビットマップ(*.bmp)|*.bmp';
        if SavePictureDialog1.Execute then begin
          bmpClip := TBitmap.Create;
          bmpClip.Width := transW;
          bmpClip.Height := transH;
          BitBlt(bmpClip.Canvas.Handle , 0,0, transW, transH, frmCap.Canvas.Handle, 0,0,SRCCOPY);
          sFile := SavePictureDialog1.FileName;
          sFile := ChangeFileExt(sFile,'.bmp');

          bmpClip.SaveToFile(sFile);


          bmpClip.Free;


        end;
     except

     end;
end;

// 画像をクリップボードにコピー
procedure TfrmCap.mmCopyToClipBoardClick(Sender: TObject);
var
  bmpClip: TBitmap ;
begin
        bmpClip := TBitmap.Create;
        bmpClip.Width := transW;
        bmpClip.Height := transH;
        BitBlt(bmpClip.Canvas.Handle , 0,0, transW, transH, frmCap.Canvas.Handle, 0,0,SRCCOPY);
        Clipboard.Assign(bmpClip);

        bmpClip.Free;

end;

procedure TfrmCap.mmPrintClick(Sender: TObject);
var
  bmpClip: TBitmap ;
begin
   try
     bmpClip := TBitmap.Create;

     if PrintDialog1.Execute then begin
        bmpClip.Width := transW;
        bmpClip.Height := transH;
        BitBlt(bmpClip.Canvas.Handle , 0,0, transW, transH, frmCap.Canvas.Handle, 0,0,SRCCOPY);
        Printer.BeginDoc;
        //BitBlt(Printer.Canvas.Handle, 0,0, transW, transH, frmCap.Canvas.Handle, 0,0,SRCCOPY);
        Printer.Canvas.Draw(0,0,bmpClip);
        Printer.EndDoc;
     end;
   finally
        bmpClip.Free;

   end;

end;

procedure TfrmCap.mmSrcNotClick(Sender: TObject);
begin
         bSrcNot := not bSrcNot;

end;

procedure TfrmCap.mmRateClick(Sender: TObject);
begin
        mmSrcNot.Checked  := bSrcNot;
end;

procedure TfrmCap.Help2Click(Sender: TObject);
begin
  SetWindowPos(Self.Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE);
  mmTopMost.Checked := false;

  application.HelpFile := 'StretchView.hlp';
  Application.HelpCommand(HELP_FINDER,0);

end;

end.





