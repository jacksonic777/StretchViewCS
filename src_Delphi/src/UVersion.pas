unit UVersion;

interface

uses Windows, SysUtils, Classes, Graphics, Forms, Controls, StdCtrls,
  Buttons, ExtCtrls;

type
  TVersion = record
    Major, Minor, Revision, Build: Cardinal;
    StringVersion: String;
  end;

   TfrmVersion = class(TForm)
    OKButton: TButton;
    Panel1: TPanel;
    Version: TLabel;
    Memo1: TMemo;
    Edit1: TEdit;
    Label1: TLabel;
    Memo2: TMemo;
    procedure FormCreate(Sender: TObject);

  private
    { Private éŒ¾ }
  public
    { Public éŒ¾ }


end;


var
  frmVersion: TfrmVersion;
  theVersion: TVersion;

implementation

{$R *.dfm}

//******************************************************************************
//
//
//
//
procedure LoadVersionInfo;
var
  sExeName: string;
  nSize, nRead: DWORD;
  pBuffer: PChar;
  pValue: PChar;
  tsStrings: TStringList;
begin
  sExeName := Application.ExeName;
  nSize := GetFileVersionInfoSize(PChar(sExeName), nSize);
  if nSize > 0 then begin
    pBuffer := AllocMem(nSize);
    try
      GetFileVersionInfo(PChar(sExeName), 0, nSize, pBuffer);
      if VerQueryValue(pBuffer, PChar('StringFileInfo\041103A4\FileVersion'), Pointer(pValue), nRead) then
      begin
        tsStrings := TStringList.Create;
        try
          ExtractStrings(['.'], [], pValue, tsStrings);
          with theVersion do begin
            Major := StrToInt(tsStrings[0]);
            Minor := StrToInt(tsStrings[1]);
            Revision := StrToInt(tsStrings[2]);
            Build := StrToInt(tsStrings[3]);
            StringVersion := pValue;
          end;
        finally
          tsStrings.Free;
        end;
      end;
    finally
      FreeMem(pBuffer, nSize);
    end;
  end;
end;

procedure TfrmVersion.FormCreate(Sender: TObject);
begin
   memo1.Text := theversion.StringVersion;
end;

initialization
  LoadVersionInfo;

end.




