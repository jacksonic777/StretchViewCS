unit uIniManager;

interface

uses
  Windows, Classes;

type
  TIniManager = class(TObject)
  private
    // Variables
    FPath: String;
    FUpSideDown: Boolean;
    FLeftSideRight: Boolean;
    FScale: Integer;
    FBoundsRect: TRect;
    FOnChange: TNotifyEvent;
    FInfoVisible: Boolean;
    FCrossVisible: Boolean;
    FSamplingRate: Integer;
    FInfoIsHex: Boolean;
    // Methods
    procedure Read;
    procedure Write;
    procedure CallChangeHandler;
    procedure SetBoundsRect(const Value: TRect);
    procedure SetLeftSideRight(const Value: Boolean);
    procedure SetScale(const Value: Integer);
    procedure SetUpSideDown(const Value: Boolean);
    procedure SetCrossVisible(const Value: Boolean);
    procedure SetInfoVisible(const Value: Boolean);
    procedure SetSamplingRate(const Value: Integer);
    procedure SetInfoIsHex(const Value: Boolean);
  public
    // Constructor & Destructor
    constructor Create; reintroduce;
    destructor Destroy; override;
    // Properties
    property BoundsRect: TRect read FBoundsRect write SetBoundsRect;
    property Scale: Integer read FScale write SetScale;
    property LeftSideRight: Boolean read FLeftSideRight write SetLeftSideRight;
    property UpSideDown: Boolean read FUpSideDown write SetUpSideDown;
    property CrossVisible: Boolean read FCrossVisible write SetCrossVisible;
    property InfoVisible: Boolean read FInfoVisible write SetInfoVisible;
    property SamplingRate: Integer read FSamplingRate write SetSamplingRate;
    property InfoIsHex: Boolean read FInfoIsHex write SetInfoIsHex;
    // Events
    property OnChange: TNotifyEvent read FOnChange write FOnChange;
  end;

var
  IniManager: TIniManager;

implementation

uses
  Forms, SysUtils, IniFiles;

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

procedure TIniManager.Read;
begin
  with TIniFile.Create(FPath) do
    try
      FBoundsRect.Left := ReadInteger('BoundsRect', 'Left', 0);
      FBoundsRect.Right := ReadInteger('BoundsRect', 'Right', 300);
      FBoundsRect.Top := ReadInteger('BoundsRect', 'Top', 0);
      FBoundsRect.Bottom := ReadInteger('BoundsRect', 'Bottom', 300);

      FScale := ReadInteger('Lenz', 'Scale', 4);

      FLeftSideRight := ReadBool('Lenz', 'LeftSideRight', False);
      FUpSideDown := ReadBool('Lenz', 'UpSideDown', False);

      FCrossVisible := ReadBool('Lenz', 'CrossVisible', True);
      FInfoVisible := ReadBool('Lenz', 'InfoVisible', True);
      FInfoIsHex := ReadBool('Lenz', 'InfoIsHex', True);

      FSamplingRate := ReadInteger('Lenz', 'SamplingRate', 100);
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

procedure TIniManager.SetInfoVisible(const Value: Boolean);
begin
  if (FInfoVisible = Value) then
    Exit;

  FInfoVisible := Value;
  Write;
  CallChangeHandler;
end;

procedure TIniManager.SetLeftSideRight(const Value: Boolean);
begin
  if (FLeftSideRight = Value) then
    Exit;

  FLeftSideRight := Value;
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

procedure TIniManager.SetUpSideDown(const Value: Boolean);
begin
  if (FUpSideDown = Value) then
    Exit;

  FUpSideDown := Value;
  Write;
  CallChangeHandler;
end;

procedure TIniManager.Write;
begin
  with TIniFile.Create(FPath) do
    try
      WriteInteger('BoundsRect', 'Left', FBoundsRect.Left);
      WriteInteger('BoundsRect', 'Right', FBoundsRect.Right);
      WriteInteger('BoundsRect', 'Top', FBoundsRect.Top);
      WriteInteger('BoundsRect', 'Bottom', FBoundsRect.Bottom);

      WriteInteger('Lenz', 'Scale', FScale);

      WriteBool('Lenz', 'LeftSideRight', FLeftSideRight);
      WriteBool('Lenz', 'UpSideDown', FUpSideDown);

      WriteBool('Lenz', 'CrossVisible', FCrossVisible);
      WriteBool('Lenz', 'InfoVisible', FInfoVisible);
      WriteBool('Lenz', 'InfoIsHex', FInfoIsHex);

      WriteInteger('Lenz', 'SamplingRate', FSamplingRate);
    finally
      Free;
    end;
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
