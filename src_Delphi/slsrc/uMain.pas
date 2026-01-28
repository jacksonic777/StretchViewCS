unit uMain;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  ExtCtrls, Menus, uPopupMenuEx;

type
  TfrmMain = class(TForm)
    MouseTimer: TTimer;
    popupMenu: TPopupMenuEx;
    menuChangeScale: TMenuItem;
    menuChangeScaleInput: TMenuItem;
    N1: TMenuItem;
    menuChangeScaleDown: TMenuItem;
    menuChangeScaleUp: TMenuItem;
    N2: TMenuItem;
    menuLeftSideRight: TMenuItem;
    menuUpSideDown: TMenuItem;
    N3: TMenuItem;
    menuExit: TMenuItem;
    menuVersion: TMenuItem;
    N4: TMenuItem;
    menuFilp: TMenuItem;
    menuSetting: TMenuItem;
    menuSettingCrossVisible: TMenuItem;
    menuSettingRate: TMenuItem;
    menuSettingInfo: TMenuItem;
    N5: TMenuItem;
    menuSettingInfoDecimal: TMenuItem;
    menuSettingInfoHex: TMenuItem;
    N6: TMenuItem;
    menuSettingInfoVisible: TMenuItem;
    procedure MouseTimerTimer(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure menuChangeScaleUpClick(Sender: TObject);
    procedure menuChangeScaleDownClick(Sender: TObject);
    procedure menuChangeScaleInputClick(Sender: TObject);
    procedure menuExitClick(Sender: TObject);
    procedure menuLeftSideRightClick(Sender: TObject);
    procedure menuUpSideDownClick(Sender: TObject);
    procedure menuVersionClick(Sender: TObject);
    procedure menuSettingCrossVisibleClick(Sender: TObject);
    procedure menuSettingInfoClick(Sender: TObject);
    procedure menuSettingRateClick(Sender: TObject);
    procedure menuSettingInfoDecimalClick(Sender: TObject);
    procedure menuSettingInfoHexClick(Sender: TObject);
  private
    // Variables
    FScaleWidth: Integer;
    FScaleHeight: Integer;
    FRealWidth: Integer;
    FRealHeight: Integer;
    FMidX: Integer;
    FMidY: Integer;
    FHalfX: Integer;
    FHalfY: Integer;
    FBmp: TBitmap;
    FFormShowing: Boolean;
    FRGB: String;
    // Methods
    procedure CalcSize;
    // Event Handlers
    procedure IniManagerChange(Sender: TObject);
    procedure ApplicationMessage(var vMsg: TMsg; var Handled: Boolean);
    // Message Handlers
    procedure WMEraseBkGnd(var vMsg: TWMEraseBkGnd); message WM_ERASEBKGND;
    procedure WMNCHitTest(var vMsg: TWMNCHitTest); message WM_NCHITTEST;
    procedure WMNCRButtonDown(var vMsg: TWMNCRButtonDown);
      message WM_NCRBUTTONDOWN;
  protected
    // Methods
    procedure CreateParams(var vParams: TCreateParams); override;
    procedure Paint; override;
    procedure Resize; override;
  public
  end;

var
  frmMain: TfrmMain;

implementation

uses
  Clipbrd, uGraphUtils, uFormUtils, uVersion, uIniManager;

{$R *.dfm}

{ TfrmMain }

procedure TfrmMain.CalcSize;
var
  tmpWidth, tmpHeight: Integer;
begin
  FScaleWidth := ClientWidth div IniManager.Scale;
  FScaleHeight := ClientHeight div IniManager.Scale;

  tmpWidth := FScaleWidth * IniManager.Scale;
  tmpHeight := FScaleHeight * IniManager.Scale;

  FRealWidth := FScaleWidth * IniManager.Scale;
  FRealHeight := FScaleHeight * IniManager.Scale;

  FMidX := Screen.DesktopLeft + Screen.DesktopWidth shr 1;
  FMidY := Screen.DesktopTop + Screen.DesktopHeight shr 1;

  FHalfX := ((tmpWidth div IniManager.Scale) shr 1) * IniManager.Scale;
  FHalfY := ((tmpHeight div IniManager.Scale) shr 1) * IniManager.Scale;

  if (FBmp.Width <> ClientWidth) then
    FBmp.Width := ClientWidth;

  if (FBmp.Height <> ClientHeight) then
    FBmp.Height := ClientHeight;
end;

procedure TfrmMain.CreateParams(var vParams: TCreateParams);
begin
  inherited;

  vParams.Style := vParams.Style or WS_THICKFRAME;
end;

procedure TfrmMain.FormCreate(Sender: TObject);
begin
  FBmp := TBitmap.Create;

  //menuCopy.Visible := False;
  //menuCut.Visible := False;

  BoundsRect := IniManager.BoundsRect;
  MouseTimer.Interval := IniManager.SamplingRate;

  menuLeftSideRight.Checked := IniManager.LeftSideRight;
  menuUpSideDown.Checked := IniManager.UpSideDown;
  menuSettingCrossVisible.Checked := IniManager.CrossVisible;
  menuSettingInfoVisible.Checked := IniManager.InfoVisible;

  if (IniManager.InfoIsHex) then
    menuSettingInfoHex.Checked := True
  else
    menuSettingInfoDecimal.Checked := True;

  IniManager.OnChange := IniManagerChange;

  CalcSize;

  with FBmp.Canvas do begin
    Font.Assign(Self.Font);

    Brush.Style := bsSolid;
    Brush.Color := clBlack;

    Font.Color := clWhite;

    SetBkColor(Handle, Brush.Color);
  end;

  MouseTimer.Enabled := True;

  Application.OnMessage := ApplicationMessage;
end;

procedure TfrmMain.FormDestroy(Sender: TObject);
begin
  IniManager.BoundsRect := BoundsRect;
end;

procedure TfrmMain.IniManagerChange(Sender: TObject);
begin
  CalcSize;
  MouseTimer.Interval := IniManager.SamplingRate;
  Paint;
end;

procedure TfrmMain.menuChangeScaleUpClick(Sender: TObject);
begin
  IniManager.Scale := IniManager.Scale + 1;
end;

procedure TfrmMain.menuChangeScaleDownClick(Sender: TObject);
begin
  IniManager.Scale := IniManager.Scale - 1;
end;

procedure TfrmMain.menuChangeScaleInputClick(Sender: TObject);
var
  tmpStr: String;
begin
  tmpStr := IntToStr(IniManager.Scale);

  FFormShowing := True;
  try
    if (InputQuery(Caption, 'Please input scale', tmpStr)) then
      IniManager.Scale := StrToIntDef(tmpStr, IniManager.Scale);
  finally
    FFormShowing := False;
  end;
end;

procedure TfrmMain.menuExitClick(Sender: TObject);
begin
  Close;
end;

procedure TfrmMain.menuLeftSideRightClick(Sender: TObject);
begin
  menuLeftSideRight.Checked := not menuLeftSideRight.Checked;
  IniManager.LeftSideRight := menuLeftSideRight.Checked;
end;

procedure TfrmMain.menuUpSideDownClick(Sender: TObject);
begin
  menuUpSideDown.Checked := not menuUpSideDown.Checked;
  IniManager.UpSideDown := menuUpSideDown.Checked;
end;

procedure TfrmMain.MouseTimerTimer(Sender: TObject);
begin
  if (not popupMenu.Popuping) and (not FFormShowing) then
    AlWaysStayOnTop(Self);

  Paint;
end;

procedure TfrmMain.Paint;
const
  LRFlag: array [Boolean] of String = ('', ' H');
  UDFlag: array [Boolean] of String = ('', ' V');
var
  MousePos: TPoint;
  Pos: TPoint;
  X, Y: Integer;
  P, Q, M, N: Integer; 
  tmpInt: Integer;
  tmpRect: TRect;
  LFlag: Integer;
  RFlag: Integer;
  TFlag: Integer;
  BFlag: Integer;
  Info: String;
  R, G, B: Byte;
  Prefix: String;
  Desktop: TRect;
  DC: HDC;
begin
  // Get Info
  MousePos := Mouse.CursorPos;
  Pos := MousePos;

  Desktop := Screen.DesktopRect;

  X := FHalfX;
  Y := FHalfY;

  Dec(Pos.X, FScaleWidth shr 1);
  Dec(Pos.Y, FScaleHeight shr 1);

  // Check Bounds
  LFlag := Desktop.Left - Pos.X;
  TFlag := Desktop.Top - Pos.Y;
  RFlag := Pos.X + FScaleWidth - Desktop.Right;
  BFlag := Pos.Y + FScaleHeight - Desktop.Bottom;

  // Left
  if (LFlag > 0) then begin
    Pos.X := Desktop.Left;
    Dec(X, LFlag * IniManager.Scale);
  end;

  // Top
  if (TFlag > 0) then begin
    Pos.Y := Desktop.Top;
    Dec(Y, TFlag * IniManager.Scale);
  end;

  // Right
  if (RFlag > 0) then begin
    Pos.X := Desktop.Right - FScaleWidth;
    Inc(X, RFlag * IniManager.Scale);
  end;

  // Bottom
  if (BFlag > 0) then begin
    Pos.Y := Desktop.Bottom - FScaleHeight;
    Inc(Y, BFlag * IniManager.Scale);
  end;

  // Draw
  with FBmp.Canvas do  begin
    Pen.Style := psClear;
    FillRect(ClientRect);
  end;

  DC := GetDC(0);
  try
    StretchBlt(
      FBmp.Canvas.Handle,
      0,
      0,
      FRealWidth,
      FRealHeight,
      DC,
      Pos.X,
      Pos.Y,
      FScaleWidth,
      FScaleHeight,
      SRCCOPY);

    StretchBlt(
      FBmp.Canvas.Handle,
      FRealWidth,
      0,
      FRealWidth + 1,
      FRealHeight,
      DC,
      Pos.X + FScaleWidth + 1,
      Pos.Y,
      1,
      FScaleHeight,
      SRCCOPY);

    StretchBlt(
      FBmp.Canvas.Handle,
      0,
      FRealHeight,
      FRealWidth,
      FRealHeight + 1,
      DC,
      Pos.X,
      Pos.Y + FScaleHeight + 1,
      FScaleWidth,
      1,
      SRCCOPY);

    StretchBlt(
      FBmp.Canvas.Handle,
      FRealWidth,
      FRealHeight,
      FRealWidth + 1,
      FRealHeight + 1,
      DC,
      Pos.X + FScaleWidth + 1,
      Pos.Y + FScaleHeight + 1,
      1,
      1,
      SRCCOPY);
  finally
    ReleaseDC(0, DC);
  end;

  // Draw
  with FBmp.Canvas do  begin
    // Draw Cross
    Pen.Style := psSolid;
    Pen.Color := clRed;
    Pen.Mode := pmNotXor;

    ToRGB(Pixels[X, Y], R, G, B);

    Prefix := '';
    if (menuSettingInfoHex.Checked) then begin
      FRGB := Format('%.2x%.2x%.2x', [R, G, B]);
      Prefix := '#';
    end
    else
      FRGB := Format('R:%.3d G:%.3d B:%.3d', [R, G, B]);

    tmpInt := IniManager.Scale shr 1;
    Inc(X, tmpInt);
    Inc(Y, tmpInt);

    if (IniManager.CrossVisible) then begin
      MoveTo(X, 0);
      LineTo(X, Height);

      MoveTo(0, Y);
      LineTo(Width, Y);
    end;

    // Frip
    P := 0;
    Q := 0;
    M := Width;
    N := Height;

    if (IniManager.LeftSideRight) then begin
      P := M - 1;
      M := -M;
    end;

    if (IniManager.UpSideDown) then begin
      Q := N - 1;
      N := -N;
    end;

    StretchBlt(
      Handle,
      P,
      Q,
      M,
      N,
      Handle,
      0,
      0,
      Width,
      Height,
      SRCCOPY);

    // Draw to Info
    Info :=
      Format(
        '(%d:%d)x%d ' + Prefix + FRGB + '%s%s',
        [
          MousePos.X,
          MousePos.Y,
          IniManager.Scale,
          LRFlag[IniManager.LeftSideRight],
          UDFlag[IniManager.UpSideDown]
        ]);

    tmpRect :=
      Rect(
        0,
        0,
        Canvas.TextWidth(Info) + 4,
        Canvas.TextHeight('H') + 4);

    if
      ((not IniManager.LeftSideRight) and (MousePos.X < FMidX)) or
      ((IniManager.LeftSideRight) and (MousePos.X > FMidX)) 
    then begin
      tmpRect.Left := ClientWidth - tmpRect.Right;
      tmpRect.Right := ClientWidth;
    end;

    if
      ((not IniManager.UpSideDown) and (MousePos.Y < FMidY)) or
      ((IniManager.UpSideDown) and (MousePos.Y > FMidY))
    then begin
      tmpRect.Top := ClientHeight - tmpRect.Bottom;
      tmpRect.Bottom := ClientHeight;
    end;

    if (IniManager.InfoVisible) then begin
      Rectangle(tmpRect);

      InflateRect(tmpRect, -1, -1);
      FillRect(tmpRect);

      TextOut(tmpRect.Left + 1, tmpRect.Top + 1, Info);
    end;
  end;

  // Copy to Surface
  Canvas.Draw(0, 0, FBmp);
end;

procedure TfrmMain.Resize;
begin
  inherited;

  CalcSize;
end;

procedure TfrmMain.WMEraseBkGnd(var vMsg: TWMEraseBkGnd);
begin
  vMsg.Result := 1;
  Paint;
end;

procedure TfrmMain.WMNCHitTest(var vMsg: TWMNCHitTest);
begin
  inherited;

  if (vMsg.Result = HTCLIENT) then
    vMsg.Result := HTCAPTION;
end;

procedure TfrmMain.WMNCRButtonDown(var vMsg: TWMNCRButtonDown);
begin
  inherited;

  popupMenu.Popup(vMsg.XCursor, vMsg.YCursor);
end;

procedure TfrmMain.menuVersionClick(Sender: TObject);
var
  Form: TfrmVersion;
begin
  Form := TfrmVersion.Create(Self);
  try
    Centering(Form, Self);

    FFormShowing := True;
    try
      Form.ShowModal;
    finally
      FFormShowing := False;
    end;
  finally
    Form.Release;
  end;
end;

procedure TfrmMain.menuSettingCrossVisibleClick(Sender: TObject);
begin
  menuSettingCrossVisible.Checked := not menuSettingCrossVisible.Checked;
  IniManager.CrossVisible := menuSettingCrossVisible.Checked;
end;

procedure TfrmMain.menuSettingInfoClick(Sender: TObject);
begin
  menuSettingInfoVisible.Checked := not menuSettingInfoVisible.Checked;
  IniManager.InfoVisible := menuSettingInfoVisible.Checked;
end;

procedure TfrmMain.menuSettingRateClick(Sender: TObject);
var
  tmpStr: String;
begin
  tmpStr := IntToStr(IniManager.SamplingRate);

  FFormShowing := True;
  try
    if (InputQuery(Caption, 'Please input sampling rate', tmpStr)) then
      IniManager.SamplingRate := StrToIntDef(tmpStr, IniManager.SamplingRate);
  finally
    FFormShowing := False;
  end;
end;

procedure TfrmMain.ApplicationMessage(var vMsg: TMsg; var Handled: Boolean);
begin
  with vMsg do
    if (message = WM_CHAR) and ((wParam = 3) or (wParam = 24)) then 
      Clipboard.AsText := FRGB;
end;

procedure TfrmMain.menuSettingInfoDecimalClick(Sender: TObject);
begin
  menuSettingInfoDecimal.Checked := True;
  IniManager.InfoIsHex := False;
end;

procedure TfrmMain.menuSettingInfoHexClick(Sender: TObject);
begin
  menuSettingInfoHex.Checked := True;
  IniManager.InfoIsHex := True;
end;

end.
