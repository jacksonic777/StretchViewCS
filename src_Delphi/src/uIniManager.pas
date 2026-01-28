unit uIniManager;

interface

uses
  Windows, Classes;

type
  TIniManager = class(TObject)
  private

    // Methods
    procedure Read;
    procedure Write;
    //
    procedure CallChangeHandler;
    procedure SetBoundsRect(const Value: TRect);
    procedure SetHFlip(const Value: Boolean);
    procedure SetScale(const Value: Integer);

    procedure SetCapRate(const Value: Single);
    procedure SetScaleWidth(const Value: Integer);
    procedure SetScaleHeight(const Value: Integer);

    procedure SetVFlip(const Value: Boolean);
    procedure SetCrossVisible(const Value: Boolean);
    procedure SetSamplingRate(const Value: Integer);
    procedure SetInfoIsHex(const Value: Boolean);
  public
    // Variables
    FPath: String;
    FVFlip: Boolean;
    FHFlip: Boolean;
    FScale: Integer;
    FFirstRun : Boolean;
    FRunCount : Integer;
    FLisenceKey : String;
    FBoundsRect: TRect;
    FOnChange: TNotifyEvent;
    FInfoVisible: Boolean;
    FCrossVisible: Boolean;
    FSamplingRate: Integer;
    FInfoIsHex: Boolean;
    FScaleWidth : Integer;
    FScaleHeight:Integer;
    FCapRate : Single;

    FFixView : Boolean;
    FFixViewX: Integer;
    FFixViewY: Integer;

    // Constructor & Destructor
    constructor Create; reintroduce;
    destructor Destroy; override;
    // Properties
    property BoundsRect: TRect read FBoundsRect write SetBoundsRect;
    property Scale: Integer read FScale write SetScale;

    // @
    property CapRate: Single read FCapRate write SetCapRate;
    property ScaleWidth: Integer read FScaleWidth write SetScaleWidth;
    property ScaleHeight: Integer read FScaleHeight write  SetScaleHeight;

    property HFlip: Boolean read FHFlip write SetHFlip;
    property VFlip: Boolean read FVFlip write SetVFlip;
    property CrossVisible: Boolean read FCrossVisible write SetCrossVisible;
    property SamplingRate: Integer read FSamplingRate write SetSamplingRate;
    property InfoIsHex: Boolean read FInfoIsHex write SetInfoIsHex;
    // Events
    property OnChange: TNotifyEvent read FOnChange write FOnChange;
  end;

var
  IniManager: TIniManager;

implementation

uses
  Forms, SysUtils, IniFiles,Math;

{ TIniManager }

procedure TIniManager.CallChangeHandler;
begin
  if (Assigned(FOnChange)) then
    FOnChange(Self);
end;

constructor TIniManager.Create;
begin
  inherited Create;

  FPath := ChangeFileExt(Application.ExeName, '.ini');

  Read;
end;

destructor TIniManager.Destroy;
begin
  Write;

  inherited;
end;
//****************************************************************
//
//                              読み込み
//
procedure TIniManager.Read;
var
  Section : string;
begin
  with TIniFile.Create(FPath) do
    try
      Section := 'BoundsRect';
      FBoundsRect.Left   := ReadInteger(Section, 'Left', 300);
      FBoundsRect.Right  := ReadInteger(Section, 'Right', 800);
      FBoundsRect.Top    := ReadInteger(Section, 'Top', 100);
      FBoundsRect.Bottom := ReadInteger(Section, 'Bottom', 600);
      Section := 'Lenz';
      FScale := ReadInteger(Section, 'Scale', 4);

      // 2006.12.26 初回起動かどうか
      FFirstRun := ReadBool('Setting', 'FirstRun', true);
      // ライセンスキー
      FLisenceKey := ReadString('Setting','LisenceKey', '');
      // 起動回数
      FRunCount := ReadInteger('Setting', 'RunCount', 1);


      FHFlip := ReadBool(Section, 'HFlip', False);
      FVFlip := ReadBool(Section, 'VFlip', False);

      FScaleWidth := ReadInteger(Section, 'ScaleWidth', 300);
      FScaleHeight := ReadInteger(Section, 'ScaleHeight', 300);

      FCapRate := ReadInteger(Section, 'CapRate', 10) ;
      FCapRate := (FCapRate / 10);



      FCrossVisible := ReadBool(Section, 'CrossVisible', True);
      FInfoIsHex    := ReadBool(Section, 'InfoIsHex', True);
      FSamplingRate := ReadInteger(Section, 'SamplingRate', 100);
      // 表示固定
      FFixView := ReadBool(Section,'FixView',False);
      FFixViewX:= ReadInteger(Section,'FixViewX', 200);
      FFixViewY:= ReadInteger(Section,'FixViewY',200);


    finally
      Free;
    end;
end;

procedure TIniManager.Write;
var
   Section : string;
begin
  with TIniFile.Create(FPath) do
    try
      WriteInteger('BoundsRect', 'Left', FBoundsRect.Left);
      WriteInteger('BoundsRect', 'Right', FBoundsRect.Right);
      WriteInteger('BoundsRect', 'Top', FBoundsRect.Top);
      WriteInteger('BoundsRect', 'Bottom', FBoundsRect.Bottom);

      WriteInteger('Lenz', 'Scale', FScale);

      WriteBool('Lenz', 'HFlip', FHFlip);
      WriteBool('Lenz', 'VFlip', FVFlip);

      Section := 'Lenz';
      WriteInteger(Section, 'ScaleWidth', FScaleWidth);
      WriteInteger(Section, 'ScaleHeight', FScaleHeight);

      WriteInteger(Section, 'CapRate',  Math.floor(FCapRate *10));

      // 2006.12.26
      // 初回起動かどうか
      WriteBool('Setting', 'FirstRun'  , false);
      // ライセンスキー
      WriteString('Setting', 'LisenceKey', FLisenceKey);
      // 起動回数インクリメント
      Inc(FRunCount);

      // 起動回数
      WriteInteger('Setting', 'RunCount', FRunCount);

      WriteBool('Lenz', 'CrossVisible', FCrossVisible);
      WriteBool('Lenz', 'InfoVisible', FInfoVisible);
      WriteBool('Lenz', 'InfoIsHex', FInfoIsHex);

      WriteInteger('Lenz', 'SamplingRate', FSamplingRate);

      //
      WriteBool(Section, 'FixView', FFixView);
      WriteInteger(Section, 'FixViewX', FFixViewX);
      WriteInteger(Section, 'FixViewY',FFixViewY);

    finally
      Free;
    end;
end;
procedure TIniManager.SetBoundsRect(const Value: TRect);
begin
  FBoundsRect := Value;
  Write;
end;

procedure TIniManager.SetCrossVisible(const Value: Boolean);
begin
  if (FCrossVisible = Value) then
    Exit;

  FCrossVisible := Value;
  Write;
  CallChangeHandler;
end;

procedure TIniManager.SetInfoIsHex(const Value: Boolean);
begin
  if (FInfoIsHex = Value) then
    Exit;

  FInfoIsHex := Value;
  Write;
  CallChangeHandler;
end;

procedure TIniManager.SetCapRate(const Value: Single);
begin
  if (FCapRate= Value) then
    Exit;

  FCapRate := Value;
  Write;
  CallChangeHandler;
end;

procedure TIniManager.SetScaleWidth(const Value: Integer);
begin
  if (FScaleWidth = Value) then
    Exit;

  FScaleWidth := Value;
  Write;
  CallChangeHandler;
end;
procedure TIniManager.SetScaleHeight(const Value: Integer);
begin
  if (FScaleHeight = Value) then
    Exit;

  FScaleHeight := Value;
  Write;
  CallChangeHandler;
end;
procedure TIniManager.SetHFlip(const Value: Boolean);
begin
  if (FHFlip = Value) then
    Exit;

  FHFlip := Value;
  Write;
  CallChangeHandler;
end;

procedure TIniManager.SetSamplingRate(const Value: Integer);
begin
  if (FSamplingRate = Value) then
    Exit;

  FSamplingRate := Value;
  Write;
  CallChangeHandler;
end;

procedure TIniManager.SetScale(const Value: Integer);
begin
  if (FScale = Value) then
    Exit;

  FScale := Value;

  if (FScale < 1) then
    FScale := 1;

  Write;
  CallChangeHandler;
end;

procedure TIniManager.SetVFlip(const Value: Boolean);
begin
  if (FVFlip = Value) then
    Exit;

  FVFlip := Value;
  Write;
  CallChangeHandler;
end;


initialization
begin
  IniManager := TIniManager.Create;
end;

finalization
begin
  IniManager.Free;
end;

end.
